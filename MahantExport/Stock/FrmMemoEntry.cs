using MahantExport.Class;
using MahantExport.Utility;
using BarcodeLib.Barcode;
using BusLib;
using BusLib.Configuration;
using BusLib.EInvoice;
using BusLib.Master;
using BusLib.Rapaport;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using QRCoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows.Forms;
using TaxProEInvoice.API;
using MahantExport.Master;
//BtnOtherActivity
namespace MahantExport.Stock
{
    public partial class FrmMemoEntry : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_EInvoice ObjEInvoice = new BOTRN_EInvoice();
        BOFormPer ObjPer = new BOFormPer();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();
        BOTRN_RoughPurchase ObjMast = new BOTRN_RoughPurchase();

        DataTable DtabInvoice = new DataTable();

        DataTable DTabMemoDetail = new DataTable();
        DataTable DTabMemoSummury = new DataTable();
        DataTable DTabMemo = new DataTable();
        DataTable DtabPara = new DataTable();
        DataTable DtabProcess = new DataTable();
        DataTable DTabAccTrnType = new DataTable();
        DataTable DtabRapaport = new DataTable();
        DataTable DTabMemoDetailParcelFile = new DataTable();
        DataTable DTFileParcel = new DataTable();

        public DataTable DTabMemoStock;

        string strAccTrnType = "";
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
        string PrevMemoId = ""; //b Part Status Update
        string PstrOrder_MemoId = ""; // b part mathi sales ni entry thay tyare pending order ni detail save thase.  
        string PstrOrderJangedNo = "";
        string PstrRemark = "";
        DataTable DTabOld = new DataTable();
        string MemoEntry_OldDB = string.Empty;
        string PstrTrnNo_OldDB = "";
        string PStrD_Code = "";
        int PIntTrnNo = 0;
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
        double DouInvoiceAmt = 300000; // Add Khushbu 19-09-21
        double DouInvoiceAmtFE = 500000; // Add Khushbu 19-09-21

        //#K : 15122020
        DataTable DtAccountingEffect = new DataTable();
        DataTable DtBrokerAccountingEffect = new DataTable();
        BODevGridSelection ObjGridSelection;
        string StrMainBillType = "";
        string StrMainACCType = "";

        //HINA - START
        string mStrStockType = "";
        //HINA - END

        //For E-Invoice
        string AuthToken = "";
        string Skey = "";
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
        bool IsChangeBrokPer = false, IsOutSideStone = false, IsEInvoiceDone = false;
        string MenuTagName = "", SaleInvoiceMemoID = "";

        DataTable DtExportAccountingEffect = new DataTable(); // add Khushbu 18-06-21

        eInvoiceSession eInvSession = new eInvoiceSession(true, true); // add shiv 09-05-2022
        string PartyName = "";
        string InvoiceNo = "";
        bool IsManualEntryAdd = false;

        #region ::"Third Party Vairable"::

        string GspName = "", AspUserId = "", AspPassword = "", ClientId = "", ClientSecret = "", AuthUrl = "",
            BaseUrl = "", EwbBaseUrl = "", CancelEWB = "", UserName = "", Password = "", Gstin = "", AppKey = "", Sek = "", TokenExp = "", rtbResponce = "";
        DataTable dt = new DataTable();
        DataTable DtabOutSideStone = new DataTable();
        DataTable DtLedger;
        bool Autosave = false;
        bool AutoEntry = false;
        //Add shiv 01-07-2022
        double dblTCSAmount = Convert.ToDouble(new BusLib.Master.BOMST_FormPermission().GetTDSAmount());
        bool IS_MAXLIMIT = false;
        bool IS_CONSIRET = false;
        #endregion

        #region Property Settings
        public FrmMemoEntry()
        {
            InitializeComponent();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            this.WindowState = FormWindowState.Maximized;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            ResizeForm(this, screenHeight, screenWidth);
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
            CONSIGNMENTRETURN = 16,
            UNGRADEDTOMIX = 17,
            GRADEDTOMIX = 18
        }
        public void ShowForm(FORMTYPE pFormType, DataTable pDtInvoice, string pStockType = "ALL", string StrMainMemo_ID = "")  //Call When Double Click On Current Stock Color Clarity Wise Report Data
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            DataSet DS = new DataSet();
            BtnReturn.Enabled = false;
            BtnOtherActivity.Enabled = false;

            mFormType = pFormType;
            mStrStockType = pStockType;

            StrMainMemo_ID = Val.ToString(StrMainMemo_ID).Trim().Equals(string.Empty) ? Val.ToString(Guid.Empty) : StrMainMemo_ID;

            DS = ObjMemo.GetMemoListData(-1, null, null, "", "", "", 0, "", 0, "", "", "", mStrStockType, false, -1);

            //DouInvoiceAmt = 300000;

            DTabMemo = DS.Tables[0];
            DTabMemoDetail = DS.Tables[1];
            DTabMemoDetailParcelFile = DS.Tables[2];

            DTabMemoDetail.Columns["LOCATIONNAME"].DataType = typeof(string);
            DTabMemoDetail.Columns.Add("OLDMEMODISCOUNT", typeof(double));
            DTabMemoDetail.Columns.Add("OLDMEMOPRICEPERCARAT", typeof(double));

            DTabMemoDetailParcelFile.Columns["LOCATIONNAME"].DataType = typeof(string);
            DTabMemoDetailParcelFile.Columns.Add("OLDMEMODISCOUNT", typeof(double));
            DTabMemoDetailParcelFile.Columns.Add("OLDMEMOPRICEPERCARAT", typeof(double));

            GrdDetail.BeginUpdate();
            GrdSummury.BeginUpdate();

            MainGrdDetail.DataSource = DTabMemoDetail;
            GrdDetailParcel.BestFitColumns();// Add By Urvisha 
            MainGrdDetail.Refresh();

            MainGrdDetailParcel.DataSource = DTabMemoDetailParcelFile;
            MainGrdDetailParcel.Refresh();

            cmbAddresstype.SelectedIndex = 1; //#P : 14-07-2021
            if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE)
            {
                panel5.Visible = false;
                TotalPcs.Visible = false;
                txtTotalPcs.Visible = true;
                cLabel25.Visible = false;
                txtTotalCarat.Visible = true;
                cLabel53.Visible = false;
                lblTotalAvgDisc.Visible = false;
                cLabel33.Visible = false;
                lblTotalAvgRate.Visible = false;
                cLabel52.Visible = false;
                lblTotalAmount.Visible = false;
                xtraTabMasterPanel.TabPages[1].PageVisible = false;
                //DTabMemo.Rows[0]["REMARK"] = "";
            }

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
            GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = true;
            GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
            //#P : 05-05-2021


            lblSource.Text = "SOFTWARE";
            CmbPaymentMode.SelectedIndex = 0;
            CmbDeliveryType.SelectedIndex = 0;
            cmbAddresstype.SelectedIndex = 1; //#P : 14-07-2021
            cmbBillType.SelectedIndex = 0;
            cmbAccType.SelectedIndex = 0;
            lblMode.Text = "Add Mode";
            FillControlName();

            DTabMemoDetail.Rows.Clear();

            if (pDtInvoice == null && mFormType == FORMTYPE.PURCHASEISSUE)// Coz InvCurr Blank hoy to default Usd consider kare. Add Khushbu 04-02-22
            {
                DataTable DTabCurr = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);
                foreach (DataRow DRow in DTabCurr.Rows)
                {
                    if (Val.ToString(DRow["CURRENCY_ID"]) == "1")
                    {
                        txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                        txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);
                    }
                }
                DTabCurr.Dispose();
                DTabCurr = null;

                txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString()), mFormType.ToString()).ToString();
            }


            if (pDtInvoice != null)
            {
                DataTable DTabCurr = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);

                foreach (DataRow DRow in DTabCurr.Rows)
                {
                    if (pDtInvoice != null)
                    {
                        if ((pFormType == FORMTYPE.SALEINVOICE || pFormType == FORMTYPE.ORDERCONFIRMRETURN || pFormType == FORMTYPE.RELEASE) && Val.ToString(DRow["CURRENCY_ID"]) == pDtInvoice.Rows[0]["INVCURRENCY_ID"].ToString())

                        {
                            txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                            txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);
                        }
                        else if (Val.ToString(DRow["CURRENCY_ID"]) == "1")//#P : 09-07-2021 : Coz InvCurr Blank hoy to default Usd consider kare
                        {
                            txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                            txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);
                        }
                    }
                    else if (Val.ToString(DRow["CURRENCY_ID"]) == "1")
                    {
                        txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                        txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);
                    }
                }

                DTabCurr.Dispose();
                DTabCurr = null;

                //Comment by Gunjan: 21 / 11 / 2024 #: as per discussion with brijesh bhai order confirm and sale delivery ma je exc rate by default aave chhe ae zero karvano
                if (txtExcRate.Text.Length == 0)
                {
                    txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString()), mFormType.ToString()).ToString();
                }
                //End as Gunjan:21 / 11 / 2024
                if (BOConfiguration.gStrLoginSection != "B" && (mFormType == FORMTYPE.SALEINVOICE))
                {
                    foreach (DataRow DRow in pDtInvoice.DefaultView.ToTable(true, "MEMO_ID", "ORDERMEMO_ID", "OrderJangedNo").Rows)
                    {
                        PrevMemoId = PrevMemoId + "," + Val.ToString(DRow["MEMO_ID"]); // USED FOR UPDATE ISAPPROVE COLUMN
                        PstrOrder_MemoId = PstrOrder_MemoId + "," + Val.ToString(DRow["OrderMemo_ID"]);
                        PstrOrderJangedNo = PstrOrderJangedNo + "," + Val.ToString(DRow["OrderJangedNo"]);

                        if (PrevMemoId.Substring(0, 1) == ",") // remove first character ','
                        {
                            PrevMemoId = PrevMemoId.Substring(1);
                            PstrOrder_MemoId = PstrOrder_MemoId.Substring(1);
                            PstrOrderJangedNo = PstrOrderJangedNo.Substring(1);
                        }
                    }
                }
                string StrBillParty = Val.ToString(pDtInvoice.Rows[0]["BILLPARTY_ID"]);
                string StrShipParty = Val.ToString(pDtInvoice.Rows[0]["SHIPPARTY_ID"]);

                txtInvoiceNo.Text = Val.ToString(pDtInvoice.Rows[0]["INVOICENO"]);

                if (StrBillParty != "" && StrBillParty != "00000000-0000-0000-0000-000000000000"
                    && (mFormType == FORMTYPE.PURCHASERETURN || Val.ToString(pDtInvoice.Rows[0]["STATUS"]) != "AVAILABLE")) //Changes : Pinali : 07-11-2019 : Add Only status Condtn
                {
                    DataRow DRow = new BOMST_Ledger().GetDataByPK(StrBillParty);
                    if (DRow != null)
                    {
                        txtBillingParty.Text = Val.ToString(DRow["LEDGERNAME"]);
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
                foreach (DataRow DRow in pDtInvoice.Rows)
                {
                    if (PstrRemark != "")
                    {
                        txtRemark.Text = PstrRemark;
                    }
                    else
                    {
                        txtRemark.Text = Val.ToString(pDtInvoice.Rows[0]["MASTREMARK"]);
                    }

                    DataRow DRNew = DTabMemoDetail.NewRow();

                    DRNew["MEMODETAIL_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                    if (Val.ToString(DRow["STOCKTYPE"]) == "PARCEL")
                    {
                        DRNew["PREVMEMODETAIL_ID"] = DRow["PREVMEMODETAIL_ID"];  //Add shiv
                        DRNew["PREVMEMO_ID"] = DRow["PREVMEMO_ID"];
                    }
                    else
                    {
                        DRNew["PREVMEMODETAIL_ID"] = DRow["MEMODETAIL_ID"];  //Add shiv
                    }
                    DRNew["STOCK_ID"] = DRow["STOCK_ID"];
                    DRNew["STOCKNO"] = DRow["STOCKNO"];
                    DRNew["PARTYSTOCKNO"] = DRow["PARTYSTOCKNO"];
                    DRNew["STOCKTYPE"] = DRow["STOCKTYPE"];
                    DRNew["FINALBUYER_ID"] = DRow["FINALBUYER_ID"];
                    DRNew["FINALBUYERNAME"] = DRow["FINALBUYERNAME"];

                    //if (mStrStockType == "PARCEL" && (Val.ToString(StrMainMemo_ID).Equals(string.Empty) || Val.ToString(StrMainMemo_ID) == Val.ToString(Guid.Empty))) // Only IF Condtn : #P : 17-01-2020
                    //{
                    DRNew["PCS"] = DRow["BALANCEPCS"];
                    DRNew["BALANCEPCS"] = DRow["BALANCEPCS"];
                    DRNew["CARAT"] = DRow["BALANCECARAT"];
                    DRNew["BALANCECARAT"] = DRow["BALANCECARAT"];
                    //}
                    //else
                    //{
                    //    if (Val.Val(DRow["MEMOPENDINGCARAT"]) != 0)
                    //    {
                    //        DRNew["CARAT"] = DRow["MEMOPENDINGCARAT"];
                    //        DRNew["BALANCECARAT"] = DRow["MEMOPENDINGCARAT"];
                    //    }

                    //    if (Val.Val(DRow["MEMOPENDINGPCS"]) != 0)
                    //    {
                    //        DRNew["PCS"] = DRow["MEMOPENDINGPCS"];
                    //        DRNew["BALANCEPCS"] = DRow["MEMOPENDINGPCS"];
                    //    }
                    //    if (Val.Val(DRow["MEMOPENDINGCARAT"]) == 0)
                    //    {
                    //        DRNew["CARAT"] = DRow["BALANCECARAT"];
                    //        DRNew["BALANCECARAT"] = DRow["BALANCECARAT"];
                    //    }
                    //    if (Val.Val(DRow["MEMOPENDINGPCS"]) == 0)
                    //    {
                    //        DRNew["PCS"] = DRow["BALANCEPCS"];
                    //        DRNew["BALANCEPCS"] = DRow["BALANCEPCS"];

                    //    }
                    //}


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
                    if (mFormType == FORMTYPE.PURCHASEISSUE)
                    {
                        DRNew["FSALEPRICEPERCARAT"] = DRow["FSALEPRICEPERCARAT"];
                        if (BOConfiguration.gStrLoginSection == "B")
                        {
                            DRNew["FSALEAMOUNT"] = Math.Round((Val.Val(DRow["FSALEAMOUNT"])) / 1000, 3); //#P : 27-01-2022
                        }
                        else
                        {
                            DRNew["FSALEAMOUNT"] = Math.Round(Val.Val(DRow["FSALEAMOUNT"]), 3); //#P : 27-01-2022
                        }
                    }
                    // #D: 13-01-2021
                    if (mFormType == FORMTYPE.LABRETURN)
                    {
                        DRNew["LABSERVICECODE_ID"] = DRow["LABSERVICECODE_ID"];
                        DRNew["LABSERVICECODE"] = DRow["LABSERVICECODE"];
                    }
                    // #D: 13-01-2021

                    if (Val.ToString(DRow["STATUS"]).ToUpper() == "AVAILABLE") //Changed : #P :30-03-2020
                    {
                        DRNew["MEMORAPAPORT"] = DRow["SALERAPAPORT"];
                        DRNew["MEMODISCOUNT"] = DRow["SALEDISCOUNT"];
                        DRNew["MEMOPRICEPERCARAT"] = DRow["SALEPRICEPERCARAT"];
                        DRNew["MEMOAMOUNT"] = DRow["SALEAMOUNT"];

                        //DRNew["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["EXCRATE"]) * Val.Val(DRow["SALEPRICEPERCARAT"]), 3); //#P : 27-01-2022
                        //DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["EXCRATE"]) * Val.Val(DRow["SALEAMOUNT"]), 3); //#P : 27-01-2022
                        DRNew["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["SALEPRICEPERCARAT"]), 3); //#P : 27-01-2022
                        DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["SALEAMOUNT"]), 3); //#P : 27-01-2022

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
                        DRNew["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["SALEPRICEPERCARAT"]), 3); //#P : 27-01-2022
                        DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["SALEAMOUNT"]), 3); //#P : 27-01-2022

                        //DRNew["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["EXCRATE"]) * Val.Val(DRow["MEMOPRICEPERCARAT"]), 3); //#P : 27-01-2022
                        //if (BOConfiguration.gStrLoginSection == "B")
                        //{
                        //    DRNew["FMEMOAMOUNT"] = Math.Round((Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"])) / 1000, 3); //#P : 27-01-2022
                        //}
                        //else
                        //{
                        //    DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["EXCRATE"]) * Val.Val(DRow["MEMOAMOUNT"]), 3); //#P : 27-01-2022
                        //}


                        DRNew["JANGEDRAPAPORT"] = DRow["MEMORAPAPORT"];
                        DRNew["JANGEDDISCOUNT"] = DRow["MEMODISCOUNT"];
                        DRNew["JANGEDPRICEPERCARAT"] = DRow["MEMOPRICEPERCARAT"];
                        DRNew["JANGEDAMOUNT"] = DRow["MEMOAMOUNT"];

                    }


                    //#P : 07-06-2022
                    DRNew["EXPINVOICERATE"] = DRow["EXPINVOICERATE"];
                    DRNew["EXPINVOICERATEFE"] = DRow["EXPINVOICERATEFE"];
                    DRNew["EXPINVOICEAMT"] = DRow["EXPINVOICEAMT"];
                    DRNew["EXPINVOICEAMTFE"] = DRow["EXPINVOICEAMTFE"];

                    DRNew["COSTRAPAPORT"] = DRow["COSTRAPAPORT"];
                    DRNew["COSTDISCOUNT"] = DRow["COSTDISCOUNT"];
                    DRNew["COSTPRICEPERCARAT"] = DRow["COSTPRICEPERCARAT"];
                    DRNew["COSTAMOUNT"] = DRow["COSTAMOUNT"];
                    //eND : 07-06-2022

                    DRNew["STATUS"] = "PENDING";
                    DRNew["REMARK"] = DRow["DETREMARK"];
                    DRNew["LOCATION_ID"] = DRow["LOCATION_ID"];
                    DRNew["LOCATIONNAME"] = DRow["LOCATIONNAME"];
                    DRNew["SIZE_ID"] = DRow["SIZE_ID"];

                    DRNew["SIZENAME"] = DRow["SIZENAME"];
                    DRNew["LAB_ID"] = DRow["LAB_ID"];
                    DRNew["LABNAME"] = DRow["LABNAME"];
                    DRNew["ISPURCHASE"] = DRow["ISPURCHASE"];
                    DRNew["LABREPORTNO"] = DRow["LABREPORTNO"];
                    DRNew["MAINMEMO_ID"] = StrMainMemo_ID;
                    DRNew["EXCRATE"] = Val.Val(txtExcRate.Text);//DRow["EXCRATE"];
                    DRNew["DIAMONDTYPE"] = DRow["DIAMONDTYPE"];
                    DTabMemoDetail.Rows.Add(DRNew);
                }

                cmbBillType.Text = pDtInvoice.Rows[0]["INVBILLTYPE"].ToString();
                cmbAccType.Text = pDtInvoice.Rows[0]["INVACCTYPE"].ToString();

                if (Val.ToString(pDtInvoice.Rows[0]["BILLFORMAT"]) == RbDollar.Tag.ToString())
                {
                    RbDollar.Checked = true;
                    RbDollar_CheckedChanged(null, null);
                }
                else
                {
                    RbRupee.Checked = true;
                    RbRupee_CheckedChanged(null, null);
                }

                if (cmbBillType.Text.ToUpper() == "EXPORT" && mFormType == FORMTYPE.SALEINVOICE) //#P : 07-06-2022
                {
                    ChkUpdExport.Checked = true;
                }

                txtBuyer.Text = Val.ToString(pDtInvoice.Rows[0]["FINALBUYERNAME"]);
                txtBuyer.Tag = Val.ToString(pDtInvoice.Rows[0]["FINALBUYER_ID"]);
                //chkIsConsingee.Checked = Val.ToBoolean(pDtInvoice.Rows[0]["ISCONSINGEE"]);

                // #D: 13-01-2021
                if (mFormType == FORMTYPE.LABRETURN)
                {
                    txtLabServiceCode.Text = Val.ToString(pDtInvoice.Rows[0]["LABSERVICECODE"]);
                    txtLabServiceCode.Tag = Val.ToInt32(pDtInvoice.Rows[0]["LABSERVICECODE_ID"]);
                }
                // #D: 13-01-2021


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

            IsChangeBrokPer = true;
            lblMemoNo.Text = string.Empty;
            DTPMemoDate.Value = DateTime.Now;
            txtSellerName.Text = BOConfiguration.gEmployeeProperty.LEDGERNAME;
            txtSellerName.Tag = BOConfiguration.gEmployeeProperty.LEDGER_ID;

            //#S :15-03-2022 ADD B PART EXCRATE COPY TO BORKEREXCRATE
            if (cmbAccType.SelectedIndex == 3 || cmbAccType.SelectedIndex == 4 || cmbAccType.SelectedIndex == 2)
            {
                txtBrokerExcRate.Text = pDtInvoice.Rows[0]["INVEXCRATE"].ToString();
            }

            //Add : #D : 20-06-2020
            if ((mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRMRETURN || mFormType == FORMTYPE.RELEASE || mFormType == FORMTYPE.PURCHASEISSUE) && pDtInvoice != null)
            //if (pDtInvoice != null)
            {
                txtBroker.Text = pDtInvoice.Rows[0]["INVBROKERNAME"].ToString();
                txtBroker.Tag = pDtInvoice.Rows[0]["INVBROKER_ID"];
                txtBaseBrokeragePer.Text = pDtInvoice.Rows[0]["INVBROKRAGEPER"].ToString();
                txtProfitBrokeragePer.Text = pDtInvoice.Rows[0]["INVBROKRAGEPROFITPER"].ToString();
                txtExcRate.Text = Val.Val(pDtInvoice.Rows[0]["INVEXCRATE"]) != 0 ? pDtInvoice.Rows[0]["INVEXCRATE"].ToString() : Val.ToString(txtExcRate.Text);
                txtSellerName.Text = pDtInvoice.Rows[0]["INSELLERNAME"].ToString();
                txtSellerName.Tag = pDtInvoice.Rows[0]["INVSELLER_ID"];
                txtTerms.Tag = pDtInvoice.Rows[0]["INVTERMS_ID"];
                txtTerms.Text = pDtInvoice.Rows[0]["INVTERMSNAME"].ToString();
                DTPTermsDate.Text = pDtInvoice.Rows[0]["INVTERMSDATE"].ToString();
                txtTermsDays.Text = pDtInvoice.Rows[0]["INVTERMSDAYS"].ToString();
                //txtTransport.Text = pDtInvoice.Rows[0]["INVTRANSPORTNAME"].ToString();
                CmbDeliveryType.Text = pDtInvoice.Rows[0]["INVDELIVERYTYPE"].ToString();
                CmbPaymentMode.Text = pDtInvoice.Rows[0]["INVPAYMENTMODE"].ToString();
                cmbBillType.Text = pDtInvoice.Rows[0]["INVBILLTYPE"].ToString();
                cmbAccType.Text = pDtInvoice.Rows[0]["INVACCTYPE"].ToString();
                CmbMemoType.Text = pDtInvoice.Rows[0]["INVMEMOTYPE"].ToString();
                txtExcRate_Validated(null, null);
                //Coz : Pending mathi AddDelv time pr BillFormat db mathi select thy ne natu aavtu
                if (Val.ToString(pDtInvoice.Rows[0]["BILLFORMAT"]) == RbDollar.Tag.ToString())
                {
                    RbDollar.Checked = true;
                    RbDollar_CheckedChanged(null, null);
                }
                else
                {
                    RbRupee.Checked = true;
                    RbRupee_CheckedChanged(null, null);
                }
            }

            if (mFormType == FORMTYPE.LABISSUE)
            {
                txtLabServiceCode.Visible = true;
                BtnApplyAll.Visible = true;
                lblLabServiceCode.Visible = true;
                GrdDetail.Bands["BandLabService"].Visible = false;
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


            CreateSummaryTable();
            if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN)
            {

                DataRow Drow = DTabMemoSummury.NewRow();
                DTabMemoSummury.Rows.Add(Drow);


                GetSummuryDetailForGrid();
                MainGridSummury.DataSource = DTabMemoSummury;
                MainGridSummury.Refresh();
                GrdSummury.BestFitColumns();//Add By Urvisha :12-02-2024
                RbDollar_CheckedChanged(null, null);
            }
            else
            {
                xtraTabControl1.TabPages.Remove(StoneSummaryTab);
                MainGridSummury.DataSource = null;
                GrdSummury.BestFitColumns();//Add By Urvisha :12-02-2024
                MainGridSummury.Refresh();
            }

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
                xtraTabControl1.TabPages.Remove(StoneSummaryTab);
                MainGridSummury.DataSource = null;
                MainGridSummury.Refresh();
            }

            //#K: 05122020
            ChkApprovedOrder.Visible = false;
            //if (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.CONSIGNMENTISSUE)
            //    ChkApprovedOrder.Visible = true;

            //SHIV 26-12-22
            lblType.Visible = false;
            cmbAccType.Visible = false;
            btnTrialEInvoice.Visible = false;
            btnAccPrint.Visible = false;
            if (mFormType == FORMTYPE.SALEINVOICE && MenuTagName == "SALEINVOICEENTRY")
            {
                lblType.Visible = true;
                cmbAccType.Visible = true;
                btnTrialEInvoice.Visible = true;
                btnAccPrint.Visible = true;
            }
            else if (mFormType == FORMTYPE.SALEINVOICE)
            {
                lblType.Visible = true;
                cmbAccType.Visible = true;
            }
            //Calculation();

            //DTPMemoDate.Focus();
            txtBillingParty.Focus();

            if (mFormType != FORMTYPE.SALEINVOICE && mFormType != FORMTYPE.SALESDELIVERYRETURN && mFormType != FORMTYPE.PURCHASERETURN && mFormType != FORMTYPE.PURCHASEISSUE)
            {
                xtraTabControl1.TabPages.Remove(BookOfPostingTab);
                xtraTabControl1.TabPages.Remove(PostingViewTab);
                xtraTabControl1.TabPages.Remove(ExportPostingTab);
            }

            if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
            {
                if (BOConfiguration.gStrLoginSection != "B")
                {
                    //txtBuyer.Visible = true;
                    //lblBuyer.Visible = true;
                    chkIsConsingee.Visible = true;
                }
                else
                {
                    txtBuyer.Visible = false;
                    lblBuyer.Visible = false;
                    chkIsConsingee.Visible = false;
                }
            }
            else
            {
                txtBuyer.Visible = false;
                lblBuyer.Visible = false;
                chkIsConsingee.Visible = false;
            }

            if (mFormType == FORMTYPE.MEMOISSUE || mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.HOLD || mFormType == FORMTYPE.LABISSUE || mFormType == FORMTYPE.CONSIGNMENTISSUE || mFormType == FORMTYPE.SALEINVOICE)
            {
                BtnJangedNo.Enabled = true;
                txtThrough.Visible = true;
                lblThrough.Visible = true;
            }
            else
            {
                BtnJangedNo.Enabled = false;
                txtThrough.Visible = false;
                lblThrough.Visible = false;
            }
            chkIsConsingee_CheckedChanged(null, null);

            //Add shiv 04-06-2022 For Parcel
            if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE)
            {
                if (mStrStockType == "PARCEL" || mStrStockType == "ALL")
                {
                    MenuTagName = "SALEINVOICEENTRY";
                    cmbBillType_SelectedIndexChanged(null, null);
                    GridCalulationMNL();
                    //mStrStockType = "ALL";
                }

                if (MenuTagName == "SALEINVOICEENTRY")
                {
                    ExportGridColHide();
                    if (cmbAccType.Text == "Export")
                    {
                        DataView view = new DataView(pDtInvoice);
                        DataTable distinctValues = view.ToTable(true, "INVBROKERNAME");

                        if (distinctValues.Rows.Count < 1)
                        {
                            txtBroker.Text = "";
                            txtBroker.Tag = "";
                            txtBaseBrokeragePer.Text = "";
                            txtBrokerAmtFE.Text = "";
                            txtBrokerExcRate.Text = "";
                        }
                    }
                }
            }
            CalculationNew();
            if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
            {
                PanelSaleCalculation.Visible = true;
                BulkPriceCalculation();
            }
            else
            {
                PanelSaleCalculation.Visible = false;
            }
            //Gunjan:15/07/2024
            //if(Val.ToString(txtSellerName.Text) == "")
            {
                txtSellerName.Text = BOConfiguration.gEmployeeProperty.LEDGERNAME;
                txtSellerName.Tag = BOConfiguration.gEmployeeProperty.LEDGER_ID;
            }
            //End As Gunjan

            GrdDetail.BestFitColumns();
            xtraTabControl1.TabPages.Remove(StoneSummaryTab);

            //PnlGrpBrokerGSTDetail.Visible = false;
            pnlButton.Dock = DockStyle.Bottom;
            GrdDetail.EndUpdate();
            GrdDetailParcel.EndUpdate();
            GrdSummury.EndUpdate();
        }

        public void ShowForm(FORMTYPE pFormType, DataTable DTabSaleDetail)// Add By urvisha For Sale
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Show();

                this.Text = "SALE DELIEVERY";

                DTabSaleDetail.Columns.Add("OLDMEMODISCOUNT", typeof(double));
                DTabSaleDetail.Columns.Add("OLDMEMOPRICEPERCARAT", typeof(double));

                DTabSaleDetail.AcceptChanges();

                xtraTabControl1.TabPages.Remove(StoneSummaryTab);
                xtraTabControl1.TabPages.Remove(BookOfPostingTab);
                xtraTabControl1.TabPages.Remove(PostingViewTab);
                xtraTabControl1.TabPages.Remove(ExportPostingTab);
                xtraTabControl1.TabPages.Remove(StoneDetailParcelTab);

                DTabMemoDetail = DTabSaleDetail;

                txtCurrency.Text = DTabMemoDetail.Rows[0]["CURRENCYNAME"].ToString();
                txtCurrency.Tag = DTabMemoDetail.Rows[0]["CURRENCY_ID"].ToString();
                txtBillingParty.Tag = DTabMemoDetail.Rows[0]["BILLINGPARTY_ID"].ToString();
                txtBillingParty.Text = DTabMemoDetail.Rows[0]["BILLINGPARTYNAME"].ToString();


                MainGrdDetail.DataSource = DTabMemoDetail;

                GrdDetail.Bands["BANDEXPORT"].Visible = false;
                MainGrdDetail.Refresh();
                GrdDetail.BestFitColumns();
                CalculationNew();
                FillControlName();
                if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
                {
                    PanelSaleCalculation.Visible = true;
                    BulkPriceCalculation();
                }
                else
                {
                    PanelSaleCalculation.Visible = false;
                }
                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.ToString());
            }

        }
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
                BtnClear_Click(null, null);

                mStrStockType = pStrStockType;
                DataSet DS = ObjMemo.GetMemoListData(0, null, null, "", "", "", 0, "", 0, "", "ALL", pStrMemoID, mStrStockType, false, -1);

                lblStockType.Text = mStrStockType + " STOCK";

                DTabMemo = DS.Tables[0];
                DTabMemoDetail = DS.Tables[1];
                DTabMemoDetailParcelFile = DS.Tables[2];

                DTabMemoDetail.Columns.Add("OLDMEMODISCOUNT", typeof(double));
                DTabMemoDetail.Columns.Add("OLDMEMOPRICEPERCARAT", typeof(double));

                DTabMemoDetailParcelFile.Columns.Add("OLDMEMODISCOUNT", typeof(double));
                DTabMemoDetailParcelFile.Columns.Add("OLDMEMOPRICEPERCARAT", typeof(double));

                MainGrdDetail.DataSource = DTabMemoDetail;
                MainGrdDetail.Refresh();

                MainGrdDetailParcel.DataSource = DTabMemoDetailParcelFile;
                MainGrdDetailParcel.Refresh();

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
                IsChangeBrokPer = true;

                txtAdat.Tag = Val.ToString(DRow["ADAT_ID"]);
                txtAdat.Text = Val.ToString(DRow["ADATNAME"]);
                txtAdatPer.Text = Val.ToString(DRow["ADATPER"]);

                txtTerms.Tag = Val.ToString(DRow["TERMS_ID"]);
                txtTerms.Text = Val.ToString(DRow["TERMSNAME"]);
                txtTermsDays.Text = Val.ToString(DRow["TERMSDAYS"]);

                CmbDeliveryType.Text = Val.ToString(DRow["DELIVERYTYPE"]);
                CmbPaymentMode.Text = Val.ToString(DRow["PAYMENTMODE"]);
                cmbBillType.Text = Val.ToString(DRow["BILLTYPE"]);
                cmbAccType.Text = Val.ToString(DRow["ACCTYPE"]);
                StrMainACCType = Val.ToString(DRow["ACCTYPE"]);

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

                StrMainBillType = Val.ToString(DRow["BILLTYPE"]);

                txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);
                txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                txtExcRate.Text = Val.ToString(DRow["EXCRATE"]);

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
                txtSCity.Text = Val.ToString(DRow["SHIPPINGCITY"]);
                txtSCountry.Tag = Val.ToString(DRow["SHIPPINGCOUNTRY_ID"]);
                txtSCountry.Text = Val.ToString(DRow["SHIPPINGCOUNTRYNAME"]);
                txtSState.Text = Val.ToString(DRow["SHIPPINGSTATE"]);
                txtSZipCode.Text = Val.ToString(DRow["SHIPPINGZIPCODE"]);
                // End By Urvisha : 29012024

                txtRemark.Text = Val.ToString(DRow["REMARK"]);
                lblSource.Text = Val.ToString(DRow["SOURCE"]);

                txtTransport.Text = Val.ToString(DRow["TRANSPORTNAME"]);

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
                txtTotalPcs.Text = Val.ToString(DRow["TOTALPCS"]);
                txtTotalCarat.Text = Val.ToString(DRow["TOTALCARAT"]);
                txtMemoAvgDisc.Text = Val.ToString(DRow["TOTALAVGDISC"]);
                txtMemoAvgRate.Text = Val.ToString(DRow["TOTALAVGRATE"]);
                lblTitle.Text = Val.ToString(DRow["PROCESSNAME"]);
                lblTitle.Tag = Val.ToString(DRow["PROCESS_ID"]);
                txtconsignee.Text = Val.ToString(DRow["CONSIGNEE"]);
                cmbAddresstype.Text = Val.ToString(DRow["ADDRESSTYPE"]);
                if (Val.ToBooleanToInt(DRow["APPROVAL"]) == 1)
                    ChkApprovedOrder.Checked = true;
                else
                    ChkApprovedOrder.Checked = false;

                txtLabServiceCode.Text = Val.ToString(DRow["LABSERVICECODE"]);
                txtLabServiceCode.Tag = Val.ToInt32(DRow["LABSERVICECODE_ID"]);

                txtNarration.Text = Val.ToString(DRow["NARRATIONNAME"]);
                txtNarration.Tag = Val.ToInt32(DRow["NARRATION_ID"]);

                txtBackAddLess.Text = Val.ToString(DRow["BACKADDLESS"]);
                txtTermsAddLessPer.Text = Val.ToString(DRow["TERMSADDLESSPER"]);
                txtBlindAddLessPer.Text = Val.ToString(DRow["BLINDADDLESSPER"]);

                txtInvoiceNo.Text = Val.ToString(DRow["INVOICENO"]);
                txtAutoInvoiceNo.Text = Val.ToString(DRow["TRNNO_OLDDB"]);

                txtThrough.Text = Val.ToString(DRow["THROUGH"]);

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
                else if (Val.ToInt(lblTitle.Tag) == 32) mFormType = FORMTYPE.UNGRADEDTOMIX;
                else if (Val.ToInt(lblTitle.Tag) == 33) mFormType = FORMTYPE.GRADEDTOMIX;

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
                    GrdDetail.Columns["MEMORAPAPORT"].Fixed = FixedStyle.Right;
                    GrdDetail.Columns["SALEDISCOUNT"].Visible = true;
                    GrdDetail.Columns["MEMODISCOUNT"].Visible = true;

                    GrdDetail.Columns["RETURNPCS"].Visible = false;
                    GrdDetail.Columns["RETURNCARAT"].Visible = false;

                }
                //End : #P : 13-01-2020

                if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
                {
                    GrdDetail.Bands["BndCost"].Visible = true;
                }
                else
                {
                    GrdDetail.Bands["BndCost"].Visible = false;
                }

                //Coz JangedDisc And PerCts prthi Sale Amt Consider thay 6e etle MemoDisc Editable nathi..
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;

                CmbMemoType.SelectedItem = Val.ToString(DRow["MEMOTYPE"]);

                if (Val.ToString(lblMode.Text).ToUpper() == "EDIT MODE")
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

                ChkApprovedOrder.Visible = false;
                //if (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.CONSIGNMENTISSUE)
                //    ChkApprovedOrder.Visible = true;

                lblType.Visible = false;
                cmbAccType.Visible = false;
                btnTrialEInvoice.Visible = false;
                btnAccPrint.Visible = false;
                if (mFormType == FORMTYPE.SALEINVOICE && MenuTagName == "SALEINVOICEENTRY")
                {
                    lblType.Visible = true;
                    cmbAccType.Visible = true;
                    btnTrialEInvoice.Visible = true;
                    btnAccPrint.Visible = true;
                }
                else if (mFormType == FORMTYPE.SALEINVOICE)
                {
                    lblType.Visible = true;
                    cmbAccType.Visible = true;
                }
                if (mFormType == FORMTYPE.SALEINVOICE)
                {
                    xtraTabMasterPanel.TabPages[1].PageVisible = false;
                }
                else
                {
                    xtraTabMasterPanel.TabPages[1].PageVisible = true;
                }

                //DTPMemoDate.Focus();
                txtBillingParty.Focus();
                CreateSummaryTable();

                if (mFormType == FORMTYPE.SALEINVOICE)
                {
                    DataRow Drow = DTabMemoSummury.NewRow();
                    DTabMemoSummury.Rows.Add(Drow);

                    GetSummuryDetailForGrid();
                    MainGridSummury.DataSource = DTabMemoSummury;
                    MainGridSummury.Refresh();
                }
                else
                {
                    xtraTabControl1.TabPages.Remove(StoneSummaryTab);
                    MainGridSummury.DataSource = null;
                    MainGridSummury.Refresh();
                }

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
                    xtraTabControl1.TabPages.Remove(BookOfPostingTab);

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
                StoneSummaryTab.Focus();

                if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
                {
                    if (BOConfiguration.gStrLoginSection != "B")
                    {
                        //txtBuyer.Visible = true;
                        //lblBuyer.Visible = true;
                        chkIsConsingee.Visible = true;
                    }
                    else
                    {
                        txtBuyer.Visible = false;
                        lblBuyer.Visible = false;
                        chkIsConsingee.Visible = false;
                    }
                }
                else
                {
                    txtBuyer.Visible = false;
                    lblBuyer.Visible = false;
                    chkIsConsingee.Visible = false;
                }
                chkIsConsingee_CheckedChanged(null, null);

                txtBuyer.Text = Val.ToString(DRow["FINALBUYERNAME"]);
                txtBuyer.Tag = Val.ToString(DRow["FINALBUYER_ID"]);
                chkIsConsingee.Checked = Val.ToBoolean(DRow["ISCONSINGEE"]);

                if (mFormType == FORMTYPE.MEMOISSUE || mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.HOLD || mFormType == FORMTYPE.LABISSUE || mFormType == FORMTYPE.CONSIGNMENTISSUE || mFormType == FORMTYPE.SALEINVOICE)
                {
                    BtnJangedNo.Enabled = true;
                    txtThrough.Visible = true;
                    lblThrough.Visible = true;
                }
                else
                {
                    BtnJangedNo.Enabled = false;
                    txtThrough.Visible = false;
                    lblThrough.Visible = false;
                }

                this.Cursor = Cursors.Default;

                if (BOConfiguration.gStrLoginSection == "B" && mFormType == FORMTYPE.ORDERCONFIRM)
                {
                    pnlButton.Enabled = false;
                }
                if (mFormType == FORMTYPE.PURCHASEISSUE)
                {
                    GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                }
                cmbAccType_SelectedIndexChanged(null, null);
                cmbBillType_SelectedIndexChanged(null, null);
                pnlButton.Dock = DockStyle.Bottom;

                FillControlName();
                CalculationNew();
                if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
                {
                    PanelSaleCalculation.Visible = true;
                    PanelSaleCalculation.Location = new Point(339, 0);
                    BulkPriceCalculation();
                }
                else
                {
                    PanelSaleCalculation.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString());
            }
        }

        public void ShowForm(FORMTYPE pFormType, DataTable pDtInvoice, DataTable pDtMain, string pStockType = "ALL", string StrMainMemo_ID = "")  //Add by khushbu 29-10-21 use in sale dlivery return in b part
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            BtnReturn.Enabled = false;
            BtnOtherActivity.Enabled = false;

            mFormType = pFormType;
            mStrStockType = pStockType;

            DataSet DS = ObjMemo.GetMemoListData(-1, null, null, "", "", "", 0, "", 0, "", "", "", mStrStockType, false, -1);

            StrMainMemo_ID = Val.ToString(StrMainMemo_ID).Trim().Equals(string.Empty) ? Val.ToString(Guid.Empty) : StrMainMemo_ID;
            //DouInvoiceAmt = 300000;

            //DTabMemo = DS.Tables[0];
            DTabMemo = pDtMain;
            DTabMemoDetail = DS.Tables[1];
            DTabMemoDetailParcelFile = DS.Tables[2];

            DTabMemoDetail.Columns["LOCATIONNAME"].DataType = typeof(string);
            DTabMemoDetail.Columns.Add("OLDMEMODISCOUNT", typeof(double));
            DTabMemoDetail.Columns.Add("OLDMEMOPRICEPERCARAT", typeof(double));

            DTabMemoDetailParcelFile.Columns["LOCATIONNAME"].DataType = typeof(string);
            DTabMemoDetailParcelFile.Columns.Add("OLDMEMODISCOUNT", typeof(double));
            DTabMemoDetailParcelFile.Columns.Add("OLDMEMOPRICEPERCARAT", typeof(double));


            MainGrdDetail.DataSource = DTabMemoDetail;
            MainGrdDetail.Refresh();

            MainGrdDetailParcel.DataSource = DTabMemoDetailParcelFile;
            MainGrdDetailParcel.Refresh();

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
            CmbPaymentMode.SelectedIndex = 0;
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
                        if ((pFormType == FORMTYPE.SALEINVOICE || pFormType == FORMTYPE.ORDERCONFIRMRETURN || pFormType == FORMTYPE.RELEASE) && Val.ToString(DRow["CURRENCY_ID"]) == pDtInvoice.Rows[0]["INVCURRENCY_ID"].ToString())
                        //if  (Val.ToString(DRow["CURRENCY_ID"]) == pDtInvoice.Rows[0]["INVCURRENCY_ID"].ToString())
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

                if (BOConfiguration.gStrLoginSection == "B" && (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.PURCHASEISSUE))
                {
                    foreach (DataRow DRow in pDtInvoice.DefaultView.ToTable(true, "MEMO_ID", "JANGEDNOSTR", "MASTREMARK").Rows)
                    {
                        PstrApprove_MemoId = PstrApprove_MemoId + "," + Val.ToString(DRow["MEMO_ID"]); // USED FOR UPDATE ISAPPROVE COLUMN
                        PstrOrder_MemoId = PstrOrder_MemoId + "," + Val.ToString(DRow["MEMO_ID"]);
                        PstrOrderJangedNo = PstrOrderJangedNo + "," + Val.ToString(DRow["JANGEDNOSTR"]);
                        PstrRemark = PstrRemark + "," + Val.ToString(DRow["MASTREMARK"]);

                        if (PstrApprove_MemoId.Substring(0, 1) == ",") // remove first character ','
                        {
                            PstrApprove_MemoId = PstrApprove_MemoId.Substring(1);
                            PstrOrder_MemoId = PstrOrder_MemoId.Substring(1);
                            PstrOrderJangedNo = PstrOrderJangedNo.Substring(1);
                            PstrRemark = PstrRemark.Substring(1);
                        }
                    }
                }

                string StrBillParty = Val.ToString(pDtMain.Rows[0]["BILLINGPARTY_ID"]);
                string StrShipParty = Val.ToString(pDtMain.Rows[0]["SHIPPINGPARTY_ID"]);

                txtInvoiceNo.Text = Val.ToString(pDtMain.Rows[0]["INVOICENO"]);

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
                foreach (DataRow DRow in pDtInvoice.Rows)
                {
                    if (PstrRemark != "")
                    {
                        txtRemark.Text = PstrRemark;
                    }
                    else
                    {
                        txtRemark.Text = Val.ToString(pDtMain.Rows[0]["MASTREMARK"]);
                    }

                    DataRow DRNew = DTabMemoDetail.NewRow();

                    DRNew["MEMODETAIL_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                    DRNew["PREVMEMODETAIL_ID"] = DRow["MEMODETAIL_ID"];  //Add shiv
                    DRNew["STOCK_ID"] = DRow["STOCK_ID"];
                    DRNew["STOCKNO"] = DRow["STOCKNO"];
                    DRNew["PARTYSTOCKNO"] = DRow["PARTYSTOCKNO"];
                    DRNew["STOCKTYPE"] = DRow["STOCKTYPE"];
                    DRNew["FINALBUYER_ID"] = DRow["FINALBUYER_ID"];
                    DRNew["FINALBUYERNAME"] = DRow["FINALBUYERNAME"];

                    if (mStrStockType == "PARCEL" && (Val.ToString(StrMainMemo_ID).Equals(string.Empty) || Val.ToString(StrMainMemo_ID) == Val.ToString(Guid.Empty))) // Only IF Condtn : #P : 17-01-2020
                    {
                        DRNew["PCS"] = DRow["BALANCEPCS"];
                        DRNew["BALANCEPCS"] = DRow["BALANCEPCS"];
                        DRNew["CARAT"] = DRow["BALANCECARAT"];
                        DRNew["BALANCECARAT"] = DRow["BALANCECARAT"];
                    }
                    else
                    {
                        if (Val.Val(DRow["CARAT"]) != 0)
                        {
                            DRNew["CARAT"] = DRow["CARAT"];
                            DRNew["BALANCECARAT"] = DRow["BALANCECARAT"];
                        }
                        if (Val.Val(DRow["PCS"]) != 0)
                        {
                            DRNew["PCS"] = DRow["PCS"];
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

                    //Comment by Gunjan:21/11/2024 #: as per discussion with brijesh bhai order confirm and sale delivery ma je exc rate by default aave chhe ae zero karvano
                    //txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString()), mFormType.ToString()).ToString();
                    //End as Gunjan:21/11/2024

                    if (Val.ToString(DRow["STATUS"]).ToUpper() == "AVAILABLE") //Changed : #P :30-03-2020
                    {
                        DRNew["MEMORAPAPORT"] = DRow["SALERAPAPORT"];
                        DRNew["MEMODISCOUNT"] = DRow["SALEDISCOUNT"];
                        DRNew["MEMOPRICEPERCARAT"] = DRow["SALEPRICEPERCARAT"];
                        DRNew["MEMOAMOUNT"] = DRow["SALEAMOUNT"];
                        DRNew["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["SALEPRICEPERCARAT"]), 3); //#P : 27-01-2022

                        if (BOConfiguration.gStrLoginSection == "B")
                        {
                            DRNew["FMEMOAMOUNT"] = Math.Round((Val.Val(txtExcRate.Text) * Val.Val(DRow["SALEAMOUNT"])) / 1000, 3); //#P : 27-01-2022
                        }
                        else
                        {
                            DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["SALEAMOUNT"]), 3);   //#P : 27-01-2022
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
                        DRNew["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOPRICEPERCARAT"]), 3); //#P : 27-01-2022
                        //DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"]), 2);
                        if (BOConfiguration.gStrLoginSection == "B")
                        {
                            DRNew["FMEMOAMOUNT"] = Math.Round((Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"])) / 1000, 3); //#P : 27-01-2022
                        }
                        else
                        {
                            DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"]), 3); //#P : 27-01-2022
                        }

                        DRNew["JANGEDRAPAPORT"] = DRow["MEMORAPAPORT"];
                        DRNew["JANGEDDISCOUNT"] = DRow["MEMODISCOUNT"];
                        DRNew["JANGEDPRICEPERCARAT"] = DRow["MEMOPRICEPERCARAT"];
                        DRNew["JANGEDAMOUNT"] = DRow["MEMOAMOUNT"];
                    }

                    DRNew["STATUS"] = "PENDING";
                    DRNew["REMARK"] = DRow["DETREMARK"];
                    DRNew["LOCATION_ID"] = DRow["LOCATION_ID"];
                    DRNew["LOCATIONNAME"] = DRow["LOCATIONNAME"];
                    DRNew["SIZE_ID"] = DRow["SIZE_ID"];
                    DRNew["SIZENAME"] = DRow["SIZENAME"];
                    DRNew["LAB_ID"] = DRow["LAB_ID"];
                    DRNew["LABNAME"] = DRow["LABNAME"];
                    DRNew["ISPURCHASE"] = DRow["ISPURCHASE"];
                    DRNew["LABREPORTNO"] = DRow["LABREPORTNO"];
                    DRNew["MAINMEMO_ID"] = StrMainMemo_ID;
                    DRNew["EXCRATE"] = DRow["EXCRATE"];

                    DTabMemoDetail.Rows.Add(DRNew);
                }

                //txtGrossWeight.Text = Val.ToString(DTabMemo.Rows[0]["GROSSWEIGHT"]);
                //cmbInsuranceType.Text = Val.ToString(DTabMemo.Rows[0]["INSURANCETYPE"]);

                txtBuyer.Text = Val.ToString(DTabMemo.Rows[0]["FINALBUYERNAME"]);
                txtBuyer.Tag = Val.ToString(DTabMemo.Rows[0]["FINALBUYER_ID"]);
                chkIsConsingee.Checked = Val.ToBoolean(DTabMemo.Rows[0]["ISCONSINGEE"]);

                // #D: 13-01-2021
                if (mFormType == FORMTYPE.LABRETURN)
                {
                    txtLabServiceCode.Text = Val.ToString(pDtInvoice.Rows[0]["LABSERVICECODE"]);
                    txtLabServiceCode.Tag = Val.ToInt32(pDtInvoice.Rows[0]["LABSERVICECODE_ID"]);
                }
                // #D: 13-01-2021

            }

            //DataTable DTabCurr = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);

            //foreach (DataRow DRow in DTabCurr.Rows)
            //{
            //    if (pDtInvoice != null)
            //    {
            //        if (pFormType == FORMTYPE.SALEINVOICE && Val.ToString(DRow["CURRENCY_ID"]) == pDtInvoice.Rows[0]["INVCURRENCY_ID"].ToString())
            //        {
            //            txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
            //            txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);

            //            GrdDetail.Columns["FMEMOPRICEPERCARAT"].Caption = "  " + Val.ToString(DRow["SYMBOL"]) + "/Cts";
            //            GrdDetail.Columns["FMEMOAMOUNT"].Caption = "  Amt (" + Val.ToString(DRow["SYMBOL"]) + ")";
            //        }
            //        else if (Val.ToString(DRow["CURRENCY_ID"]) == "1")//#P : 09-07-2021 : Coz InvCurr Blank hoy to default Usd consider kare
            //        {
            //            txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
            //            txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);

            //            GrdDetail.Columns["FMEMOPRICEPERCARAT"].Caption = "  " + Val.ToString(DRow["SYMBOL"]) + "/Cts";
            //            GrdDetail.Columns["FMEMOAMOUNT"].Caption = "  Amt (" + Val.ToString(DRow["SYMBOL"]) + ")";
            //        }
            //    }
            //    else if (Val.ToString(DRow["CURRENCY_ID"]) == "1")
            //    {
            //        txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
            //        txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);

            //        GrdDetail.Columns["FMEMOPRICEPERCARAT"].Caption = "  " + Val.ToString(DRow["SYMBOL"]) + "/Cts";
            //        GrdDetail.Columns["FMEMOAMOUNT"].Caption = "  Amt (" + Val.ToString(DRow["SYMBOL"]) + ")";
            //    }
            //}
            //DTabCurr.Dispose();
            //DTabCurr = null;

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
            if ((mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRMRETURN || mFormType == FORMTYPE.RELEASE) && pDtInvoice != null)
            //if (pDtInvoice != null)
            {
                txtBroker.Text = pDtInvoice.Rows[0]["INVBROKERNAME"].ToString();
                txtBroker.Tag = pDtInvoice.Rows[0]["INVBROKER_ID"];
                txtBaseBrokeragePer.Text = pDtInvoice.Rows[0]["INVBROKRAGEPER"].ToString();
                txtProfitBrokeragePer.Text = pDtInvoice.Rows[0]["INVBROKRAGEPROFITPER"].ToString();
                txtExcRate.Text = Val.Val(pDtInvoice.Rows[0]["INVEXCRATE"]) != 0 ? pDtInvoice.Rows[0]["INVEXCRATE"].ToString() : Val.ToString(txtExcRate.Text);

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
                //txtPlaceOfSupply.Text = pDtInvoice.Rows[0]["INVPLACEOFSUPPLY"].ToString();
                txtExcRate_Validated(null, null);

                if (BOConfiguration.gStrLoginSection == "B")
                {
                    txtBaseBrokeragePer.Text = Val.ToString(pDtInvoice.Rows[0]["INVBROKRAGEPER"]);
                    txtBroker.Tag = Val.ToString(pDtInvoice.Rows[0]["INVBROKER_ID"]);
                    txtBroker.Text = pDtInvoice.Rows[0]["INVBROKERNAME"].ToString();
                    IsChangeBrokPer = true;
                }

            }

            if (mFormType == FORMTYPE.LABISSUE)
            {
                txtLabServiceCode.Visible = true;
                BtnApplyAll.Visible = true;
                lblLabServiceCode.Visible = true;
                GrdDetail.Bands["BandLabService"].Visible = false;
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
                xtraTabControl1.TabPages.Remove(StoneSummaryTab);
                MainGridSummury.DataSource = null;
                MainGridSummury.Refresh();
            }



            //#K: 05122020
            ChkApprovedOrder.Visible = false;
            //if (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.CONSIGNMENTISSUE)
            //    ChkApprovedOrder.Visible = true;

            //Calculation();
            CalculationNew();
            //DTPMemoDate.Focus();
            txtBillingParty.Focus();

            if (mFormType != FORMTYPE.SALEINVOICE && mFormType != FORMTYPE.SALESDELIVERYRETURN && mFormType != FORMTYPE.PURCHASERETURN && mFormType != FORMTYPE.PURCHASEISSUE)
            {
                xtraTabControl1.TabPages.Remove(BookOfPostingTab);
                xtraTabControl1.TabPages.Remove(PostingViewTab);
                xtraTabControl1.TabPages.Remove(ExportPostingTab);
            }

            if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
            {
                if (BOConfiguration.gStrLoginSection != "B")
                {
                    //txtBuyer.Visible = true;
                    //lblBuyer.Visible = true;
                    chkIsConsingee.Visible = true;
                }
                else
                {
                    txtBuyer.Visible = false;
                    lblBuyer.Visible = false;
                    chkIsConsingee.Visible = false;
                }
            }
            else
            {
                txtBuyer.Visible = false;
                lblBuyer.Visible = false;
                chkIsConsingee.Visible = false;
            }
            chkIsConsingee_CheckedChanged(null, null);
            //PnlGrpBrokerGSTDetail.Visible = false;
            pnlButton.Dock = DockStyle.Bottom;
            if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
            {
                PanelSaleCalculation.Visible = true;
                BulkPriceCalculation();
            }
            else
            {
                PanelSaleCalculation.Visible = false;
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
            DTabMemoSummury.Columns.Add(new DataColumn("SALEAVGRATEFE", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("SALEAVGDISCFE", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("SALETOTALAMOUNTFE", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("MEMOAVGRATEFE", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("MEMOAVGDISCFE", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("MEMOTOTALAMOUNTFE", typeof(double)));
        }

        public void ShowForm(string Memo_ID, string pStrMenuTag, int SVlue)  //Call When Click MDI SALE Invoice Entry Menu
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Val.FormGeneralSetting(this);
                AttachFormDefaultEvent();

                this.Show();
                MenuTagName = pStrMenuTag;
                if (Memo_ID == "")
                    SaleInvoiceMemoID = "00000000-0000-0000-0000-000000000000";
                else
                    SaleInvoiceMemoID = Memo_ID;
                BtnReturn.Enabled = true;
                BtnOtherActivity.Enabled = true;
                BtnClear_Click(null, null);
                xtraTabMasterPanel.TabPages[1].PageVisible = false;
                rdoBroTDS.SelectedIndex = 1;
                rdBroGST.SelectedIndex = 1;
                lblStockType.Text = "SINGLE" + " STOCK";
                if (MenuTagName == "SALEINVOICEENTRY")
                {
                    xtraTabCtrlManual.BringToFront();
                    xtraTabCtrlManual.Dock = DockStyle.Fill;
                    xtraTabControl1.SendToBack();
                    IsManualEntryAdd = true;
                    ChkApprovedOrder.Visible = false;
                    cmbAccType.Visible = true;
                    lblType.Visible = true;
                    btnTrialEInvoice.Visible = true;
                    btnAccPrint.Visible = true;
                    simpleButton1.Visible = false;


                    cLabel23.Visible = false;
                    CmbDeliveryType.Visible = false;


                    lblBuyer.Visible = false;
                    txtBuyer.Visible = false;

                    chkIsConsingee.Visible = false;
                    //cLabel69.Visible = false;
                    //txtInvoiceNo.Visible = false;
                    lblStkAmt.Visible = false;
                    txtStkAmtFE.Visible = false;
                    txtAdatAmt.Visible = false;
                    txtBrokerAmt.Visible = false;
                    cLabel6.Visible = false;
                    txtTransport.Visible = false;
                    BtnPrintDrop.Visible = false;
                    txtExcRate.ReadOnly = false;


                    cmbBillType.Items.Clear();
                    cmbBillType.Items.Add("None");
                    cmbBillType.Items.Add("RupeesBill");
                    cmbBillType.Items.Add("DollarBill");
                    cmbBillType.Items.Add("Export");
                    cmbBillType.Items.Add("Consignment");
                    cmbBillType.Items.Add("Net Consignment");
                    cmbBillType.Items.Add("Consignment Return");

                    lblSeller.Location = new Point(1100, 9);
                    //842, 9
                    txtSellerName.Location = new Point(1215, 4);
                    //957, 4
                    cLabel57.Location = new Point(635, 9);
                    //889, 57
                    cmbBillType.Location = new Point(705, 4);
                    //957, 53
                    lblType.Location = new Point(866, 9);
                    //1118, 58
                    cmbAccType.Location = new Point(939, 4);
                    //1191, 54

                    panel5.Visible = false;
                    TotalPcs.Visible = false;
                    txtTotalPcs.Visible = true;
                    cLabel25.Visible = false;
                    txtTotalCarat.Visible = true;
                    cLabel53.Visible = false;
                    lblTotalAvgDisc.Visible = false;
                    cLabel33.Visible = false;
                    lblTotalAvgRate.Visible = false;
                    cLabel52.Visible = false;
                    lblTotalAmount.Visible = false;

                    //Add shiv 02-06-2022
                    simpleButton1.Visible = false;
                    BtnOtherActivity.Visible = false;
                    BtnReturn.Visible = false;
                    //Add shiv 27-05-2022
                    //txtBrokerExcRate.Visible = true;
                    //txtAdatExcRate.Visible = true;
                    //PnlGrpBrokerGSTDetail.Visible = true;

                }
                else
                {
                    xtraTabCtrlManual.SendToBack();
                    xtraTabControl1.BringToFront();
                    //ChkApprovedOrder.Visible = true;
                    cmbAccType.Visible = false;
                    lblType.Visible = false;
                    btnTrialEInvoice.Visible = false;
                    btnAccPrint.Visible = false;
                    simpleButton1.Visible = true;
                    cLabel23.Visible = true;
                    CmbDeliveryType.Visible = true;


                    //lblBuyer.Visible = true;
                    //txtBuyer.Visible = true;
                    chkIsConsingee.Visible = true;
                    //cLabel69.Visible = true;
                    //txtInvoiceNo.Visible = true;
                    lblStkAmt.Visible = true;
                    txtStkAmtFE.Visible = true;
                    txtAdatAmt.Visible = true;
                    txtBrokerAmt.Visible = true;
                    cLabel6.Visible = true;
                    txtTransport.Visible = true;


                    BtnPrintDrop.Visible = true;
                    txtExcRate.ReadOnly = true;

                    lblSeller.Location = new Point(842, 9);
                    //842, 9
                    txtSellerName.Location = new Point(957, 4);
                    //957, 4
                    cLabel57.Location = new Point(889, 57);
                    //889, 57
                    cmbBillType.Location = new Point(957, 53);
                    //957, 53
                    lblType.Location = new Point(1118, 58);
                    //1118, 58
                    cmbAccType.Location = new Point(1191, 54);
                    //1191, 54
                    panel5.Visible = false;
                    TotalPcs.Visible = true;
                    txtTotalPcs.Visible = true;
                    cLabel25.Visible = true;
                    txtTotalCarat.Visible = true;
                    cLabel53.Visible = true;
                    lblTotalAvgDisc.Visible = true;
                    cLabel33.Visible = true;
                    lblTotalAvgRate.Visible = true;
                    cLabel52.Visible = true;
                    lblTotalAmount.Visible = true;
                    //Add shiv 02-06-2022
                    simpleButton1.Visible = true;
                    BtnOtherActivity.Visible = true;
                    BtnReturn.Visible = true;
                    //Add shiv 27-05-2022
                    //txtBrokerExcRate.Visible = true;
                    //txtAdatExcRate.Visible = true;
                    //PnlGrpBrokerGSTDetail.Visible = false;
                }

                DataTable DTabTerms = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_TERMS);
                foreach (DataRow DRow1 in DTabTerms.Rows)
                {
                    if (Val.ToString(DRow1["TERMSNAME"]) == "NONE")
                    {
                        txtTerms.Text = Val.ToString(DRow1["TERMSNAME"]);
                        txtTerms.Tag = Val.ToString(DRow1["TERMS_ID"]);
                        txtTermsDays.Text = Val.ToString(DRow1["TERMSDAYS"]);
                        break;
                    }
                }
                DTabTerms.Dispose();
                DTabTerms = null;

                lblSource.Text = "SOFTWARE";
                CmbPaymentMode.SelectedIndex = 0;
                CmbDeliveryType.SelectedIndex = 0;
                cmbAddresstype.SelectedIndex = 1;



                lblMode.Text = "Add Mode";
                lblMemoNo.Text = string.Empty;
                DTPMemoDate.Value = DateTime.Now;

                txtSellerName.Text = BOConfiguration.gEmployeeProperty.LEDGERNAME;
                txtSellerName.Tag = BOConfiguration.gEmployeeProperty.LEDGER_ID;

                #region Data Fillup
                if (Memo_ID != "")
                {
                    FillControlName();
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
                    IsChangeBrokPer = true;

                    txtAdat.Tag = Val.ToString(DRow["ADAT_ID"]);
                    txtAdat.Text = Val.ToString(DRow["ADATNAME"]);
                    txtAdatPer.Text = Val.ToString(DRow["ADATPER"]);

                    txtTerms.Tag = Val.ToString(DRow["TERMS_ID"]);
                    txtTerms.Text = Val.ToString(DRow["TERMSNAME"]);
                    txtTermsDays.Text = Val.ToString(DRow["TERMSDAYS"]);

                    CmbDeliveryType.Text = Val.ToString(DRow["DELIVERYTYPE"]);
                    CmbPaymentMode.Text = Val.ToString(DRow["PAYMENTMODE"]);
                    cmbBillType.Text = Val.ToString(DRow["BILLTYPE"]);
                    cmbAccType.Text = Val.ToString(DRow["ACCTYPE"]);
                    StrMainACCType = Val.ToString(DRow["ACCTYPE"]);

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

                    StrMainBillType = Val.ToString(DRow["BILLTYPE"]);
                    DTPTermsDate.Text = DateTime.Parse(DRow["TERMSDATE"].ToString()).ToShortDateString();

                    txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);
                    txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                    txtExcRate.Text = Val.ToString(DRow["EXCRATE"]);
                    //txtOrgExcRate.Text = Val.ToString(DRow["ORG_EXCRATE"]);

                    //txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString())).ToString();
                    txtExcRate_Validated(null, null);


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
                    //txtPlaceOfSupply.Text = Val.ToString(DRow["PLACEOFSUPPLY"]);

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


                    txtTCSPer.Text = Val.ToString(DRow["TCSPER"]);
                    txtTCSAmount.Text = Val.ToString(DRow["TCSAMOUNT"]);
                    txtTCSAmountFE.Text = Val.ToString(DRow["FTCSAMOUNT"]);

                    txtNetAmount.Text = Val.ToString(DRow["NETAMOUNT"]);
                    txtNetAmountFE.Text = Val.ToString(DRow["FNETAMOUNT"]);

                    txtRemark.Text = Val.ToString(DRow["REMARK"]);
                    txtTransport.Text = Val.ToString(DRow["TRANSPORTNAME"]);
                    //txtPlaceOfSupply.Text = Val.ToString(DRow["PLACEOFSUPPLY"]);

                    txtTotalPcs.Text = Val.ToString(DRow["TOTALPCS"]);
                    txtTotalCarat.Text = Val.ToString(DRow["TOTALCARAT"]);
                    txtMemoAvgDisc.Text = Val.ToString(DRow["TOTALAVGDISC"]);
                    txtMemoAvgRate.Text = Val.ToString(DRow["TOTALAVGRATE"]);

                    lblTitle.Text = Val.ToString(DRow["PROCESSNAME"]);
                    lblTitle.Tag = Val.ToString(DRow["PROCESS_ID"]);

                    //txtGrossWeight.Text = Val.ToString(DRow["GROSSWEIGHT"]);
                    //cmbInsuranceType.Text = Val.ToString(DRow["INSURANCETYPE"]);

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
                    txtAutoInvoiceNo.Text = Val.ToString(DRow["TRNNO_OLDDB"]);

                    txtBrokerExcRate.Text = Val.ToString(DRow["BROKERAGEEXCRATE"]);
                    txtAdatExcRate.Text = Val.ToString(DRow["ADATEXCRATE"]);

                    txtGRFreight.Text = Val.ToString(DRow["GRFREIGHT"]);
                    txtBroTdsPer.Text = Val.ToString(DRow["BROTDSPER"]);
                    txtBroTdsRs.Text = Val.ToString(DRow["BROTDSRS"]);
                    txtBroTAmt.Text = Val.ToString(DRow["BROTOTALAMT"]);


                    txtBroCgstPer.Text = Val.ToString(DRow["BROCGSTPER"]);
                    txtBroCgstAmt.Text = Val.ToString(DRow["BROCGSTRS"]);
                    txtBroSgstPer.Text = Val.ToString(DRow["BROSGSTPER"]);
                    txtBroSgstAmt.Text = Val.ToString(DRow["BROSGSTRS"]);
                    txtBroIgstPer.Text = Val.ToString(DRow["BROIGSTPER"]);
                    txtBroIgstAmt.Text = Val.ToString(DRow["BROIGSTRS"]);

                    txtConsignmentRefNo.Text = Val.ToString(DRow["ConsignmentRefNo"]);

                    //ADD shiv 10-10-2022
                    if (SVlue == 1)
                    {
                        mFormType = FORMTYPE.CONSIGNMENTRETURN;
                        if (SaleInvoiceMemoID != "")
                        {
                            lblMode.Text = "Add Mode";

                            GrdSummuryMNL.Columns["RETURNCARAT"].Visible = true;
                            GrdSummuryMNL.Columns["RETURNPCS"].Visible = true;

                            GrdSummuryMNL.Columns["RETURNCARAT"].OptionsColumn.AllowEdit = true;
                            GrdSummuryMNL.Columns["RETURNPCS"].OptionsColumn.AllowEdit = true;

                            GrdSummuryMNL.Columns["CARAT"].Visible = false;
                            GrdSummuryMNL.Columns["PCS"].Visible = false;

                            DTabMemo.Rows[0]["REMARK"] = "";

                            var DRows = DTabMemoDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["CARAT"]) == "").ToArray();
                            foreach (DataRow dr in DRows)
                                DTabMemoDetail.Rows.Remove(dr);

                            for (int i = 0; i < DTabMemoDetail.Rows.Count; i++)
                            {
                                DTabMemoDetail.Rows[i]["MEMOPRICEPERCARAT"] = Math.Round(Convert.ToDecimal(DTabMemoDetail.Rows[i]["MEMOPRICEPERCARAT"]), 2);
                                DTabMemoDetail.Rows[i]["MEMOAMOUNT"] = Math.Round(Convert.ToDecimal(DTabMemoDetail.Rows[i]["MEMOAMOUNT"]), 2);
                                DTabMemoDetail.Rows[i]["RETURNCARAT"] = Math.Round(Convert.ToDecimal(DTabMemoDetail.Rows[i]["CARAT"]), 3);
                                DTabMemoDetail.Rows[i]["RETURNPCS"] = Math.Round(Convert.ToDecimal(DTabMemoDetail.Rows[i]["Pcs"]), 0);
                                DTabMemoDetail.Rows[i]["ITEMNAME"] = "Cut & Polished Diamond";
                                DTabMemoDetail.Rows[i]["HSNCODE"] = "71023910";
                                DTabMemoDetail.Rows[i]["REMARK"] = "";


                                if (Val.ToString(DTabMemoDetail.Rows[i]["Pcs"]) == "0")
                                {
                                    DTabMemoDetail.Rows[i]["Pcs"] = "1";
                                }
                            }

                            IS_CONSIRET = true;
                        }
                    }

                    //ADD SHIV 20-09-22
                    if (cmbAccType.SelectedIndex == 1)
                    {
                        if (Val.ToString(DRow["IS_TDSLIMIT"]) == "True")
                        {
                            lblCrdlimit.Visible = false;
                            if (txtBillingParty.Tag != "")
                            {
                                DataTable dt = new BOMST_Ledger().GetLedgerDataForTDSCredit(Val.ToString(txtBillingParty.Tag), Val.SqlDate(DTPMemoDate.Text), Val.ToGuid(lblMemoNo.Tag), Val.Val(txtGrossAmountFE.Text));
                                //if (dt.Rows.Count != 0)
                                //{
                                //    lblCrdlimit.Text = Val.ToString(dt.Rows[0]["AMOUNTFE"]);
                                //}
                                //else
                                //{
                                //    lblCrdlimit.Text = "0.00";
                                //}
                                if (dt.Rows.Count != 0)
                                {
                                    txtTCSCalcAmt.Text = Val.ToString(dt.Rows[0]["TDSAmount"]);
                                }
                                else
                                {
                                    txtTCSCalcAmt.Text = "0.00";
                                }
                            }

                            //DataTable DT = new BOMST_Ledger().GetLedgerDataForTDSCreditLimit(Val.ToString(txtBillingParty.Tag));
                            //double DouMemoAmount1 = Val.Val(txtGrossAmountFE.Text);
                            //double DouLedgerTotAmt1 = Val.Val(lblCrdlimit.Text);
                            //if (DouLedgerTotAmt1 > dblTCSAmount)
                            //{
                            //    if (DT.Rows.Count > 0)
                            //    {
                            //        IS_MAXLIMIT = Val.ToBoolean(DT.Rows[0]["IS_Maxlimit"]);
                            //    }
                            //    if (IS_MAXLIMIT == true)
                            //    {
                            //        if (DouLedgerTotAmt1 > dblTCSAmount)
                            //        {
                            //            double dblDiff = dblTCSAmount - (DouLedgerTotAmt1);
                            //            if (dblDiff > 0)
                            //            {
                            //                txtTCSCalcAmt.Text = "0.00";
                            //            }
                            //            else
                            //            {
                            //                Global.Message("TDS Limit Exists Fifty lakh Above Please Add Tds.......");
                            //                txtTCSCalcAmt.Text = Val.ToString(Math.Abs(dblDiff));
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        Global.Message("TDS Limit Exists Fifty lakh Above Please Add Tds.......");
                            //        txtTCSCalcAmt.Text = Val.ToString(DouMemoAmount1);
                            //    }
                            //}
                            //else
                            //{
                            //    txtTCSCalcAmt.Text = Val.ToString(DouMemoAmount1);
                            //}
                        }
                        else
                        {
                            lblCrdlimit.Visible = false;
                            if (txtBillingParty.Tag != "")
                            {
                                DataTable dt = new BOMST_Ledger().GetLedgerDataForTDSCredit(Val.ToString(txtBillingParty.Tag), Val.SqlDate(DTPMemoDate.Text), Val.ToGuid(lblMemoNo.Tag), Val.Val(txtGrossAmountFE.Text));
                                //if (dt.Rows.Count != 0)
                                //{
                                //    lblCrdlimit.Text = Val.ToString(dt.Rows[0]["AMOUNTFE"]);
                                //}
                                //else
                                //{
                                //    lblCrdlimit.Text = "0.00";
                                //}
                                if (dt.Rows.Count != 0)
                                {
                                    txtTCSCalcAmt.Text = Val.ToString(dt.Rows[0]["TDSAmount"]);
                                }
                                else
                                {
                                    txtTCSCalcAmt.Text = "0.00";
                                }
                            }
                            //#region TDS Amount Calculation
                            //DataTable DTMAX = new BOMST_Ledger().GetLedgerDataForTDSCreditLimit(Val.ToString(txtBillingParty.Tag));
                            //if (DTMAX.Rows.Count != 0)
                            //{
                            //    txtTCSCalcAmt.Text = Val.ToString(DTMAX.Rows[0]["TDSAmount"]);
                            //}
                            //else
                            //{
                            //    txtTCSCalcAmt.Text = "0.00";
                            //}

                            //double DouMemoAmount = Val.Val(txtGrossAmountFE.Text);
                            //double DouLedgerTotAmt = Val.Val(lblCrdlimit.Text);
                            //if (DouLedgerTotAmt > dblTCSAmount)
                            //{
                            //    if (DTMAX.Rows.Count > 0)
                            //    {
                            //        IS_MAXLIMIT = Val.ToBoolean(DTMAX.Rows[0]["IS_Maxlimit"]);
                            //    }
                            //    if (IS_MAXLIMIT == true)
                            //    {
                            //        if (DouLedgerTotAmt > dblTCSAmount)
                            //        {
                            //            double dblDiff = dblTCSAmount - (DouLedgerTotAmt);
                            //            if (dblDiff > 0)
                            //            {
                            //                txtTCSCalcAmt.Text = "0.00";
                            //            }
                            //            else
                            //            {
                            //                Global.Message("TDS Limit Exists Fifty lakh Above Please Add Tds.......");
                            //                txtTCSCalcAmt.Text = Val.ToString(Math.Abs(dblDiff));
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        Global.Message("TDS Limit Exists Fifty lakh Above Please Add Tds.......");
                            //        txtTCSCalcAmt.Text = Val.ToString(DouMemoAmount);
                            //    }
                            //}

                        }
                    }

                    if (Val.ToBoolean(DRow["IS_BROGST"]) == true)
                    {
                        rdBroGST.SelectedIndex = 0;
                    }
                    else
                    {
                        rdBroGST.SelectedIndex = 1;
                    }

                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        txtApproveMemoNo.Visible = true;
                        txtOrderMemoNo.Visible = true;
                        txtApproveMemoNo.Text = Val.ToString(DRow["APPROVEMEMONO"]);
                        txtOrderMemoNo.Text = Val.ToString(DRow["ORDERMEMONO"]);
                        txtOrderMemoNo.Tag = Val.ToString(DRow["ORDERMEMO_ID"]);
                        PstrApprove_MemoId = Val.ToString(DRow["MEMO_ID"]);
                        PstrTrnNo_OldDB = Val.ToString(DTabMemoDetail.Rows[0]["TRNNO_OLDDB"]);

                        if (!PstrTrnNo_OldDB.Trim().Equals(string.Empty))
                        {
                            var parts = PstrTrnNo_OldDB.Split('_');
                            PStrD_Code = parts[0];
                            PIntTrnNo = Val.ToInt32(parts[1]);
                        }
                    }
                    txtThrough.Text = Val.ToString(DRow["THROUGH"]);

                    if (Val.ToInt(lblTitle.Tag) == 9) mFormType = FORMTYPE.SALEINVOICE;

                    lblStockType.Text = mStrStockType + " STOCK";

                    CalculationNewInvoiceEntry();

                    CmbMemoType.SelectedItem = Val.ToString(DRow["MEMOTYPE"]);

                    if (Val.ToString(lblMode.Text).ToUpper() == "EDIT MODE") //#P : 06-10-2019
                    {
                        //if (ChkApprovedOrder.Checked == true)
                        //{
                        //    BtnSave.Enabled = false;
                        //    BtnReturn.Enabled = false;
                        //    simpleButton1.Enabled = false;
                        //    BtnDelete.Enabled = false;
                        //    BtnOtherActivity.Enabled = false;
                        //}
                        //else
                        //{
                        //    BtnSave.Enabled = true;
                        //    BtnReturn.Enabled = true;
                        //    simpleButton1.Enabled = true;
                        //    BtnDelete.Enabled = true;
                        //    BtnOtherActivity.Enabled = true;
                        //}
                    }


                    //#K: 05122020
                    ChkApprovedOrder.Visible = false;
                    //if (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.CONSIGNMENTISSUE)
                    //    ChkApprovedOrder.Visible = true;

                    //SHIV 26-12-2022
                    lblType.Visible = false;
                    cmbAccType.Visible = false;
                    btnTrialEInvoice.Visible = false;
                    btnAccPrint.Visible = false;
                    if (mFormType == FORMTYPE.SALEINVOICE && MenuTagName == "SALEINVOICEENTRY")
                    {
                        lblType.Visible = true;
                        cmbAccType.Visible = true;
                        btnTrialEInvoice.Visible = true;
                        btnAccPrint.Visible = true;
                    }
                    else if (mFormType == FORMTYPE.SALEINVOICE)
                    {
                        lblType.Visible = true;
                        cmbAccType.Visible = true;
                    }
                    //DTPMemoDate.Focus();
                    txtBillingParty.Focus();

                    CreateSummaryTable();
                    if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN)
                    {
                        DataRow Drow = DTabMemoSummury.NewRow();
                        DTabMemoSummury.Rows.Add(Drow);
                        GetSummuryDetailForGridSaleInvoiceEntry();
                        //MainGridSummuryMNL.DataSource = DTabMemoSummury;
                        //MainGridSummuryMNL.Refresh();
                    }
                    else
                    {
                        xtraTabControl1.TabPages.Remove(StoneSummaryTab);
                        MainGridSummury.DataSource = null;
                        MainGridSummury.Refresh();
                    }

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

                    if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
                    {
                        if (BOConfiguration.gStrLoginSection != "B")
                        {
                            //txtBuyer.Visible = true;
                            //lblBuyer.Visible = true;
                            chkIsConsingee.Visible = true;
                        }
                        else
                        {
                            txtBuyer.Visible = false;
                            lblBuyer.Visible = false;
                            chkIsConsingee.Visible = false;
                        }
                    }
                    else
                    {
                        txtBuyer.Visible = false;
                        lblBuyer.Visible = false;
                        chkIsConsingee.Visible = false;
                    }
                    chkIsConsingee_CheckedChanged(null, null);


                    txtBuyer.Text = Val.ToString(DRow["FINALBUYERNAME"]);
                    txtBuyer.Tag = Val.ToString(DRow["FINALBUYER_ID"]);
                    chkIsConsingee.Checked = Val.ToBoolean(DRow["ISCONSINGEE"]);

                    if (mFormType == FORMTYPE.MEMOISSUE || mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.HOLD || mFormType == FORMTYPE.LABISSUE || mFormType == FORMTYPE.CONSIGNMENTISSUE || mFormType == FORMTYPE.SALEINVOICE)
                    {
                        BtnJangedNo.Enabled = true;
                        txtThrough.Visible = true;
                        lblThrough.Visible = true;
                    }
                    else
                    {
                        BtnJangedNo.Enabled = false;
                        txtThrough.Visible = false;
                        lblThrough.Visible = false;
                    }

                    this.Cursor = Cursors.Default;

                    if (BOConfiguration.gStrLoginSection == "B" && mFormType == FORMTYPE.ORDERCONFIRM)
                    {
                        pnlButton.Enabled = false;
                    }
                    if (mFormType == FORMTYPE.PURCHASEISSUE)
                    {
                        GrdSummuryMNL.Bands["BANDEXTRAPARAM"].Visible = false;
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                    {
                        if (cmbBillType.Text.ToUpper() == "RUPEESBILL")
                        {
                            txtExcRate.Text = "1";
                            GrdSummuryMNL.Columns["FMEMOPRICEPERCARAT"].Visible = true;
                            GrdSummuryMNL.Columns["FMEMOAMOUNT"].Visible = true;
                            GrdSummuryMNL.Columns["FMEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                            GrdSummuryMNL.Columns["FMEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                            GrdSummuryMNL.Columns["MEMOPRICEPERCARAT"].Visible = false;
                            GrdSummuryMNL.Columns["MEMOAMOUNT"].Visible = false;
                            GrdSummuryMNL.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                            GrdSummuryMNL.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                        }
                        else if (cmbBillType.Text.ToUpper() == "DOLLARBILL")
                        {

                            txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString()), mFormType.ToString()).ToString();
                            GrdSummuryMNL.Columns["FMEMOPRICEPERCARAT"].Visible = false;
                            GrdSummuryMNL.Columns["FMEMOAMOUNT"].Visible = false;
                            GrdSummuryMNL.Columns["FMEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                            GrdSummuryMNL.Columns["FMEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                            GrdSummuryMNL.Columns["MEMOPRICEPERCARAT"].Visible = true;
                            GrdSummuryMNL.Columns["MEMOAMOUNT"].Visible = true;
                            GrdSummuryMNL.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                            GrdSummuryMNL.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                        }
                    }

                    if (Val.ToBoolean(DRow["IS_BROTDS"]) == true)
                    {
                        rdoBroTDS.SelectedIndex = 0;
                    }
                    else
                    {
                        rdoBroTDS.SelectedIndex = 1;
                    }
                    IsEInvoiceDone = Val.ToBoolean(DRow["IsEInvoiceDone"]);

                    if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME == "administrator")
                    {
                        btnBrokerUpdate.Visible = true;
                        chkIsTdsAmt.Visible = true;
                    }
                    else
                    {
                        btnBrokerUpdate.Visible = false;
                        chkIsTdsAmt.Visible = false;
                    }
                }

                #endregion

                //Comment by Gunjan:21/11/2024 #: as per discussion with brijesh bhai order confirm and sale delivery ma je exc rate by default aave chhe ae zero karvano
                //if (lblMode.Text == "Add Mode")
                //{
                //    txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString()), mFormType.ToString()).ToString();

                //    txtExcRate_Validated(null, null);
                //}
                //End as Gunjan

                FillControlName();
                //CalculationNew();


                lblCrdlimit.Visible = false;
                if (txtBillingParty.Tag != "")
                {
                    DataTable dt = new BOMST_Ledger().GetLedgerDataForTDSCredit(Val.ToString(txtBillingParty.Tag), Val.SqlDate(DTPMemoDate.Text), Val.ToGuid(lblMemoNo.Tag), Val.Val(txtGrossAmountFE.Text));
                    //if (dt.Rows.Count != 0)
                    //{
                    //    lblCrdlimit.Text = Val.ToString(dt.Rows[0]["AMOUNTFE"]);
                    //}
                    //else
                    //{
                    //    lblCrdlimit.Text = "0.00";
                    //}
                    if (dt.Rows.Count != 0)
                    {
                        txtTCSCalcAmt.Text = Val.ToString(dt.Rows[0]["TDSAmount"]);
                    }
                    else
                    {
                        txtTCSCalcAmt.Text = "0.00";
                    }
                }

                CalculationNewInvoiceEntry();

                //DTPMemoDate.Focus();
                txtBillingParty.Focus();
                GrdSummuryMNL.Columns["PCS"].OptionsColumn.AllowEdit = true;
                GrdSummuryMNL.Columns["CARAT"].OptionsColumn.AllowEdit = true;


                if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
                {
                    if (MenuTagName == "SALEINVOICEENTRY" || mFormType == FORMTYPE.SALEINVOICE)
                    {
                        if (cmbAccType.SelectedIndex == 1 || cmbAccType.SelectedIndex == 2 || cmbAccType.SelectedIndex == 3)
                        {
                            txtBuyer.Visible = false;
                            chkIsConsingee.Visible = false;
                            lblBuyer.Visible = false;
                            btnTrialEInvoice.Visible = true; //ADD SHIV 26-11-22
                        }
                        else if (cmbAccType.SelectedIndex == 4)
                        {
                            //txtBuyer.Visible = true;
                            chkIsConsingee.Visible = true;
                            //lblBuyer.Visible = true;
                        }
                        else
                        {
                            //txtBuyer.Visible = true;
                            chkIsConsingee.Visible = true;
                            //lblBuyer.Visible = true;
                            //Add shiv 26-11-22
                            btnTrialEInvoice.Visible = false;

                        }
                    }
                }


                cmbAccType_SelectedIndexChanged(null, null);
                GridCalulationMNL();



                //if (DouLedgerTotAmt > dblTCSAmount)
                //{
                //    txtTCSCalcAmt.Text = Val.ToString(DouMemoAmount);
                //}
                //else
                //{
                //    double dblDiff = dblTCSAmount - (DouMemoAmount + DouLedgerTotAmt);
                //    if (dblDiff > 0)
                //    {
                //        txtTCSCalcAmt.Text = "0.00";
                //    }
                //    else
                //    {
                //        txtTCSCalcAmt.Text = Val.ToString(Math.Abs(dblDiff));
                //    }
                //}
                //#endregion

                //if (IsEInvoiceDone == true)
                //    BtnSave.Enabled = false;
                if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME != "administrator" && IsEInvoiceDone == true)
                {
                    Global.Message("This Entry Lock Connect To Administrator......!");
                }
                if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
                {
                    PanelSaleCalculation.Visible = true;
                    BulkPriceCalculation();
                }
                else
                {
                    PanelSaleCalculation.Visible = false;
                }
                this.Cursor = Cursors.Default;
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
            DTabAccTrnType = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_ACCTRNTYPE);

            DtabProcess = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PROCESSALL);

            CmbMemoType.Items.Clear();

            DtabRapaport = new BOTRN_PriceRevised().GetOriginalRapData("GETCURRENTRAPAPORT", "", "", 0, 0);
            DtabPara = new BOMST_Parameter().GetParameterData();
            GrdDetail.Columns["PARTYSTOCKNO"].Visible = false;

            if (mFormType == FORMTYPE.HOLD)
            {
                GetSelectedProcessIssue("HOLD");

                GrdDetail.Bands["BandLabService"].Visible = false;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["ISPURCHASE"].Visible = false;
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan
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
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
            }
            else if (mFormType == FORMTYPE.RELEASE)
            {
                GetSelectedProcessIssue("RELEASE");

                GrdDetail.Bands["BandLabService"].Visible = false;
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan
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
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
            }

            else if (mFormType == FORMTYPE.OFFLINE)
            {
                GetSelectedProcessIssue("OFFLINE");
                GrdDetail.Bands["BandLabService"].Visible = false;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan
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
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
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
                txtAutoInvoiceNo.Visible = false;
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
            }

            else if (mFormType == FORMTYPE.ORDERCONFIRM)
            {
                GetSelectedProcessIssue("ORDER CONFIRM");
                GrdDetail.Bands["BandLabService"].Visible = false;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;

                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = true;
                //End As Gunjan

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
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = true;//Gunjan:10/09/2024
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
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
            }
            else if (mFormType == FORMTYPE.SALEINVOICE)
            {
                GetSelectedProcessIssue("SALES DELIVERY");
                GrdDetail.Bands["BandLabService"].Visible = false;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = true;
                //End As Gunjan

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
                txtAutoInvoiceNo.Visible = true;
                PanelSaleCalculation.Visible = true;//Gunjan:10/09/2024
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
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
            }

            else if (mFormType == FORMTYPE.MEMOISSUE)
            {
                GetSelectedProcessIssue("MEMO ISSUE");
                GrdDetail.Bands["BandLabService"].Visible = false;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;

                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan

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
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
            }

            else if (mFormType == FORMTYPE.MEMORETURN)
            {
                GrdDetail.Bands["BandLabService"].Visible = false;

                GetSelectedProcessIssue("MEMO RETURN");
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan
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
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
            }

            else if (mFormType == FORMTYPE.LABISSUE)
            {
                GetSelectedProcessIssue("LAB ISSUE");
                GrdDetail.Bands["BandLabService"].Visible = true;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan

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
                //Gunjan:13/08/2024
                GrdDetail.Columns["SALEPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["SALEDISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["SALEAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
            }

            else if (mFormType == FORMTYPE.LABRETURN)
            {
                GetSelectedProcessIssue("LAB RETURN");
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Bands["BandLabService"].Visible = true;
                GrdDetail.Columns["LABSERVICECODE"].OptionsColumn.AllowEdit = false;
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan

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
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
            }
            else if (mFormType == FORMTYPE.CONSIGNMENTISSUE)
            {
                GetSelectedProcessIssue("CONSIGNMENT ISSUE");
                GrdDetail.Bands["BandLabService"].Visible = false;
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan

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
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
            }

            else if (mFormType == FORMTYPE.CONSIGNMENTRETURN)
            {
                GrdDetail.Bands["BandLabService"].Visible = false;
                GetSelectedProcessIssue("CONSIGNMENT RETURN");
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan
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
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
            }

            else if (mFormType == FORMTYPE.PURCHASEISSUE)
            {
                DtabPara = new BOMST_Parameter().GetParameterData();

                //BtnNewRow.Visible = true;
                BtnNewRow.Visible = true;

                GetSelectedProcessIssue("PURCHASE");
                GrdDetail.Bands["BandLabService"].Visible = false;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = mStrStockType == "SINGLE" ? true : false;

                if (BOConfiguration.gStrLoginSection == "B")
                {
                    GrdDetail.Bands["BANDJANGEDDETAIL"].Visible = false;
                    GrdDetail.Bands["BANDMEMOPRICE"].Visible = false;
                }

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
                GrdDetail.Columns["SALEPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["SALEAMOUNT"].OptionsColumn.AllowEdit = false;

                GrdDetail.Columns["MEMORAPAPORT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;

                GrdDetail.Columns["JANGEDDISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["JANGEDPRICEPERCARAT"].OptionsColumn.AllowEdit = false;

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
                GrdDetail.Columns["FSALEPRICEPERCARAT"].Caption = "Cost ₹/Cts(₹)";
                GrdDetail.Columns["FSALEAMOUNT"].Caption = "Cost Amt(₹)";

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
                GrdDetail.Columns["STOCKNO"].OptionsColumn.AllowEdit = true;

                //GrdDetail.Columns["PARTYSTOCKNO"].OptionsColumn.AllowEdit = true;


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
                txtAutoInvoiceNo.Visible = true;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
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
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan


                lblSeller.Visible = false;
                txtSellerName.Visible = false;
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
            }

            else if (mFormType == FORMTYPE.UNGRADEDTOMIX)
            {
                GrdDetail.Bands["BandLabService"].Visible = false;
                GetSelectedProcessIssue("UNGRADED TO MIX");
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan
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
                txtAutoInvoiceNo.Visible = false;
                PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024
            }

            else if (mFormType == FORMTYPE.GRADEDTOMIX)
            {
                GrdDetail.Bands["BandLabService"].Visible = false;
                GetSelectedProcessIssue("GRADED TO MIX");
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                //Gunjan:13/08/2024
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                //End As Gunjan
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
                txtAutoInvoiceNo.Visible = false;
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
            PanelSaleCalculation.Visible = false;//Gunjan:10/09/2024

            CmbMemoType.SelectedIndex = 0;
        }

        public async void EInvoiceUploadGenerateIRN()
        {
            try
            {
                if (txtInvoiceNo.Text != "")
                {
                    TRN_EInvoiceProperty Property = ObjEInvoice.GetEInvoiceCredential(BusLib.Configuration.BOConfiguration.COMPANY_ID);
                    TxnRespWithObj<eInvoiceSession> txnRespWithObj = await eInvoiceAPI.GetAuthTokenAsync(eInvSession);
                    if (txnRespWithObj.IsSuccess)
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
                            string JsonData = Val.ToString(dsJson.Tables[0].Rows[0]["Json"]);
                            TxnRespWithObj<RespPlGenIRN> txnRespWithObjINR = await eInvoiceAPI.GenIRNAsync(eInvSession, JsonData, 250);
                            RespPlGenIRN respPlGenIRN = txnRespWithObjINR.RespObj;
                            string ErrorCodes = "";
                            string ErrorDesc = "";
                            if (txnRespWithObjINR.IsSuccess)
                            {
                                rtbResponce = JsonConvert.SerializeObject(respPlGenIRN);
                                respPlGenIRN.QrCodeImage = null;
                                TxnRespWithObj<VerifyRespPl> txnRespWithObj1 = await eInvoiceAPI.VerifySignedInvoice(eInvSession, respPlGenIRN);
                                AckNo = respPlGenIRN.AckNo;
                                IrnNo = respPlGenIRN.Irn;
                                SignedInvoice = respPlGenIRN.SignedInvoice;
                                SignedQRCode = respPlGenIRN.SignedQRCode;
                                IrnDate = respPlGenIRN.AckDt;
                                ObjEInvoice.UpdateEInvoiceUploadDate(IrnDate, Val.ToString(lblMemoNo.Tag));
                                int RetValue = ObjEInvoice.InsertEInvoiceDetail(Val.ToString(lblMemoNo.Tag), AckNo, IrnNo, Val.SqlDate(IrnDate), SignedInvoice, SignedQRCode, null, Property);
                                if (RetValue == -1)
                                {
                                    StrMessage = "Record Not Inserted";
                                    IntIsError = 1;
                                    return;
                                }
                                else
                                {
                                    rtbResponce = txnRespWithObj.TxnOutcome;
                                    MessageBox.Show(rtbResponce);
                                    if (txnRespWithObj.IsSuccess)
                                    {
                                        StrMessage = "E-Invoice Upload Successfully";
                                        Autosave = false;
                                        MessageBox.Show(StrMessage);
                                        btnPrintDetail_ItemClick(null, null);
                                    }
                                    IntIsError = 0;
                                    string dir = System.Windows.Forms.Application.StartupPath + "\\" + "JSON";
                                    if (!Directory.Exists(dir))
                                    {
                                        Directory.CreateDirectory(dir);
                                    }
                                    string Path = System.Windows.Forms.Application.StartupPath + "\\" + "JSON" + "\\" + "E_INV_" + txtInvoiceNo.Text + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                                    if (File.Exists(Path))
                                    {
                                        File.Delete(Path);
                                        using (var tw = new StreamWriter(Path, true))
                                        {
                                            tw.WriteLine(JsonData.ToString());
                                            tw.Close();
                                        }
                                    }
                                    else if (!File.Exists(Path))
                                    {
                                        using (var tw = new StreamWriter(Path, true))
                                        {
                                            tw.WriteLine(JsonData.ToString());
                                            tw.Close();
                                        }
                                    }
                                    // Write that JSON to txt file,  
                                    //System.IO.File.WriteAllText(StrFilePath + "output.json", JsonData);
                                }
                            }
                            else
                            {
                                if (txnRespWithObj.ErrorDetails != null)
                                {
                                    foreach (RespErrDetailsPl errPl in txnRespWithObj.ErrorDetails)
                                    {
                                        //Process errPl item here
                                        ErrorCodes += errPl.ErrorCode + ",";
                                        ErrorDesc += errPl.ErrorCode + ": " + errPl.ErrorMessage + Environment.NewLine;
                                        rtbResponce = ErrorDesc;
                                        Global.Message(rtbResponce);
                                    }
                                }

                                //Process InfoDetails here
                                RespInfoDtlsPl respInfoDtlsPl = new RespInfoDtlsPl();
                                //Serialize Desc object from InfoDtls as per InfCd
                                if (txnRespWithObj.InfoDetails != null)
                                {
                                    foreach (RespInfoDtlsPl infoPl in txnRespWithObj.InfoDetails)
                                    {
                                        var strDupIrnPl = JsonConvert.SerializeObject(infoPl.Desc);   //Convert object type to json string
                                        switch (infoPl.InfCd)
                                        {
                                            case "DUPIRN":
                                                DupIrnPl dupIrnPl = JsonConvert.DeserializeObject<DupIrnPl>(strDupIrnPl);
                                                break;
                                            case "EWBERR":
                                                List<EwbErrPl> ewbErrPl = JsonConvert.DeserializeObject<List<EwbErrPl>>(strDupIrnPl);
                                                break;
                                            case "ADDNLNFO":
                                                string strDesc = (string)infoPl.Desc;
                                                break;
                                        }
                                    }
                                }

                                string errormsg = txnRespWithObjINR.TxnOutcome;
                                Global.Message(errormsg);
                                return;
                            }
                        }
                        else
                        {
                            string StrMessage = Val.ToString(dsJson.Tables[0].Rows[0]["Message"]);
                            Global.Message(StrMessage);
                            return;
                        }
                        //rtbResponce.Text = JsonConvert.SerializeObject(rtbResponce.Text);
                    }
                }
                else
                {
                    StrMessage = "Please Enter Invoice No because E-Invoice Not Upload";

                    MessageBox.Show(StrMessage);
                    Autosave = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

            //BOTRN_EInvoiceAPI EAPI = new BOTRN_EInvoiceAPI();
            //TRN_EInvoiceProperty Property = ObjEInvoice.GetEInvoiceCredential(BusLib.Configuration.BOConfiguration.COMPANY_ID);
            //GetTokenData Obj = EAPI.GetToken(Property.CLIENTID, Property.CLIENTSECRET, Property.GSTIN);
            //if (Obj.AuthToken.Length > 0)
            //{
            //    AuthToken = Obj.AuthToken;
            //    Skey = Obj.Key;
            //}
            //if (AuthToken == "")
            //{
            //    StrMessage = "No Any Token Generated For Invoice Posing";
            //    IntIsError = 1;
            //    return;
            //}
            //else
            //{
            //    DataSet dsJson = ObjEInvoice.GetEInvoiceInvoiceInfo(Val.ToString(lblMemoNo.Tag));
            //    if (dsJson.Tables.Count == 0)
            //    {
            //        StrMessage = "No Data Found For Upload";
            //        IntIsError = 1;
            //        return;
            //    }
            //    if (dsJson.Tables[0].Columns[0].ToString() == "Json")
            //    {
            //        string JsonData = Val.ToString(dsJson.Tables[0].Rows[0]["Json"]);
            //        GetINRData ObjData = EAPI.GenerateINR(Property.CLIENTID, Property.CLIENTSECRET, Property.GSTIN, Property.USERNAME, AuthToken, Skey, JsonData);
            //        if (ObjData.IrnNo != null)
            //        {
            //            AckNo = ObjData.AckNo;
            //            IrnNo = ObjData.IrnNo;
            //            SignedInvoice = ObjData.SignedInvoice;
            //            SignedQRCode = ObjData.SignedQRCode;
            //            IrnDate = ObjData.AckDt;
            //            ObjEInvoice.UpdateEInvoiceUploadDate(IrnDate, Val.ToString(lblMemoNo.Tag));
            //            int RetValue = ObjEInvoice.InsertEInvoiceDetail(Val.ToString(lblMemoNo.Tag), AckNo, IrnNo, Val.SqlDate(IrnDate), SignedInvoice, SignedQRCode, null, Property);
            //            if (RetValue == -1)
            //            {
            //                StrMessage = "Record Not Inserted";
            //                MessageBox.Show(StrMessage);
            //                IntIsError = 1;
            //                return;
            //            }
            //            else
            //            {
            //                StrMessage = "E-Invoice Upload Successfully";
            //                MessageBox.Show(StrMessage);
            //                IntIsError = 0;
            //                string StrFilePath = System.Windows.Forms.Application.StartupPath + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + txtVoucherNoStr.Text + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");
            //                if (File.Exists(StrFilePath))
            //                {
            //                    File.Delete(StrFilePath);
            //                }
            //                // Write that JSON to txt file,  
            //                System.IO.File.WriteAllText(StrFilePath + "output.json", JsonData);
            //                MessageBox.Show("Json file Generated!");
            //            }
            //        }
            //        else
            //        {
            //            StrMessage = ObjData.ErrorMeeasge;
            //            String StrReqId = Obj.AuthToken;
            //            IntIsError = 1;
            //            //ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, StrMessage, Val.ToString(lblMemoNo.Tag));
            //            //MessageBox.Show(StrMessage);
            //            return;
            //        }
            //    }
            //    else if (dsJson.Tables[0].Columns[0].ToString() == "Message")
            //    {
            //        StrMessage = Val.ToString(dsJson.Tables[0].Rows[0]["Message"]);
            //        MessageBox.Show(StrMessage);
            //        return;
            //    }
            //}
        }


        public async void EInvoiceCancel()
        {
            try
            {
                ReqPlCancelIRN reqPlCancelIRN = new ReqPlCancelIRN();
                TRN_EInvoiceProperty Property = ObjEInvoice.GetEInvoiceCredential(BusLib.Configuration.BOConfiguration.COMPANY_ID);
                DataSet dsJson = ObjEInvoice.GetEInvoiceCancelInvoiceInfo(Val.ToString(lblMemoNo.Tag));
                if (dsJson.Tables[0].Columns[0].ToString() == "Json")
                {
                    if (dsJson.Tables.Count == 0)
                    {
                        StrMessage = "No Data Found For Upload";
                        IntIsError = 1;
                        return;
                    }
                    if (dsJson.Tables[0].Columns[0].ToString() == "Json")
                    {
                        string JsonData = Val.ToString(dsJson.Tables[0].Rows[0]["Json"]);

                        dt = ConvertJsonToDataTable(JsonData);

                        if (dt.Rows.Count > 0)
                        {
                            reqPlCancelIRN.Irn = Val.ToString(dt.Rows[0]["Irn"]);
                            reqPlCancelIRN.CnlRem = Val.ToString(dt.Rows[0]["CnlRem"]);
                            reqPlCancelIRN.CnlRsn = Val.ToString(dt.Rows[0]["CnlRsn"]);
                        }
                        TxnRespWithObj<RespPlCancelIRN> txnRespWithObj = await eInvoiceAPI.CancelIRNIRNAsync(eInvSession, reqPlCancelIRN);
                        if (txnRespWithObj.IsSuccess)
                        {
                            rtbResponce = JsonConvert.SerializeObject(txnRespWithObj.RespObj);
                            StrCancelDate = txnRespWithObj.RespObj.CancelDate;
                            IrnNo = txnRespWithObj.RespObj.Irn;
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
                            string dir = System.Windows.Forms.Application.StartupPath + "\\" + "JSON";
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }
                            string Path = System.Windows.Forms.Application.StartupPath + "\\" + "JSON" + "\\" + "E_INV_CANCEL_" + txtInvoiceNo.Text + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                            if (File.Exists(Path))
                            {
                                File.Delete(Path);
                                using (var tw = new StreamWriter(Path, true))
                                {
                                    tw.WriteLine(JsonData.ToString());
                                    tw.Close();
                                }
                            }
                            else if (!File.Exists(Path))
                            {
                                using (var tw = new StreamWriter(Path, true))
                                {
                                    tw.WriteLine(JsonData.ToString());
                                    tw.Close();
                                }
                            }
                        }
                        else
                        {
                            rtbResponce = txnRespWithObj.TxnOutcome;
                        }
                    }
                    else
                    {
                        string StrMessage = Val.ToString(dsJson.Tables[0].Rows[0]["Message"]);
                        Global.Message(StrMessage);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
            //BOTRN_EInvoiceAPI EAPI = new BOTRN_EInvoiceAPI();
            //TRN_EInvoiceProperty Property = ObjEInvoice.GetEInvoiceCredential(BusLib.Configuration.BOConfiguration.COMPANY_ID);
            //GetTokenData Obj = EAPI.GetToken(Property.CLIENTID, Property.CLIENTSECRET, Property.GSTIN);
            //if (Obj.AuthToken.Length > 0)
            //{
            //    AuthToken = Obj.AuthToken;
            //    Skey = Obj.Key;
            //}
            //if (AuthToken == "")
            //{
            //    StrMessage = "No Any Token Generated For Invoice Posing";
            //    IntIsError = 1;
            //    return;
            //}
            //else
            //{
            //    DataSet dsJson = ObjEInvoice.GetEInvoiceCancelInvoiceInfo(Val.ToString(lblMemoNo.Tag));
            //    if (dsJson.Tables[0].Columns[0].ToString() == "Json")
            //    {
            //        if (dsJson.Tables.Count == 0)
            //        {
            //            StrMessage = "No Data Found For Upload";
            //            IntIsError = 1;
            //            return;
            //        }
            //        if (dsJson.Tables[0].Columns[0].ToString() == "Json")
            //        {
            //            string JsonData = Val.ToString(dsJson.Tables[0].Rows[0]["Json"]);
            //            GetCancelINR ObjCancel = EAPI.CancelNR(Property.CLIENTID, Property.CLIENTSECRET, Property.GSTIN, Property.USERNAME, AuthToken, JsonData, Skey);
            //            if (ObjCancel.Irn != null)
            //            {
            //                IrnNo = ObjCancel.Irn;
            //                StrCancelDate = ObjCancel.CancelDate;
            //                ObjEInvoice.UpdateEInvoiceCancelDate(StrCancelDate, Val.ToString(lblMemoNo.Tag));
            //                int RetValue = ObjEInvoice.InsertEInvoiceDetail(Val.ToString(lblMemoNo.Tag), AckNo, IrnNo, Val.SqlDate(IrnDate), SignedInvoice, SignedQRCode, StrCancelDate, Property);
            //                if (RetValue == -1)
            //                {
            //                    StrMessage = "Record Not Inserted";
            //                    IntIsError = 1;
            //                    return;
            //                }
            //                StrMessage = "E-Invoice Cancled Successfully";
            //                IntIsError = 0;
            //            }
            //            else
            //            {
            //                IntIsError = 1;
            //                return;
            //            }
            //        }
            //    }
            //}
        }


        //public void EInvoiceCancel()
        //{
        //    AuthToken = GetToken(ref key);
        //    if (AuthToken == "")
        //    {
        //        StrMessage = "No Any Token Generated For Invoice Posing";
        //        IntIsError = 1;
        //        return;
        //    }
        //    else
        //    {
        //        DataSet dsJson = ObjEInvoice.GetEInvoiceCancelInvoiceInfo(Val.ToString(lblMemoNo.Tag));

        //        if (dsJson.Tables.Count == 0)
        //        {
        //            StrMessage = "No Data Found For Upload";
        //            IntIsError = 1;
        //            return;
        //        }

        //        if (dsJson.Tables[0].Columns[0].ToString() == "Json")
        //        {
        //            TRN_EInvoiceProperty Property = ObjEInvoice.GetEInvoiceCredential(BusLib.Configuration.BOConfiguration.COMPANY_ID);
        //            string StrReqId = "SRDCNCLREQID" + ObjEInvoice.GetMaxRequstId("EINVOICEREQUESTID");
        //            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xC0 | 0x300 | 0xC00);
        //            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Property.URL + "/Invoice/cancel");
        //            request.Method = "POST";
        //            request.KeepAlive = true;
        //            request.ProtocolVersion = HttpVersion.Version10;
        //            request.ServicePoint.Expect100Continue = false;
        //            request.AllowAutoRedirect = false;
        //            request.Accept = "*/*";
        //            request.UnsafeAuthenticatedConnectionSharing = true;
        //            request.ContentType = "application/json";
        //            request.Headers.Add("user_name", Property.USERNAME);
        //            request.Headers.Add("password", Property.PASSWORD);
        //            request.Headers.Add("Gstin", Property.GSTIN);
        //            request.Headers.Add("requestid", StrReqId);
        //            request.Headers.Add("Authorization", AuthToken);

        //            byte[] _aeskey = GenerateSecureKey();
        //            string straesKey = Convert.ToBase64String(_aeskey);

        //            string JsonData = Val.ToString(dsJson.Tables[0].Rows[0]["Json"]);
        //            JsonData = JsonData.Replace("\t", "");
        //            var serializer = new JavaScriptSerializer();
        //            using (var StreamWriter = new StreamWriter(request.GetRequestStream()))
        //            {
        //                StreamWriter.Write(JsonData);
        //                StreamWriter.Flush();
        //                StreamWriter.Close();
        //            }

        //            try
        //            {
        //                WebResponse response = request.GetResponse();
        //                string Result = new StreamReader(response.GetResponseStream()).ReadToEnd();

        //                Result = Result.Replace("result", "");
        //                Result = Result.Replace(":{", "");
        //                DataTable dtresult = ConvertJsonToDataTable(Result);

        //                if (dtresult.Rows.Count > 0)
        //                {
        //                    if (Val.ToString(dtresult.Rows[0]["success"]) == "false")
        //                    {
        //                        StrMessage = Val.ToString(dtresult.Rows[0]["message"]);
        //                        StrMessage = StrMessage.Replace("'", "");
        //                        IntIsError = 1;
        //                        ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, StrMessage, Val.ToString(lblMemoNo.Tag));
        //                        IntIsError = 1;
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        foreach (DataRow dr in dtresult.Rows)
        //                        {
        //                            StrMessage = Val.ToString(dr["message"]);
        //                            IrnNo = Val.ToString(dr["Irn"]);
        //                            StrCancelDate = Val.ToString(dr["CancelDate"]);
        //                        }
        //                        ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, Val.ToString(dtresult.Rows[0]["message"]), Val.ToString(lblMemoNo.Tag));
        //                        ObjEInvoice.UpdateEInvoiceCancelDate(StrCancelDate, Val.ToString(lblMemoNo.Tag));
        //                        int RetValue = ObjEInvoice.InsertEInvoiceDetail(Val.ToString(lblMemoNo.Tag), AckNo, IrnNo, Val.SqlDate(IrnDate), SignedInvoice, SignedQRCode, StrCancelDate, Property);
        //                        if (RetValue == -1)
        //                        {
        //                            StrMessage = "Record Not Inserted";
        //                            IntIsError = 1;
        //                            return;
        //                        }

        //                        StrMessage = "E-Invoice Cancled Successfully";
        //                        IntIsError = 0;
        //                    }
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                StrMessage = ex.Message.ToString();
        //                IntIsError = 1;
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            StrMessage = Val.ToString(dsJson.Tables[0].Rows[0][0]);
        //            IntIsError = 1;
        //        }

        //    }
        //}

        //public void EInvoicePrint()
        //{
        //    DataRow DRow = ObjEInvoice.GetEInvoiceExists(Val.ToString(lblMemoNo.Tag));
        //    if (DRow == null)
        //    {
        //        StrMessage = "First Generate IRN Number For E-Invoice , Upload Your Invoice";
        //        IntIsError = 1;
        //        return;
        //    }

        //    AuthToken = GetToken(ref key);
        //    if (AuthToken == "")
        //    {
        //        StrMessage = "No Any Token Generated For Invoice Posing";
        //        IntIsError = 1;
        //        return;
        //    }
        //    else
        //    {
        //        {
        //            string public_key = "";

        //            TRN_EInvoiceProperty Property = ObjEInvoice.GetEInvoiceCredential(BusLib.Configuration.BOConfiguration.COMPANY_ID);
        //            using (var reader = File.OpenText(Application.StartupPath + @"\\einv_sandbox.pem"))
        //            {
        //                public_key = reader.ReadToEnd().Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace(Constants.vbLf, "");
        //            }

        //            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xC0 | 0x300 | 0xC00);
        //            string StrReqId = "SRDPRNTREQID" + ObjEInvoice.GetMaxRequstId("EINVOICEREQUESTID");
        //            string StrUrl = Property.URL + "/Invoice/irn?irn=" + Val.ToString(DRow["IRNNO"]);
        //            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(StrUrl);
        //            request.Method = "GET";
        //            request.KeepAlive = true;
        //            request.ProtocolVersion = HttpVersion.Version10;
        //            request.ServicePoint.Expect100Continue = false;
        //            request.AllowAutoRedirect = false;
        //            request.Accept = "*/*";
        //            request.UnsafeAuthenticatedConnectionSharing = true;
        //            request.ContentType = "application/json";
        //            request.Headers.Add("user_name", Property.USERNAME);
        //            request.Headers.Add("password", Property.PASSWORD);
        //            request.Headers.Add("Gstin", Property.GSTIN);
        //            request.Headers.Add("requestid", StrReqId);
        //            request.Headers.Add("Authorization", AuthToken);

        //            try
        //            {
        //                WebResponse response = request.GetResponse();
        //                string Result = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //                Result = Result.Replace("result", "");
        //                Result = Result.Replace(":{", "");
        //                DataTable dtresult = ConvertJsonToDataTable(Result);
        //                if (dtresult.Rows.Count > 0)
        //                {
        //                    if (Val.ToString(dtresult.Rows[0]["success"]) == "false")
        //                    {
        //                        StrMessage = Val.ToString(dtresult.Rows[0]["message"]);
        //                        StrMessage = StrMessage.Replace("'", "");
        //                        IntIsError = 1;
        //                        ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, StrMessage, Val.ToString(lblMemoNo.Tag));
        //                        IntIsError = 1;
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        foreach (DataRow dr in dtresult.Rows)
        //                        {
        //                            AckNo = Val.ToString(dr["AckNo"]);
        //                            IrnNo = Val.ToString(dr["Irn"]);
        //                            IrnDate = Val.ToString(dr["AckDt"]);
        //                            SignedInvoice = Val.ToString(dr["SignedInvoice"]);
        //                            SignedQRCode = Val.ToString(dr["SignedQRCode"]);
        //                        }
        //                        ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, Val.ToString(dtresult.Rows[0]["message"]), Val.ToString(lblMemoNo.Tag));
        //                        string result2 = "";
        //                        string resultFinal = "";
        //                        result2 = Decode(SignedInvoice);
        //                        resultFinal = DecodePayload(SignedInvoice);
        //                        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //                        Acct_EinvoiceApiRequest Einvre = serializer.Deserialize<Acct_EinvoiceApiRequest>(resultFinal);
        //                        string Data = Einvre.Data;
        //                        var dsRDS = new DataSet();
        //                        var dtFinal = new DataTable();
        //                        XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(Data, "friends");
        //                        string str1 = doc.InnerXml.Replace("<TranDtls>", "").Replace("</TranDtls>", "").Replace("<DocDtls>", "").Replace("</DocDtls>", "").Replace("<SellerDtls>", "").Replace("</SellerDtls>", "").Replace("<BuyerDtls>", "").Replace("</BuyerDtls>", "").Replace("<DispDtls>", "").Replace("</DispDtls>", "").Replace("<ItemList>", "").Replace("</ItemList>", "").Replace("<ValDtls>", "").Replace("</ValDtls>", "");
        //                        ObjEInvoice.InsertEInvoicePrintOutPutXML(Val.ToString(lblMemoNo.Tag), str1);

        //                        doc.LoadXml(str1);
        //                        if (doc.InnerXml.Trim().Length > 0)
        //                        {
        //                            using (var stringReader = new StringReader(doc.InnerXml))
        //                            {
        //                                dsRDS = new DataSet();
        //                                dsRDS.ReadXml(stringReader, XmlReadMode.Auto);
        //                            }
        //                        }

        //                        dtFinal = dsRDS.Tables[0];
        //                        var newColumn = new System.Data.DataColumn("SignedQRCode", typeof(byte[]));
        //                        dtFinal.Columns.Add(newColumn);
        //                        var newColumnBr = new System.Data.DataColumn("Barcode", typeof(byte[]));
        //                        dtFinal.Columns.Add(newColumnBr);
        //                        var newColumnBuyer = new System.Data.DataColumn("BuyerAdd", typeof(string));
        //                        dtFinal.Columns.Add(newColumnBuyer);
        //                        var newColumnBuyerGST = new System.Data.DataColumn("BuyerGST", typeof(string));
        //                        dtFinal.Columns.Add(newColumnBuyerGST);
        //                        var newColumnSeller = new System.Data.DataColumn("SellerAdd", typeof(string));
        //                        dtFinal.Columns.Add(newColumnSeller);
        //                        var newColumnSellerGST = new System.Data.DataColumn("SellerGST", typeof(string));
        //                        dtFinal.Columns.Add(newColumnSellerGST);
        //                        var newColumnTaxRateStr = new System.Data.DataColumn("TaxRateStr", typeof(string));
        //                        dtFinal.Columns.Add(newColumnTaxRateStr);

        //                        var Pic = new PictureBox();
        //                        Image Img;
        //                        var qr = new QRCodeGenerator();
        //                        QRCodeData Qrdata = qr.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.Q);
        //                        var code = new QRCoder.QRCode(Qrdata);
        //                        Pic.Image = code.GetGraphic(2);
        //                        Img = Pic.Image;
        //                        var barcode = new Linear();
        //                        barcode.Type = BarcodeType.CODE128;
        //                        barcode.Data = AckNo;

        //                        DataSet dsJson = ObjEInvoice.GetEInvoiceInvoiceInfo(Val.ToString(lblMemoNo.Tag));

        //                        DataTable dtBuyer = dsJson.Tables[1];
        //                        DataTable dtSeller = dsJson.Tables[2];
        //                        string Str = "";
        //                        foreach (DataRow dr in dtFinal.Rows)
        //                        {
        //                            var ms = new MemoryStream();
        //                            Img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

        //                            dr["SignedQRCode"] = ms.ToArray();
        //                            dr["Barcode"] = barcode.drawBarcodeAsBytes();
        //                            dr["BuyerAdd"] = Val.ToString(dtBuyer.Rows[0]["BuyerAdd"]);
        //                            dr["BuyerGST"] = Val.ToString(dtBuyer.Rows[0]["BuyerGST"]);
        //                            dr["SellerAdd"] = Val.ToString(dtSeller.Rows[0]["SellerAdd"]);
        //                            dr["SellerGST"] = Val.ToString(dtSeller.Rows[0]["SellerGST"]);
        //                            //dr["CesRt"] = Val.Val(dr["CesRt"]) == 0 ? 0.00 : Val.Val(dr["CesRt"]);

        //                            Str = (Val.Val(dr["GstRt"]) == 0 ? "0.00" : Val.ToString(dr["GstRt"])) + "+" + (Val.Val(dr["CesRt"]) == 0 ? "0.00" : Val.ToString(dr["CesRt"])) + "|"

        //                                  + (Val.Val(dr["StateCesRt"]) == 0 ? "0.00" : Val.ToString(dr["StateCesRt"]))
        //                                  + "+"
        //                                  + (Val.Val(dr["StateCesNonAdvlAmt"]) == 0 ? "0.00" : Val.ToString(dr["StateCesNonAdvlAmt"]))
        //                                  ;

        //                            dr["TaxRateStr"] = Val.ToString(Str);

        //                        }
        //                        this.BeginInvoke(new MethodInvoker(delegate
        //                        {
        //                            Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
        //                            FrmReportViewer.MdiParent = Global.gMainRef;
        //                            FrmReportViewer.ShowForm("rptEInvoice", dtFinal);
        //                        }));
        //                    }
        //                }
        //                IntIsError = -1;
        //            }
        //            catch (Exception ex)
        //            {
        //                Global.MessageError(ex.Message.ToString());
        //                return;
        //            }
        //        }
        //    }
        //}

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
                //EInvoiceCancel();
            }
            else if (Val.ToInt32(e.Argument) == 3)
            {
                //EInvoicePrint();
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
                cmbAccType.SelectedIndex = 0;

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

                txtTerms.Text = string.Empty;
                txtTermsDays.Text = string.Empty;
                DTPTermsDate.Text = Val.ToString(DateTime.Now);

                txtMemoAvgDisc.Text = string.Empty;
                txtMemoAvgRate.Text = string.Empty;
                txtMemoAmount.Text = string.Empty;
                txtRemark.Text = string.Empty;
                txtTransport.Text = string.Empty;

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

                //DTPMemoDate.Focus();
                txtBillingParty.Focus();
                DTabMemoDetail.Rows.Clear();
                MainGrdDetail.Refresh();

                txtBackPriceFileName.Text = string.Empty;
                txtBackAddLess.Text = string.Empty;
                txtTermsAddLessPer.Text = string.Empty;
                txtBlindAddLessPer.Text = string.Empty;

                txtBuyer.Text = string.Empty;
                txtBuyer.Tag = string.Empty;
                chkIsConsingee.Checked = false;
                txtThrough.Text = string.Empty;
                PnlAccPrint.Visible = false;

                txtCGSTAmountFE.Text = string.Empty;
                txtSGSTAmountFE.Text = string.Empty;
                txtIGSTAmountFE.Text = string.Empty;

                txtBrokerExcRate.Text = string.Empty;
                txtAdatExcRate.Text = string.Empty;

                txtGRFreight.Text = string.Empty;
                txtBroTdsPer.Text = string.Empty;
                txtBroTdsRs.Text = string.Empty;
                txtBroTAmt.Text = string.Empty;
                rdoBroTDS.SelectedIndex = 1;

                txtBroCgstPer.Text = string.Empty;
                txtBroCgstAmt.Text = string.Empty;
                txtBroSgstPer.Text = string.Empty;
                txtBroSgstAmt.Text = string.Empty;
                txtBroIgstPer.Text = string.Empty;
                txtBroIgstAmt.Text = string.Empty;
                rdBroGST.SelectedIndex = 1;


                if (MenuTagName == "SALEINVOICEENTRY")
                {
                    //DataSet DS = ObjMemo.GetMemoListData(9, null, null, "", "", "", 0, "", 0, "", "ALL", SaleInvoiceMemoID, "SINGLE", false
                    //, -1);



                    DataSet DS = ObjMast.GetDataForSaleInvoiceList(9, "ALL", SaleInvoiceMemoID, "SINGLE", "", "", "");
                    DTabMemo = DS.Tables[0];
                    DTabMemoDetail = DS.Tables[1];
                    DTabMemoDetailParcelFile = DS.Tables[2];

                    if (DTabMemoDetail.Rows.Count > 0)
                    {
                        if (DTabMemoDetail.Rows[0]["STOCKTYPE"].ToString() == "PARCEL" || DTabMemoDetail.Rows[0]["STOCKTYPE"].ToString() == "SINGLE" || DTabMemoDetail.Rows[0]["STOCKTYPE"].ToString() == "")
                        {
                            if (DTabMemo.Rows[0]["ACCTYPE"].ToString() == "Local Sales")
                            {
                                DTabMemo.Rows[0]["REMARK"] = "";
                                for (int i = 0; i < DTabMemoDetail.Rows.Count; i++)
                                {
                                    DTabMemoDetail.Rows[i]["ITEMNAME"] = "Cut & Polished Diamond";
                                    DTabMemoDetail.Rows[i]["HSNCODE"] = "71023910";
                                    DTabMemoDetail.Rows[i]["REMARK"] = "";
                                }
                            }
                            else if (DTabMemo.Rows[0]["ACCTYPE"].ToString() == "Diamond Dollar")
                            {
                                DTabMemo.Rows[0]["REMARK"] = "";
                                for (int i = 0; i < DTabMemoDetail.Rows.Count; i++)
                                {
                                    DTabMemoDetail.Rows[i]["ITEMNAME"] = "Cut & Polished Diamond";
                                    DTabMemoDetail.Rows[i]["HSNCODE"] = "71023910";
                                    DTabMemoDetail.Rows[i]["REMARK"] = "";
                                    DTabMemoDetail.Rows[i]["MEMOPRICEPERCARAT"] = Math.Round(Convert.ToDecimal(DTabMemoDetail.Rows[i]["MEMOPRICEPERCARAT"]), 2);
                                    DTabMemoDetail.Rows[i]["MEMOAMOUNT"] = Math.Round(Convert.ToDecimal(DTabMemoDetail.Rows[i]["MEMOAMOUNT"]), 2);
                                }
                            }
                            else if (DTabMemo.Rows[0]["ACCTYPE"].ToString() == "Export")
                            {
                                DTabMemo.Rows[0]["REMARK"] = "";
                                for (int i = 0; i < DTabMemoDetail.Rows.Count; i++)
                                {
                                    DTabMemoDetail.Rows[i]["ITEMNAME"] = "Cut & Polished Diamond";
                                    DTabMemoDetail.Rows[i]["HSNCODE"] = "71023910";
                                    DTabMemoDetail.Rows[i]["REMARK"] = "";
                                    DTabMemoDetail.Rows[i]["MEMOPRICEPERCARAT"] = Math.Round(Convert.ToDecimal(DTabMemoDetail.Rows[i]["MEMOPRICEPERCARAT"]), 2);
                                    DTabMemoDetail.Rows[i]["MEMOAMOUNT"] = Math.Round(Convert.ToDecimal(DTabMemoDetail.Rows[i]["MEMOAMOUNT"]), 2);

                                    //if (Val.ToString(DTabMemoDetail.Rows[i]["Pcs"]) == "0")
                                    //{
                                    //    DTabMemoDetail.Rows[i]["Pcs"] = "1";
                                    //}
                                }
                            }

                        }
                    }


                    IS_CONSIRET = false;

                    MainGridSummuryMNL.DataSource = DTabMemoDetail;
                    MainGridSummuryMNL.Refresh();

                    MainGridSummuryMNLParcel.DataSource = DTabMemoDetailParcelFile;
                    MainGridSummuryMNLParcel.Refresh();

                    DataTable DTabCur = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);
                    txtCurrency.Text = Val.ToString(DTabCur.Rows[0]["CURRENCYNAME"]);
                    txtCurrency.Tag = Val.ToString(DTabCur.Rows[0]["CURRENCY_ID"]);

                    BtnContinue_Click(null, null);
                }

                //Calculation();


                CalculationNew();
                IsOutSideStone = false;
                IsManualEntryAdd = false;
                txtOutSideStoneFileName.Text = string.Empty;
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
                    if (MenuTagName == "SALEINVOICEENTRY")
                    {
                        DataRow drow = DTabMemoDetail.NewRow();
                        drow["STOCKTYPE"] = mStrStockType == "PARCEL" ? "PARCEL" : "SINGLE";
                        DTabMemoDetail.Rows.Add(drow);
                        GrdSummuryMNL.FocusedColumn = GrdSummuryMNL.VisibleColumns[0];
                        //GrdSummuryMNL.FocusedColumn = GrdSummuryMNL.Columns["ITEMNAME"];
                        GrdSummuryMNL.FocusedRowHandle = drow.Table.Rows.IndexOf(drow);
                        GrdSummuryMNL.Focus();

                    }
                    else
                    {
                        DataRow drow = DTabMemoDetail.NewRow();
                        DTabMemoDetail.Rows.Add(drow);
                        GrdDetail.FocusedRowHandle = drow.Table.Rows.IndexOf(drow);
                        GrdDetail.FocusedColumn = GrdDetail.VisibleColumns[0];
                        GrdDetail.Focus();
                        GrdDetail.ShowEditor();
                    }
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
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME,MOBILENO,EMAILID,SKYPEID";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    FrmSearch.mFilterType = FrmSearchPopupBox.FilterType.Like;

                    this.Cursor = Cursors.WaitCursor;

                    //FrmSearch.DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_SALEPARTY);

                    DataTable DtabParty = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);

                    if (mFormType == FORMTYPE.PURCHASEISSUE || mFormType == FORMTYPE.PURCHASERETURN)
                        FrmSearch.mDTab = DtabParty.Select("PARTYTYPE = 'PURCHASE'").CopyToDataTable();
                    else if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN
                             || mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.ORDERCONFIRMRETURN
                             || mFormType == FORMTYPE.MEMOISSUE || mFormType == FORMTYPE.MEMORETURN
                             || mFormType == FORMTYPE.CONSIGNMENTISSUE || mFormType == FORMTYPE.CONSIGNMENTRETURN
                             || mFormType == FORMTYPE.UNGRADEDTOMIX || mFormType == FORMTYPE.GRADEDTOMIX

                       )
                        FrmSearch.mDTab = DtabParty.Select("PARTYTYPE = 'SALE'").CopyToDataTable();
                    //else if (mFormType == FORMTYPE.MEMOISSUE || mFormType == FORMTYPE.MEMORETURN)
                    //    FrmSearch.mDTab = DtabParty.Select("PARTYTYPE IN ('PURCHASE','SALE','EMPLOYEE')").CopyToDataTable();
                    else
                        FrmSearch.mDTab = DtabParty;
                    ////Gunjan:19/04/2025
                    //if (mFormType == FORMTYPE.ORDERCONFIRM)
                    //{
                    FrmSearch.mBoolISPostBack = true;
                    FrmSearch.mStrISPostBackColumn = "PARTYNAME";
                    //}
                    //End As Gunjan
                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID,PARTYTYPE,COMPANYNAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBillingParty.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBillingParty.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);

                        if (Val.ToString(FrmSearch.DRow["IS_SECADDRESS"]) == "True")
                        {
                            txtBAddress1.Text = Val.ToString(FrmSearch.DRow["SEC_ADDRESS"]);
                            txtBAddress2.Text = Val.ToString(FrmSearch.DRow["Sec_Address1"]);
                            txtBAddress3.Text = Val.ToString(FrmSearch.DRow["Sec_Address2"]);
                            txtBCity.Text = Val.ToString(FrmSearch.DRow["BILLINGCITY"]);
                            txtBCountry.Tag = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRY_ID"]);
                            txtBCountry.Text = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRYNAME"]);
                            txtBState.Text = Val.ToString(FrmSearch.DRow["BILLINGSTATE"]);
                            txtBZipCode.Text = Val.ToString(FrmSearch.DRow["BILLINGZIPCODE"]);
                        }
                        else
                        {
                            txtBAddress1.Text = Val.ToString(FrmSearch.DRow["BILLINGADDRESS1"]);
                            txtBAddress2.Text = Val.ToString(FrmSearch.DRow["BILLINGADDRESS2"]);
                            txtBAddress3.Text = Val.ToString(FrmSearch.DRow["BILLINGADDRESS3"]);
                            txtBCity.Text = Val.ToString(FrmSearch.DRow["BILLINGCITY"]);
                            txtBCountry.Tag = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRY_ID"]);
                            txtBCountry.Text = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRYNAME"]);
                            txtBState.Text = Val.ToString(FrmSearch.DRow["BILLINGSTATE"]);
                            txtBZipCode.Text = Val.ToString(FrmSearch.DRow["BILLINGZIPCODE"]);
                        }

                        txtBroker.Tag = Val.ToString(FrmSearch.DRow["COORDINATOR_ID"]);
                        txtBroker.Text = Val.ToString(FrmSearch.DRow["COORDINATORNAME"]);

                        txtFinalDestination.Text = Val.ToString(FrmSearch.DRow["BILLINGCITY"]);
                        txtCountryOfFinalDestination.Text = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRYNAME"]);

                        lblKYCType.Text = Val.ToString(FrmSearch.DRow["KYCTYPE"]);

                        AutoGstCalculationNew();
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

                        //txtSAddress1.Text = Val.ToString(FrmSearch.DRow["SHIPPINGADDRESS1"]);
                        //txtSAddress2.Text = Val.ToString(FrmSearch.DRow["SHIPPINGADDRESS2"]);
                        //txtSAddress3.Text = Val.ToString(FrmSearch.DRow["SHIPPINGADDRESS3"]);
                        //txtSCity.Text = Val.ToString(FrmSearch.DRow["SHIPPINGCITY"]);
                        //txtSCountry.Text = Val.ToString(FrmSearch.DRow["SHIPPINGCOUNTRYNAME"]);
                        //txtSCountry.Tag = Val.ToString(FrmSearch.DRow["SHIPPINGCOUNTRY_ID"]);
                        //txtSState.Text = Val.ToString(FrmSearch.DRow["SHIPPINGSTATE"]);
                        //txtSZipCode.Text = Val.ToString(FrmSearch.DRow["SHIPPINGZIPCODE"]);

                        txtSAddress1.Text = Val.ToString(FrmSearch.DRow["BILLINGADDRESS1"]);
                        txtSAddress2.Text = Val.ToString(FrmSearch.DRow["BILLINGADDRESS2"]);
                        txtSAddress3.Text = Val.ToString(FrmSearch.DRow["BILLINGADDRESS3"]);
                        txtSCity.Text = Val.ToString(FrmSearch.DRow["BILLINGCITY"]);
                        txtSCountry.Text = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRYNAME"]);
                        txtSCountry.Tag = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRY_ID"]);
                        txtSState.Text = Val.ToString(FrmSearch.DRow["BILLINGSTATE"]);
                        txtSZipCode.Text = Val.ToString(FrmSearch.DRow["BILLINGZIPCODE"]);
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
            double TotalCarat = 0, TotalCaratParcel = 0;
            foreach (DataRow dr in DTabMemoDetail.Rows)
            {
                if (MenuTagName == "")
                {
                    //For Update Validation
                    if (Val.ToString(dr["PARTYSTOCKNO"]).Trim().Equals(string.Empty) && !Val.ToString(dr["STOCK_ID"]).Trim().Equals(string.Empty))
                    {
                        Global.Message("Please Enter 'Client Ref. No'");
                        IntCol = 1;
                        IntRow = dr.Table.Rows.IndexOf(dr);
                        break;
                    }
                    //end as


                    //if (Val.ToDouble(dr["JANGEDDISCOUNT"]) == 0)
                    //{
                    //    Global.Message("Please Enter Janged Discount..");
                    //    IntCol = 1;
                    //    IntRow = dr.Table.Rows.IndexOf(dr);
                    //    break;
                    //}

                    //if (Val.ToDouble(dr["JANGEDPRICEPERCARAT"]) == 0)
                    //{
                    //    Global.Message("Please Enter Janged Rate..");
                    //    IntCol = 1;
                    //    IntRow = dr.Table.Rows.IndexOf(dr);
                    //    break;
                    //}

                    if (mFormType == FORMTYPE.PURCHASEISSUE)
                    {
                        dr["PARTYSTOCKNO"] = dr["STOCKNO"];
                    }
                    if (Val.ToString(dr["PARTYSTOCKNO"]).Trim().Equals(string.Empty))
                    {
                        if (DTabMemoDetail.Rows.Count == 1)
                        {
                            Global.Message("Please Enter 'Client Ref. No.'");
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
                TotalCarat += Val.Val(dr["Carat"]);
            }
            foreach (DataRow dr in DTabMemoDetailParcelFile.Rows)
            {
                TotalCaratParcel += Val.Val(dr["Carat"]);
            }
            TotalCaratParcel = Math.Round(TotalCaratParcel, 3);
            TotalCarat = Math.Round(TotalCarat, 3);
            ////if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE && cmbBillType.Text == "Export" && IsOutSideStone == true)
            ////{
            ////    if(TotalCarat != TotalCaratParcel)
            ////    {
            ////        Global.Message("Please Check File Upload Carat And Stone Carat.");
            ////        return true;
            ////    }
            ////}
            //if(BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE && cmbBillType.Text == "Export" && lblMode.Text == "Edit Mode" && DTabMemoDetailParcelFile.Rows.Count > 0 && IsOutSideStone == true)
            //////if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE && cmbBillType.Text == "Export" && lblMode.Text == "Edit Mode" && IsOutSideStone == true)
            //{
            //    if (TotalCarat != TotalCaratParcel)
            //    {
            //        Global.Message("Please Check File Upload Carat And Stone Carat.");
            //        return true;
            //    }
            //}
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
                if (txtBillingParty.Text.Length == 0)
                {
                    Global.Message("Billing Party Is Required");
                    txtBillingParty.Focus();
                    return;
                }

                //if (txtBCountry.Text.Length == 0)
                //{
                //    Global.Message("Billing Country Is Required");
                //    txtBCountry.Focus();
                //    return;
                //}

                //if (txtSellerName.Text.Length == 0)
                //{
                //    Global.Message("Seller Name Is Required");
                //    txtSellerName.Focus();
                //    return;
                //}

                if (txtTerms.Text.Length == 0)
                {
                    Global.Message("Terms Is Required");
                    txtTerms.Focus();
                    return;
                }

                //if (Val.ToString(txtCurrency.Text).Trim().Equals(string.Empty))
                //{
                //    Global.Message("Currency Is Required");
                //    txtCurrency.Focus();
                //    return;
                //}

                if (Val.ToString(cmbBillType.SelectedItem) == "None" && (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.PURCHASEISSUE))
                {
                    Global.Message("Please Select BillType");
                    cmbBillType.Focus();
                    return;
                }

                //Add Shiv AccType
                //if (Val.ToString(cmbAccType.SelectedItem) == "None" && (mFormType == FORMTYPE.SALEINVOICE))
                //{
                //    Global.Message("Please Select AccType");
                //    cmbAccType.Focus();
                //    return;
                //}

                //if (Val.ToString(CmbPaymentMode.SelectedItem) == "None" && (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.SALEINVOICE))
                //{
                //    Global.Message("Please Select PaymentMode");
                //    CmbPaymentMode.Focus();
                //    return;
                //}

                //if (Val.Val(txtExcRate.Text) == 0)
                //{
                //    Global.Message("Please Enter ExcRate.");
                //    txtExcRate.Focus();
                //    return;
                //}

                //if (Val.ToString(txtCompanyBank.Text).Length == 0 && mFormType == FORMTYPE.SALEINVOICE)
                //{
                //    Global.Message("Please Enter CompanyBank");
                //    txtCompanyBank.Focus();
                //    xtraTabMasterPanel.SelectedTabPageIndex = 1;
                //    return;
                //}

                //if (Val.ToString(cmbAddresstype.SelectedItem) == "NONE" && mFormType == FORMTYPE.SALEINVOICE)
                //{
                //    Global.Message("Please Select Company Address Type");
                //    cmbAddresstype.Focus();
                //    xtraTabMasterPanel.SelectedTabPageIndex = 1;
                //    return;
                //}

                //// #D: 01-03-2021
                //if (MenuTagName == "SALEINVOICEENTRY")
                //{
                //    if (cmbBillType.Text.ToUpper() == "RUPEESBILL")
                //    {
                //        //txtExcRate.Text Validation Skip
                //    }
                //}
                //else
                //{
                //    //if (Val.Val(txtExcRate.Text) == 0 || Val.Val(txtExcRate.Text) == 1)
                //    //{
                //    //    Global.Message("Please Enter Proper ExcRate");
                //    //    txtExcRate.Focus();
                //    //    xtraTabMasterPanel.SelectedTabPageIndex = 0;
                //    //    return;
                //    //}
                //}
                // #D: 01-03-2021

                // D: 19/05/2021
                //if (chkIsConsingee.Checked == true && Val.ToString(txtBuyer.Text).Length == 0 && (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM))
                //{
                //    Global.Message("Please Enter Final Buyer Detail");
                //    txtBuyer.Focus();
                //    xtraTabMasterPanel.SelectedTabPageIndex = 0;
                //    return;
                //}
                // D: 19/05/2021

                //if (Val.ToString(lblKYCType.
                //Text) == "BUYERKYC" && mFormType == FORMTYPE.ORDERCONFIRM || Val.ToString(lblKYCType.Text) == "BUYERKYC" && mFormType == FORMTYPE.SALEINVOICE)
                //{
                //    Global.Message("Please Select BillingKyc Party...");
                //    txtBillingParty.Focus();
                //    return;
                //}


                //S 27-08-2022
                if (cmbAccType.SelectedText == "Local Sales")
                {
                    if (Val.Val(txtTCSAmountFE.Text) == 0.00)
                    {
                        //double DouMemoAmount = Val.Val(txtGrossAmountFE.Text);
                        //double DouLedgerTotAmt = Val.Val(lblCrdlimit.Text);
                        //if (DouLedgerTotAmt > dblTCSAmount)
                        //{
                        //    //txtTCSCalcAmt.Text = Val.ToString(DouMemoAmount);
                        //    double dblDiff = dblTCSAmount - (DouMemoAmount + DouLedgerTotAmt);
                        //    if (dblDiff > 0)
                        //    {
                        //        txtTCSCalcAmt.Text = "0.00";
                        //    }
                        //    else
                        //    {
                        //        Global.Message("TDS Limit Exists Fifty lakh Above Please Add Tds.......");
                        //        txtTCSPer_Validated(null, null);
                        //    }
                        //}
                        //else
                        //{
                        //    double dblDiff = dblTCSAmount - (DouMemoAmount + DouLedgerTotAmt);
                        //    if (dblDiff > 0)
                        //    {
                        //        txtTCSCalcAmt.Text = "0.00";
                        //    }
                        //    else
                        //    {
                        //        txtTCSCalcAmt.Text = Val.ToString(Math.Abs(dblDiff));
                        //        Global.Message("TDS Limit Exists Fifty lakh Above Please Add Tds.......");
                        //        txtTCSPer_Validated(null, null);
                        //    }
                        //}
                        DataTable DTMAX = new BOMST_Ledger().GetLedgerDataForTDSCreditLimit(Val.ToString(txtBillingParty.Tag));
                        double DouMemoAmount = Val.Val(txtGrossAmountFE.Text);
                        double DouLedgerTotAmt = Val.Val(lblCrdlimit.Text);
                        if (DouLedgerTotAmt > dblTCSAmount)
                        {
                            if (DTMAX.Rows.Count > 0)
                            {
                                IS_MAXLIMIT = Val.ToBoolean(DTMAX.Rows[0]["IS_Maxlimit"]);
                            }
                            if (IS_MAXLIMIT == true)
                            {
                                if (DouLedgerTotAmt > dblTCSAmount)
                                {
                                    double dblDiff = dblTCSAmount - (DouLedgerTotAmt);
                                    if (dblDiff > 0)
                                    {
                                        txtTCSCalcAmt.Text = "0.00";
                                    }
                                    else
                                    {
                                        Global.Message("TDS Limit Exists Fifty lakh Above Please Add Tds.......");
                                        txtTCSCalcAmt.Text = Val.ToString(Math.Abs(dblDiff));
                                    }
                                }
                            }
                            else
                            {
                                Global.Message("TDS Limit Exists Fifty lakh Above Please Add Tds.......");
                                txtTCSCalcAmt.Text = Val.ToString(DouMemoAmount);
                            }
                        }
                        else
                        {
                            txtTCSCalcAmt.Text = "0.00";
                        }
                    }
                }

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

                //if (mFormType == FORMTYPE.LABISSUE && txtLabServiceCode.Text.Length == 0)
                //{
                //    Global.Message("Please Select Lab Service Code");
                //    txtLabServiceCode.Focus();
                //    xtraTabMasterPanel.SelectedTabPageIndex = 1;
                //    return;
                //}

                //Add shiv
                if (Autosave != true)
                {
                    if (Global.Confirm("Are You Sure For Goods Entry") == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }

                MemoEntryProperty Property = new MemoEntryProperty();

                if (lblMode.Text == "Add Mode")
                {
                    lblMemoNo.Tag = BusLib.Configuration.BOConfiguration.FindNewSequentialID().ToString();
                }

                Property.MEMO_ID = Val.ToString(lblMemoNo.Tag);


                if (lblMode.Text == "Edit Mode")
                {
                    Property.MEMO_ID = Val.ToString(lblMemoNo.Tag);
                }

                Property.JANGEDNOSTR = txtJangedNo.Text;
                Property.MEMONO = Val.ToInt64(lblMemoNo.Text);
                Property.MEMOTYPE = Val.ToString(CmbMemoType.SelectedItem);
                Property.MEMODATE = Val.SqlDate(DTPMemoDate.Text);
                Property.THROUGH = Val.ToString(txtThrough.Text);

                if (txtBillingParty.Tag != "")
                {
                    Property.BILLINGPARTY_ID = Val.ToString(txtBillingParty.Tag);
                }
                else
                {
                    Property.BILLINGPARTY_ID = "00000000-0000-0000-0000-000000000000";
                }

                Property.BILLINGPARTNAME = Val.ToString(txtBillingParty.Text);

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
                //if (txtAdat.Text.Trim().Length != 0)
                //{
                //    Property.ADAT_ID = Val.ToString(txtAdat.Tag);
                //    Property.ADATPER = Val.Val(txtAdatPer.Text);
                //    Property.AdatAmtFE = Val.Val(txtAdatAmtFE.Text);
                //    Property.AdatAmt = Val.Val(txtAdatAmt.Text);
                //}
                //else
                //{
                //    Property.ADAT_ID = null;
                //    Property.ADATPER = 0.00;
                //    Property.AdatAmtFE = 0.00;
                //    Property.AdatAmt = 0.00;

                //}
                if (txtAdat.Text.Trim().Length != 0)
                {
                    Property.ADAT_ID = Val.ToString(txtAdat.Tag);
                }
                else
                {
                    Property.ADAT_ID = null;
                }
                Property.ADATPER = Val.Val(txtAdatPer.Text);
                Property.AdatAmtFE = Val.Val(txtAdatAmtFE.Text);
                Property.AdatAmt = Val.Val(txtAdatAmt.Text);

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
                Property.ACCTYPE = Val.ToString(cmbAccType.SelectedItem); //Add Shiv 13-04-2022

                Property.TOTALPCS = Val.ToInt(txtTotalPcs.Text);
                Property.TOTALCARAT = Val.Val(txtTotalCarat.Text);
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
                //Property.PLACEOFSUPPLY = txtPlaceOfSupply.Text;

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
                Property.ORDERJANGEDNO = PstrOrderJangedNo;

                if (PstrOrder_MemoId.Length == 0)
                {
                    Property.ORDERMEMO_ID = "00000000-0000-0000-0000-000000000000";
                }
                else
                {
                    Property.ORDERMEMO_ID = PstrOrder_MemoId;
                }

                Property.BILLFORMAT = (RbDollar.Checked) ? RbDollar.Tag.ToString() : RbRupee.Tag.ToString();
                string MemoEntryDetailForXML = string.Empty;
                string MemoEntryDetailForXMLParcel = string.Empty;
                DataTable DTabExpMemoDetail = new DataTable();
                if (MenuTagName == "SALEINVOICEENTRY")
                {
                    int IntI = 0;
                    foreach (DataRow DRow in DTabMemoDetail.Rows)
                    {
                        IntI++;
                        DRow["ENTRYSRNO"] = IntI;
                        //if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE)
                        //{
                        //    if (DTabMemoSummury.Rows.Count > 0)
                        //    {
                        //        if (cmbAccType.SelectedIndex == 1)
                        //        {
                        //            DRow["FMEMOPRICEPERCARAT"] = Val.ToDouble(DTabMemoSummury.Rows[0]["MEMOAVGRATEFE"]);
                        //            DRow["FMEMOAMOUNT"] = Val.ToDouble(DTabMemoSummury.Rows[0]["MEMOTOTALAMOUNTFE"]);
                        //        }
                        //    }
                        //}
                    }
                    var DRows1 = DTabMemoDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["PCS"]) == "" && Val.ToString(row["CARAT"]) == "").ToArray();
                    foreach (DataRow dr in DRows1)
                        DTabMemoDetail.Rows.Remove(dr);
                    DTabMemoDetail.AcceptChanges();

                    foreach (DataRow DRow in DTabMemoDetailParcelFile.Rows)
                    {
                        IntI++;
                        DRow["ENTRYSRNO"] = IntI;
                        if (mFormType == FORMTYPE.SALEINVOICE && Val.ToString(DRow["STOCK_ID"]) == "")
                        {
                            DRow["STOCK_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                        }
                    }

                    using (StringWriter sw = new StringWriter())
                    {
                        DTabMemoDetail.WriteXml(sw);
                        MemoEntryDetailForXML = sw.ToString();
                    }

                    using (StringWriter sw = new StringWriter())
                    {
                        DTabMemoDetailParcelFile.TableName = "TableParcel";
                        DTabMemoDetailParcelFile.WriteXml(sw);
                        MemoEntryDetailForXMLParcel = sw.ToString();
                        //MemoEntryDetailForXMLParcel.Replace("<DocumentElement>", "<NewDataSet>");
                        MemoEntryDetailForXMLParcel = MemoEntryDetailForXMLParcel.Replace("<NewDataSet>", "<DocumentElement>");
                        MemoEntryDetailForXMLParcel = MemoEntryDetailForXMLParcel.Replace("</NewDataSet>", "</DocumentElement>");
                    }
                }
                else
                {
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
                    DTabExpMemoDetail = DTabMemoDetail.Copy();

                    foreach (DataRow DRow in DTabMemoDetailParcelFile.Rows)
                    {
                        IntI++;
                        DRow["ENTRYSRNO"] = IntI;
                        if (mFormType == FORMTYPE.SALEINVOICE && Val.ToString(DRow["STOCK_ID"]) == "")
                        {
                            DRow["STOCK_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                        }
                    }

                    foreach (DataRow DRow in DTabExpMemoDetail.Rows)
                    {
                        DRow["MEMOPRICEPERCARAT"] = DRow["EXPINVOICERATE"];
                        DRow["MEMOAMOUNT"] = DRow["EXPINVOICEAMT"];
                        DRow["FMEMOPRICEPERCARAT"] = DRow["EXPINVOICERATEFE"];
                        DRow["FMEMOAMOUNT"] = DRow["EXPINVOICEAMTFE"];
                    }

                    using (StringWriter sw = new StringWriter())
                    {
                        DTabMemoDetail.WriteXml(sw);
                        MemoEntryDetailForXML = sw.ToString();
                    }

                    using (StringWriter sw = new StringWriter())
                    {
                        DTabMemoDetailParcelFile.TableName = "TableParcel";
                        DTabMemoDetailParcelFile.WriteXml(sw);
                        MemoEntryDetailForXMLParcel = sw.ToString();
                        MemoEntryDetailForXMLParcel = MemoEntryDetailForXMLParcel.Replace("<NewDataSet>", "<DocumentElement>");
                        MemoEntryDetailForXMLParcel = MemoEntryDetailForXMLParcel.Replace("</NewDataSet>", "</DocumentElement>");
                    }
                }
                string ReturnMessageDesc = "";
                string ReturnMessageType = "";
                //#K: 05122020
                Property.ORDERAPPROVAL = Val.ToBooleanToInt(ChkApprovedOrder.Checked);

                //#K : 15122020


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
                //add shiv 27-05-2022
                Property.BASEBROKERAGEEXCRATE = Val.ToDouble(txtBrokerExcRate.Text);
                Property.ADATEXCRATE = Val.ToDouble(txtAdatExcRate.Text);
                Property.GRFREIGHT = Val.ToDouble(txtGRFreight.Text);
                //add shiv 04-06-2022
                Property.BROTDSPER = Val.ToDouble(txtBroTdsPer.Text);
                Property.BROTDSRS = Val.ToDouble(txtBroTdsRs.Text);
                Property.BROTOTALAMT = Val.ToDouble(txtBroTAmt.Text);
                Property.IS_BROTDS = Val.ToBoolean(rdoBroTDS.Text);
                Property.IS_OUTSIDESTONE = IsOutSideStone;

                Property.BROCGSTPER = Val.ToDouble(txtBroCgstPer.Text);
                Property.BROCGSTRS = Val.ToDouble(txtBroCgstAmt.Text);
                Property.BROSGSTPER = Val.ToDouble(txtBroSgstPer.Text);
                Property.BROSGSTRS = Val.ToDouble(txtBroSgstAmt.Text);
                Property.BROIGSTPER = Val.ToDouble(txtBroIgstPer.Text);
                Property.BROIGSTRS = Val.ToDouble(txtBroIgstAmt.Text);
                Property.IS_BROGST = Val.ToBoolean(rdBroGST.Text);

                Property.ConsignmentRefNo = Val.ToString(txtConsignmentRefNo.Text);

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        SqlConnection cn_T;

                        cn_T = new SqlConnection(BOConfiguration.ConnectionString);
                        if (cn_T.State == ConnectionState.Open) { cn_T.Close(); }
                        cn_T.Open();

                        if (BOConfiguration.gStrLoginSection != "B")
                        {
                            if (MenuTagName == "SALEINVOICEENTRY")
                            {
                                if (IS_CONSIRET == true)
                                { Property = ObjMemo.SaveSalesRetEntry(cn_T, Property, MemoEntryDetailForXML, lblMode.Text, "", MemoEntryDetailForXMLParcel, IS_CONSIRET); }
                                else
                                {
                                    Property = ObjMemo.SaveSalesEntry(cn_T, Property, MemoEntryDetailForXML, lblMode.Text, "", MemoEntryDetailForXMLParcel);
                                }
                            }
                            else
                            {
                                Property = ObjMemo.SaveMemoEntry(cn_T, Property, MemoEntryDetailForXML, lblMode.Text, "", MemoEntryDetailForXMLParcel);
                            }
                            txtJangedNo.Text = Property.ReturnValueJanged;
                            lblMemoNo.Text = Property.ReturnValue;
                        }

                        if (Property.ReturnValue == "-1")
                        {
                            ReturnMessageDesc = Property.ReturnMessageDesc;
                            ReturnMessageType = Property.ReturnMessageType;
                            Global.Message(ReturnMessageDesc);
                            return;
                        }
                        else
                        {
                            ReturnMessageDesc = Property.ReturnMessageDesc;
                            ReturnMessageType = Property.ReturnMessageType;
                            ReturnMessageType = "SUCCESS";
                        }

                        transactionScope.Complete();
                        transactionScope.Dispose();
                        cn_T.Close();
                    }

                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        Global.MessageError(ex.Message.ToString());

                    }
                }

                if (ReturnMessageType == "SUCCESS" && (mFormType == FORMTYPE.MEMOISSUE || mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.SALEINVOICE))
                {

                    if (MenuTagName == "SALEINVOICEENTRY")
                    {
                        if (Autosave != true)
                        {
                            Global.Message(ReturnMessageDesc);
                            txtVoucherNoStr.Text = string.Empty;
                            if (lblMode.Text == "Edit Mode")
                                SaleInvoiceMemoID = "00000000-0000-0000-0000-000000000000";
                            BtnClear_Click(null, null);
                            //this.Close();
                        }
                        else
                        {
                            Autosave = false;
                            AutoEntry = false;
                        }
                    }
                    else
                    {
                        Global.Message(ReturnMessageDesc);
                        //this.Close();
                    }
                }
                else if (ReturnMessageType == "SUCCESS")
                {
                    Global.Message(ReturnMessageDesc);
                    //this.Close();
                }
                else
                    Global.MessageError(ReturnMessageDesc);

                DTabMemoStock = ObjMemo.StockGetDataForMemo(Val.ToString(lblMemoNo.Text));

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
                        txtTermsDays_TextChanged(null, null);
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

                GrdDetail.PostEditor();
                DataRow dr = GrdDetail.GetFocusedDataRow();

                if (mFormType == FORMTYPE.PURCHASEISSUE)
                {
                    //dr["PARTYSTOCKNO"] = Val.ToString(dr["STOCKNO"]);
                    GrdDetail.SetFocusedRowCellValue("PARTYSTOCKNO", Val.ToString(dr["STOCKNO"]));
                    return;
                }

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
                    dr["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DrDetail["SALEPRICEPERCARAT"]), 3); //#P : 27-01-2022
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        dr["FMEMOAMOUNT"] = Math.Round((Val.Val(txtExcRate.Text) * Val.Val(DrDetail["SALEAMOUNT"])) / 1000, 3); //#P : 27-01-2022
                    }
                    else
                    {
                        dr["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DrDetail["SALEAMOUNT"]), 3); //#P : 27-01-2022
                    }

                }
                else
                {
                    dr["MEMORAPAPORT"] = DrDetail["MEMORAPAPORT"];
                    dr["MEMODISCOUNT"] = DrDetail["MEMODISCOUNT"];
                    dr["MEMOPRICEPERCARAT"] = DrDetail["MEMOPRICEPERCARAT"];
                    dr["MEMOAMOUNT"] = DrDetail["MEMOAMOUNT"];
                    dr["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DrDetail["MEMOPRICEPERCARAT"]), 3); //#P : 27-01-2022
                    dr["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DrDetail["MEMOAMOUNT"]), 3); //#P : 27-01-2022
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        dr["FMEMOAMOUNT"] = Math.Round((Val.Val(txtExcRate.Text) * Val.Val(DrDetail["MEMOAMOUNT"])) / 1000, 3); //#P : 27-01-2022
                    }
                    else
                    {
                        dr["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DrDetail["MEMOAMOUNT"]), 3); //#P : 27-01-2022
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

                if (Val.ToString(Dr["MAINMEMODETAIL_ID"]) == string.Empty && BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.SALEINVOICE)
                {
                    GrdDetail.Columns["STOCKNO"].OptionsColumn.AllowEdit = true;
                }
                else
                {
                    GrdDetail.Columns["STOCKNO"].OptionsColumn.AllowEdit = false;
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

                //Gunjan:27/02/2027
                DR["FMEMOAMOUNT"] = Math.Round(DouCostAmount * Val.Val(DR["EXCRATE"]), 3);
                DR["FMEMOPRICEPERCARAT"] = Math.Round(DouCostPricePerCarat * Val.Val(DR["EXCRATE"]), 3);
                //End As Gunjan
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
                double DouCostFEAmount = 0;

                if (MenuTagName == "SALEINVOICEENTRY")
                {
                    DataRow DR = GrdSummuryMNL.GetFocusedDataRow();
                    DouCarat = Val.Val(GrdSummuryMNL.GetFocusedRowCellValue("CARAT"));
                    DouCostPricePerCarat = Val.Val(GrdSummuryMNL.EditingValue);

                    DouCostRapaport = Val.Val(GrdSummuryMNL.GetFocusedRowCellValue("MEMORAPAPORT"));

                    if (DouCostRapaport != 0)
                        DouCostDiscount = Math.Round((DouCostPricePerCarat - DouCostRapaport) / DouCostRapaport * 100, 2);
                    else
                        DouCostDiscount = 0;
                    if (Val.ToString(cmbBillType.SelectedItem).ToUpper() == "DOLLARBILL")
                    {
                        DouCostAmount = Math.Round(DouCostPricePerCarat * DouCarat, 2);
                        DouCostFEAmount = Math.Round(DouCostAmount * Val.Val(txtExcRate.Text), 2);
                        DR["FMEMOPRICEPERCARAT"] = Math.Round(DouCostFEAmount / DouCarat, 2);
                    }
                    else if (Val.ToString(cmbBillType.SelectedItem).ToUpper() == "RUPEESBILL")
                    {
                        DouCostFEAmount = Math.Round(DouCostPricePerCarat * DouCarat, 2);
                        if (Val.Val(txtExcRate.Text) != 0)
                        {
                            DouCostAmount = Math.Round(DouCostFEAmount / Val.Val(txtExcRate.Text), 2);
                        }
                        else
                        {
                            DouCostAmount = 0;
                        }

                        DR["MEMOPRICEPERCARAT"] = Math.Round(DouCostAmount / DouCarat, 2);
                    }

                    DR["MEMODISCOUNT"] = DouCostDiscount;
                    DR["MEMOAMOUNT"] = DouCostAmount;
                    DR["FMEMOAMOUNT"] = DouCostFEAmount;
                }
                else
                {

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
                }
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
                    DouMemoTotalAmtFE = DouMemoTotalAmt + Val.Val(DRow["EXCRATE"]);
                }

                txtTotalPcs.Text = Val.Format(IntPcs, "########0");
                txtTotalCarat.Text = Val.Format(DouCarat, "########0.00");

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
                if (Val.Val(txtNetAmount.Text) != 0)
                {
                    txtExcRate.Text = Val.Format((Val.Val(txtNetAmountFE.Text) / Val.Val(txtNetAmount.Text)), "########0.00");
                }
                if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
                {
                    BulkPriceCalculation();
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public void CalculationNew(bool pBool = true) // Add : Pinali : 29-08-2019s
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
                    if (MenuTagName == "SALEINVOICEENTRY")
                        DouSaleAmt = DouSaleAmt + Val.Val(DRow["MEMOAMOUNT"]);
                    else
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

                txtExpInvAmt.Text = Val.Format(DouExpInvAmt, "########0.00");
                txtExpInvAmtFE.Text = Val.Format(DouExpInvAmtFE, "########0.00");

                txtTotalPcs.Text = Val.Format(IntPcs, "########0");
                txtTotalCarat.Text = Val.Format(DouCarat, "########0.00");

                if (DouSaleRapAmt != 0)
                {
                    //lblTotalAvgDisc.Text = Val.Format(((DouSaleAmt - DouSaleRapAmt) / DouSaleRapAmt) * 100, "########0.00"); //#P
                    lblTotalAvgDisc.Text = Val.Format(((DouSaleRapAmt - DouSaleAmt) / DouSaleRapAmt) * 100, "########0.00");
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
                    txtMemoAvgRate.Text = Val.Format(DouSaleAmt / DouCarat, "########0.00");
                }
                else
                {
                    if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
                    //if (BOConfiguration.DEPTNAME == "ACCOUNT")
                    {
                        //txtMemoAmount.Text = Val.Format(Math.Round(DouMemoTotalAmt, 0), "########0.00");
                        //txtGrossAmount.Text = Val.Format(Math.Round(DouMemoTotalAmt, 0), "########0.00");
                        //txtGrossAmountFE.Text = Val.Format(Math.Round(DouMemoTotalAmtFE, 0), "########0.000");  Commnet by shiv 20-06-22
                        //txtMemoAvgRate.Text = Val.Format(Math.Round(DouMemoTotalAmt / DouCarat, 0), "########0.00");
                        txtGrossAmountFE.Text = Val.Format(DouMemoTotalAmtFE, "########0.00");
                        txtMemoAmount.Text = Val.Format(DouMemoTotalAmt, "########0.00");
                        txtGrossAmount.Text = Val.Format(DouMemoTotalAmt, "########0.00");
                        //txtGrossAmountFE.Text = Val.Format(DouMemoTotalAmtFE, "########0.00");
                        txtMemoAvgRate.Text = Val.Format(DouMemoTotalAmt / DouCarat, "########0.00");
                    }
                    else
                    {
                        txtMemoAmount.Text = Val.Format(DouMemoTotalAmt, "########0.00");
                        txtGrossAmount.Text = Val.Format(DouMemoTotalAmt, "########0.00");
                        txtGrossAmountFE.Text = Val.Format(DouMemoTotalAmtFE, "########0.00");
                        txtMemoAvgRate.Text = Val.Format(DouMemoTotalAmt / DouCarat, "########0.00");
                    }
                }

                double DouDiscAmt = Math.Round(Val.Val(txtDiscAmount.Text), 4);
                double DouGSTAmt = Math.Round(Val.Val(txtGSTAmount.Text), 4);
                double DouInsAmt = Math.Round(Val.Val(txtInsuranceAmount.Text), 4);
                double DouShipAmt = Math.Round(Val.Val(txtShippingAmount.Text), 4);
                double DouIGSTAmt = Math.Round(Val.Val(txtIGSTAmount.Text), 4);
                double DouCGSTAmt = Math.Round(Val.Val(txtCGSTAmount.Text), 4);
                double DouSGSTAmt = Math.Round(Val.Val(txtSGSTAmount.Text), 4);

                txtDiscAmount.Text = Val.Format(DouDiscAmt, "########0.00");
                txtInsuranceAmount.Text = Val.Format(DouInsAmt, "########0.00");
                txtShippingAmount.Text = Val.Format(DouShipAmt, "########0.00");
                txtGSTAmount.Text = Val.Format(DouGSTAmt, "########0.00");

                txtIGSTAmount.Text = Val.Format(DouIGSTAmt, "########0.00");
                txtCGSTAmount.Text = Val.Format(DouCGSTAmt, "########0.00");
                txtSGSTAmount.Text = Val.Format(DouSGSTAmt, "########0.00");

                double DouNetAmt = 0;
                if (BOConfiguration.gStrLoginSection == "B")
                {
                    DouNetAmt = Math.Round((Val.Val(txtGrossAmount.Text) + DouIGSTAmt + DouCGSTAmt + DouSGSTAmt + DouInsAmt + DouShipAmt + DouGSTAmt) - DouDiscAmt, 3);
                    txtNetAmount.Text = Val.Format(DouNetAmt, "########0.00");
                }
                else
                {
                    if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
                    //if (BOConfiguration.DEPTNAME == "ACCOUNT")
                    {
                        //DouNetAmt = Math.Round((Val.ToDouble(txtGrossAmount.Text) + DouIGSTAmt + DouCGSTAmt + DouSGSTAmt + DouInsAmt + DouShipAmt + DouGSTAmt) - DouDiscAmt, 0);
                        //txtNetAmount.Text = Val.Format(DouNetAmt, "########0.000");
                        //if (cmbAccType.Text == "Diamond Dollar" || cmbAccType.Text == "Demand" || cmbAccType.Text == "Export")
                        if (cmbAccType.Text == "Export" || cmbAccType.Text == "Net Consignment")
                        {
                            DouNetAmt = Math.Round((Val.ToDouble(txtGrossAmount.Text) + DouIGSTAmt + DouCGSTAmt + DouSGSTAmt + DouInsAmt + DouShipAmt + DouGSTAmt) - DouDiscAmt, 3);
                            txtNetAmount.Text = Val.Format(DouNetAmt, "########0.00");
                        }
                        else
                        {
                            DouNetAmt = Math.Round((Val.Val(txtGrossAmount.Text) + DouInsAmt + DouShipAmt + DouGSTAmt) - DouDiscAmt, 3);
                            txtNetAmount.Text = Val.Format(Math.Round(DouNetAmt, 0), "########0.00");
                        }
                    }
                    else
                    {
                        DouNetAmt = Math.Round((Val.Val(txtGrossAmount.Text) + DouIGSTAmt + DouCGSTAmt + DouSGSTAmt + DouInsAmt + DouShipAmt + DouGSTAmt) - DouDiscAmt, 3);
                        txtNetAmount.Text = Val.Format(DouNetAmt, "########0.00");
                    }
                }

                double DouDiscAmtFE = 0;
                double DouGSTAmtFE = 0;
                double DouInsAmtFE = 0;
                double DouShipAmtFE = 0;

                double DouIGSTAmtFE = 0;
                double DouCGSTAmtFE = 0;
                double DouSGSTAmtFE = 0;


                DouIGSTAmtFE = Math.Round(Val.Val(txtIGSTAmountFE.Text), 3);
                DouCGSTAmtFE = Math.Round(Val.Val(txtCGSTAmountFE.Text), 3);
                DouSGSTAmtFE = Math.Round(Val.Val(txtSGSTAmountFE.Text), 3);

                DouDiscAmtFE = Math.Round(Val.Val(txtDiscAmountFE.Text), 3);
                DouGSTAmtFE = Math.Round(Val.Val(txtGSTAmountFE.Text), 3);
                DouInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouInsAmt, 3);
                DouShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouShipAmt, 3);


                //#K : 08122020
                if (mFormType == FORMTYPE.SALEINVOICE)
                {
                    txtIGSTAmountFE.Text = Val.ToString(Val.Format(DouIGSTAmtFE, "########0.00"));
                    txtCGSTAmountFE.Text = Val.ToString(Val.Format(DouCGSTAmtFE, "########0.00"));
                    txtSGSTAmountFE.Text = Val.ToString(Val.Format(DouSGSTAmtFE, "########0.00"));

                    txtGrossAmountFE.Text = Val.ToString(Val.Format(Val.Val(txtGrossAmountFE.Text), "########0.00"));
                    txtDiscAmountFE.Text = Val.ToString(Val.Format(DouDiscAmtFE, "########0.00"));
                    txtInsuranceAmountFE.Text = Val.ToString(Val.Format(DouInsAmtFE, "########0.00"));
                    txtShippingAmountFE.Text = Val.ToString(Val.Format(DouShipAmtFE, "########0.00"));
                    txtGSTAmountFE.Text = Val.ToString(Val.Format(DouGSTAmtFE, "########0.00"));

                }
                else
                {
                    txtDiscAmountFE.Text = Val.Format(DouDiscAmtFE, "########0.00");
                    txtInsuranceAmountFE.Text = Val.Format(DouInsAmtFE, "########0.00");
                    txtShippingAmountFE.Text = Val.Format(DouShipAmtFE, "########0.00");
                    txtGSTAmountFE.Text = Val.Format(DouGSTAmtFE, "########0.00");

                    txtIGSTAmountFE.Text = Val.Format(DouIGSTAmtFE, "########0.00");
                    txtCGSTAmountFE.Text = Val.Format(DouCGSTAmtFE, "########0.00");
                    txtSGSTAmountFE.Text = Val.Format(DouSGSTAmtFE, "########0.00");
                }


                Double DouNetAmtFE = Math.Round((Val.Val(txtGrossAmountFE.Text) + DouIGSTAmtFE + DouCGSTAmtFE + DouSGSTAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE) - DouDiscAmtFE, 3);


                txtNetAmountFE.Text = Val.Format(Math.Round(DouNetAmtFE, 3), "########0.00");


                txtBrokerAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtNetAmountFE.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 2));
                txtAdatAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtNetAmountFE.Text) * Val.Val(txtAdatPer.Text)) / 100, 2));
                txtAdatAmt.Text = Val.ToString(Math.Round((Val.Val(txtNetAmount.Text) * Val.Val(txtAdatPer.Text)) / 100, 2));
                txtBrokerAmt.Text = Val.ToString(Math.Round((Val.Val(txtNetAmount.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 2));

                txtNetAmountFE.Text = Val.Format(Math.Round((Val.Val(txtNetAmountFE.Text) - Val.Val(txtAdatAmtFE.Text)), 0), "########0.00");
                txtNetAmount.Text = Val.Format(Math.Round((Val.Val(txtNetAmount.Text) - Val.Val(txtAdatAmt.Text)), 0), "########0.00");

                if (Val.Val(txtNetAmount.Text) != 0)
                {
                    txtExcRate.Text = Val.Format((Val.Val(DouNetAmtFE) / Val.Val(DouNetAmt)), "########0.00");
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

                txtTotalPcs.Text = Val.Format(IntPcs, "########0");
                txtTotalCarat.Text = Val.Format(DouCarat, "########0.00");

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
                //txtNetAmount.Text = Val.Format(DouNetAmt, "########0.000");
                txtNetAmount.Text = Val.Format(DouExpInvAmt, "########0.000");

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
                //txtNetAmountFE.Text = Val.Format(DouNetAmtFE, "########0.000");
                txtNetAmountFE.Text = Val.Format(DouExpInvAmtFE, "########0.000");

                txtBrokerAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtNetAmountFE.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 3));
                txtAdatAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtNetAmountFE.Text) * Val.Val(txtAdatPer.Text)) / 100, 3));
                txtAdatAmt.Text = Val.ToString(Math.Round(Val.Val(txtAdatAmtFE.Text) / Val.Val(txtExcRate.Text), 3));
                txtBrokerAmt.Text = Val.ToString(Math.Round(Val.Val(txtBrokerAmtFE.Text) / Val.Val(txtExcRate.Text), 3));

                txtStkAmtFE.Text = Val.Format(Val.Val(txtNetAmountFE.Text) - (Val.Val(txtBrokerAmtFE.Text) + Val.Val(txtAdatAmtFE.Text)), "########0.000");
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
            txtTermsDays.Text = Val.ToString(Val.DateDiff(Microsoft.VisualBasic.DateInterval.Day, DTPMemoDate.Value.ToShortDateString(), DTPTermsDate.Value.ToShortDateString()));
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

                        txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString()), mFormType.ToString()).ToString();

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
                        txtBaseBrokeragePer.Text = Val.ToString(FrmSearch.DRow["BROKERAGEPER"]);
                        if (mFormType == FORMTYPE.ORDERCONFIRM && BOConfiguration.gStrLoginSection != "B")
                        {
                            txtBaseBrokeragePer_Validated(sender, e);
                        }
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
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COMMISSION);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtAdat.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtAdat.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                        txtAdatExcRate.Text = txtExcRate.Text;
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
            //if (MenuTagName == "SALEINVOICEENTRY")
            //{
            this.Cursor = Cursors.WaitCursor;
            txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString()), mFormType.ToString()).ToString();
            txtExcRate_Validated(null, null);
            this.Cursor = Cursors.Default;
            //}
        }


        private void txtExcRate_Validated(object sender, EventArgs e)
        {
            if (MenuTagName == "SALEINVOICEENTRY")
            {

                DTabMemoDetail.AcceptChanges();
                CalculationNewInvoiceEntry();
                AutoGstCalculationNew();
            }
            else
            {
                for (int IntI = 0; IntI < GrdDetail.RowCount; IntI++)
                {
                    DataRow DRow = GrdDetail.GetDataRow(IntI);

                    DRow["EXCRATE"] = Val.Val(txtExcRate.Text);
                    DRow["FMEMOPRICEPERCARAT"] = Val.Val(DRow["EXCRATE"]) * Val.Val(DRow["MEMOPRICEPERCARAT"]);
                    DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["EXCRATE"]) * Val.Val(DRow["MEMOAMOUNT"]), 3);


                }
                DTabMemoDetail.AcceptChanges();
                //Calculation();
                AutoGstCalculation();
                AutoGstCalculationNew();
                CalculationNew();
            }
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
                        GrdDetail.SetFocusedRowCellValue("LOCATIONNAME", Val.ToString(FrmSearch.DRow["LOCATIONCODE"]));
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

                    if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.None)
                    {
                        return;
                    }

                    MemoEntryProperty Property = new MemoEntryProperty();
                    Property.MEMO_ID = Val.ToString(lblMemoNo.Tag);

                    if (Property.MEMO_ID == "")
                    {
                        Property = null;
                        Global.Message("No Memo Found For Delete");
                        return;
                    }

                    DataTable DTab = ObjMemo.ValDelete(Property);
                    if (DTab.Rows.Count != 0 && BOConfiguration.gStrLoginSection != "B")
                    {
                        Global.Message("Some Stones Are In Other Process\n\n You Can Not Delete");
                        FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                        FrmSearch.mStrSearchField = "STOCKNO";
                        FrmSearch.mStrSearchText = "";
                        this.Cursor = Cursors.WaitCursor;
                        FrmSearch.mDTab = DTab;
                        FrmSearch.mStrColumnsToHide = "STOCK_ID";
                        this.Cursor = Cursors.Default;
                        FrmSearch.ShowDialog();
                        FrmSearch.Hide();
                        FrmSearch.Dispose();
                        FrmSearch = null;

                        DTab.Dispose();
                        DTab = null;

                        return;
                    }

                    DTab.Dispose();
                    DTab = null;

                    this.Cursor = Cursors.WaitCursor;

                    Property.PROCESS_ID = Val.ToInt(lblTitle.Tag.ToString());
                    if (txtOrderMemoNo.Tag != null)
                    {
                        Property.ORDERMEMO_ID = Val.ToString(txtOrderMemoNo.Tag.ToString());
                    }


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

                    //Add Khushbu . purchase time par sale and cost ma same amount karva mate 04-02-2022
                    if (mFormType == FORMTYPE.PURCHASEISSUE)
                    {
                        DTabMemoDetail.Rows[e.RowHandle]["MEMORAPAPORT"] = DouRapaport;
                        DTabMemoDetail.Rows[e.RowHandle]["MEMODISCOUNT"] = DouDiscount;
                        DTabMemoDetail.Rows[e.RowHandle]["MEMOPRICEPERCARAT"] = DouPricePerCarat;
                        DTabMemoDetail.Rows[e.RowHandle]["MEMOAMOUNT"] = DouAmount;

                        DTabMemoDetail.Rows[e.RowHandle]["JANGEDRAPAPORT"] = DouRapaport;
                        DTabMemoDetail.Rows[e.RowHandle]["JANGEDDISCOUNT"] = DouDiscount;
                        DTabMemoDetail.Rows[e.RowHandle]["JANGEDPRICEPERCARAT"] = DouPricePerCarat;
                        DTabMemoDetail.Rows[e.RowHandle]["JANGEDAMOUNT"] = DouAmount;

                        DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(DRow["EXCRATE"]), 3);
                        DTabMemoDetail.Rows[e.RowHandle]["FSALEPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(DRow["EXCRATE"]), 3);

                        DTabMemoDetail.Rows[e.RowHandle]["FSALEAMOUNT"] = Math.Round(DouAmount * Val.Val(DRow["EXCRATE"]), 3);
                        DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(DRow["EXCRATE"]), 3);
                    }

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
                        DouDiscount = Math.Round(((DouRapaport + DouPricePerCarat) / DouRapaport) * 100, 2);  //#P:23-04-2021
                    else
                        DouDiscount = 0;

                    DTabMemoDetail.Rows[e.RowHandle]["SALEDISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["SALEAMOUNT"] = DouAmount;

                    // Add Khushbu 04-02-22
                    if (mFormType == FORMTYPE.PURCHASEISSUE)
                    {
                        DTabMemoDetail.Rows[e.RowHandle]["MEMODISCOUNT"] = DouDiscount;
                        DTabMemoDetail.Rows[e.RowHandle]["MEMOAMOUNT"] = DouAmount;

                        DTabMemoDetail.Rows[e.RowHandle]["JANGEDDISCOUNT"] = DouDiscount;
                        DTabMemoDetail.Rows[e.RowHandle]["JANGEDAMOUNT"] = DouAmount;

                        DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(DRow["EXCRATE"]), 3); //#P : 27-01-2022
                        DTabMemoDetail.Rows[e.RowHandle]["FSALEPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(DRow["EXCRATE"]), 3);

                        DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(DRow["EXCRATE"]), 3); //#P : 27-01-2022
                        DTabMemoDetail.Rows[e.RowHandle]["FSALEAMOUNT"] = Math.Round(DouAmount * Val.Val(DRow["EXCRATE"]), 3);

                    }
                    // ----------------- //
                    DTabMemoDetail.AcceptChanges();
                    GrdDetail.PostEditor();
                    break;

                //Gunjan:13/08/2024
                case "MEMOAMOUNT":
                    DouCarat = Val.Val(DRow["CARAT"]);
                    DouRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                    DouAmount = Val.Val(DRow["MEMOAMOUNT"]);

                    DouPricePerCarat = Math.Round(DouAmount / DouCarat, 2);

                    if (DouRapaport != 0)
                        //DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                        DouDiscount = Math.Round(((DouRapaport - DouPricePerCarat) / DouRapaport) * -100, 2);  //#P:23-04-2021
                    else
                        DouDiscount = 0;

                    DTabMemoDetail.Rows[e.RowHandle]["MEMODISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["MEMOPRICEPERCARAT"] = DouPricePerCarat;
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(DRow["EXCRATE"]), 3);
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(DRow["EXCRATE"]), 3);

                    DTabMemoDetail.AcceptChanges();
                    GrdDetail.PostEditor();
                    break;

                case "MEMOPRICEPERCARAT":
                    DouCarat = Val.Val(DRow["CARAT"]);
                    DouRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                    DouPricePerCarat = Val.Val(DRow["MEMOPRICEPERCARAT"]);
                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                    if (DouRapaport != 0)
                        //DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                        DouDiscount = Math.Round(((DouRapaport - DouPricePerCarat) / DouRapaport) * -100, 2);  //#P:23-04-2021
                    else
                        DouDiscount = 0;

                    DTabMemoDetail.Rows[e.RowHandle]["MEMODISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["MEMOAMOUNT"] = DouAmount;
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(DRow["EXCRATE"]), 3);
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(DRow["EXCRATE"]), 3);

                    DTabMemoDetail.AcceptChanges();
                    GrdDetail.PostEditor();
                    break;

                case "MEMODISCOUNT":
                case "MEMORAPAPORT":
                    double MDouCarat = 0;
                    double MDouRapaport = 0;
                    double MDouDiscount = 0;
                    double MDouPricePerCarat = 0;
                    double MDouAmount = 0;

                    MDouCarat = Val.Val(DRow["CARAT"]);
                    MDouRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                    MDouDiscount = Val.Val(DRow["MEMODISCOUNT"]);

                    if (MDouRapaport != 0)
                        //DouPricePerCarat = Math.Round(DouRapaport + ((DouRapaport * DouDiscount) / 100), 2); 
                        MDouPricePerCarat = Math.Round(MDouRapaport + ((MDouRapaport * MDouDiscount) / 100), 2); //#P : 23-04-2021
                    else
                        MDouPricePerCarat = 0;

                    MDouAmount = Math.Round(MDouPricePerCarat * MDouCarat, 2);

                    DTabMemoDetail.Rows[e.RowHandle]["MEMOPRICEPERCARAT"] = MDouPricePerCarat;
                    DTabMemoDetail.Rows[e.RowHandle]["MEMOAMOUNT"] = MDouAmount;
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(MDouPricePerCarat * Val.Val(DRow["EXCRATE"]), 3);
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(MDouAmount * Val.Val(DRow["EXCRATE"]), 3);

                    DTabMemoDetail.AcceptChanges();
                    GrdDetail.PostEditor();
                    MainGrdDetail.Refresh();

                    break;
                //End As Gunjan

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
                        DouPricePerCarat = Math.Round(DouRapaport + ((DouRapaport * DouDiscount) / 100), 2); //#P : 23-04-2021
                    else
                        DouPricePerCarat = 0;

                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                    DTabMemoDetail.Rows[e.RowHandle]["JANGEDRAPAPORT"] = DouRapaport;
                    DTabMemoDetail.Rows[e.RowHandle]["JANGEDPRICEPERCARAT"] = DouPricePerCarat;
                    DTabMemoDetail.Rows[e.RowHandle]["JANGEDAMOUNT"] = DouAmount;

                    DTabMemoDetail.Rows[e.RowHandle]["MEMODISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["MEMOPRICEPERCARAT"] = DouPricePerCarat;
                    DTabMemoDetail.Rows[e.RowHandle]["MEMOAMOUNT"] = DouAmount;

                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(DTabMemoDetail.Rows[e.RowHandle]["EXCRATE"]), 3); //#P : 27-01-2022

                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(DRow["EXCRATE"]), 3); //#P : 27-01-2022

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
                        DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                    //DouDiscount = Math.Round(((DouRapaport + DouPricePerCarat) / DouRapaport) * 100, 2);  //#P:23-04-2021
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
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(DRow["EXCRATE"]), 3); //#P : 27-01-2022

                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(DRow["EXCRATE"]), 3); //#P : 27-01-2022


                    DTabMemoDetail.AcceptChanges();
                    GrdDetail.PostEditor();
                    break;

                case "EXPINVOICERATE":
                    DouCarat = Val.Val(DRow["CARAT"]);
                    DouPricePerCarat = Val.Val(DRow["EXPINVOICERATE"]);
                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                    DTabMemoDetail.Rows[e.RowHandle]["EXPINVOICEAMT"] = DouAmount;
                    DTabMemoDetail.Rows[e.RowHandle]["EXPINVOICEAMTFE"] = Math.Round(DouAmount * Val.Val(DRow["EXCRATE"]), 2);
                    DTabMemoDetail.Rows[e.RowHandle]["EXPINVOICERATEFE"] = Math.Round(Val.Val(DTabMemoDetail.Rows[e.RowHandle]["EXPINVOICEAMTFE"]) / DouCarat, 4);
                    break;

                case "EXCRATE":
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(DRow["EXCRATE"]), 3);
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(DRow["EXCRATE"]), 3);
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
            if (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.SALEINVOICE)
            {
                BulkPriceCalculation();
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
                    DRow["FMEMOAMOUNT"] = Math.Round((Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text)) / 1000, 3); //#P : 27-01-2022
                }
                else
                {
                    DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022
                }
                DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022

            }
            DTabMemoDetail.AcceptChanges();
            //Calculation();
            if (MenuTagName == "SALEINVOICEENTRY")
            {
                CalculationNewInvoiceEntry();
                GetSummuryDetailForGridSaleInvoiceEntry();
            }
            else
            {
                CalculationNew();
                GetSummuryDetailForGrid();
            }

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
                        DRow["FMEMOAMOUNT"] = Math.Round((Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text)) / 1000, 3); //#P : 27-01-2022
                        DRow["FSALEAMOUNT"] = Math.Round((Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text)) / 1000, 3);
                    }
                    else
                    {
                        DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022
                    }
                    DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022
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
                        DRow["FMEMOAMOUNT"] = Math.Round((Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text)) / 1000, 3); //#P : 27-01-2022
                    }
                    else
                    {
                        DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022
                    }
                    DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022

                }
            }
            DTabMemoDetail.AcceptChanges();
            if (MenuTagName == "SALEINVOICEENTRY")
            {
                CalculationNewInvoiceEntry();
                GetSummuryDetailForGridSaleInvoiceEntry();
            }
            else
            {
                CalculationNew();
                GetSummuryDetailForGrid();
            }
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

                    DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022
                    DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022
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
                        DRow["FMEMOAMOUNT"] = Math.Round((Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text)) / 1000, 3); //#P : 27-01-2022
                    }
                    else
                    {
                        DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022
                    }
                    DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022
                }
            }
            DTabMemoDetail.AcceptChanges();
            //Calculation();
            if (MenuTagName == "SALEINVOICEENTRY")
            {
                CalculationNewInvoiceEntry();
                GetSummuryDetailForGridSaleInvoiceEntry();
            }
            else
            {
                CalculationNew();
                GetSummuryDetailForGrid();
            }
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
                if (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.SALEINVOICE && BOConfiguration.gStrLoginSection != "B")
                {
                    GrdDetail.OptionsBehavior.Editable = true;
                    GrdDetail.Columns["STOCKNO"].OptionsColumn.AllowEdit = true;
                    GrdDetail.Columns["PARTYSTOCKNO"].OptionsColumn.AllowEdit = true;
                }
                else
                {
                    GrdDetail.OptionsBehavior.Editable = false;
                    GrdDetail.Columns["STOCKNO"].OptionsColumn.AllowEdit = false;
                    GrdDetail.Columns["PARTYSTOCKNO"].OptionsColumn.AllowEdit = false;
                }
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
                DTabMemoDetail.Rows[pIntRowIndex]["FMEMOPRICEPERCARAT"] = Math.Round(SalePricePerCarat * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022
                if (BOConfiguration.gStrLoginSection == "B")
                {
                    DTabMemoDetail.Rows[pIntRowIndex]["FMEMOAMOUNT"] = Math.Round((SaleAmount * Val.Val(txtExcRate.Text)) / 1000, 3); //#P : 27-01-2022
                }
                else
                {
                    DTabMemoDetail.Rows[pIntRowIndex]["FMEMOAMOUNT"] = Math.Round(SaleAmount * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022
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
                DTabMemoDetail.Rows[pIntRowIndex]["FMEMOPRICEPERCARAT"] = Math.Round(SalePricePerCarat * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022
                if (BOConfiguration.gStrLoginSection == "B")
                {
                    DTabMemoDetail.Rows[pIntRowIndex]["FMEMOAMOUNT"] = Math.Round((SaleAmount * Val.Val(txtExcRate.Text)) / 1000, 3); //#P : 27-01-2022
                }
                else
                {
                    DTabMemoDetail.Rows[pIntRowIndex]["FMEMOAMOUNT"] = Math.Round(SaleAmount * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022
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
                //if (txtBillingParty.Text.Length == 0)
                //{
                //    Global.Message("Billing Party Is Required");
                //    txtBillingParty.Focus();
                //    return;
                //}
                //if (txtBCountry.Text.Length == 0)
                //{
                //    Global.Message("Billing Country Is Required");
                //    txtBCountry.Focus();
                //    return;
                //}
                //if (txtSellerName.Text.Length == 0)
                //{
                //    Global.Message("Seller Name Is Required");
                //    txtSellerName.Focus();
                //    return;
                //}
                //if (txtTerms.Text.Length == 0)
                //{
                //    Global.Message("Terms Is Required");
                //    txtTerms.Focus();
                //    return;
                //}

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

                var SelectedDiamondType = DtReturnStoneList.AsEnumerable().Select(s => s.Field<string>("DIAMONDTYPE")).ToArray();
                string StrDiamondtypeList = string.Join(",", SelectedDiamondType);

                LiveStockProperty LStockProperty = new LiveStockProperty();
                LStockProperty.STOCKNO = Val.ToString(StrPartyStoneNoList);
                LStockProperty.STOCKTYPE = "All";
                LStockProperty.DIAMONDTYPE = Val.ToString(StrDiamondtypeList);
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
                    else if (mFormType == FORMTYPE.HOLD)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.RELEASE, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
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

                Property.TOTALPCS = Val.ToInt(txtTotalPcs.Text);
                Property.TOTALCARAT = Val.Val(txtTotalCarat.Text);
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
                //if (txtBillingParty.Text.Length == 0)
                //{
                //    Global.Message("Billing Party Is Required");
                //    txtBillingParty.Focus();
                //    return;
                //}
                //if (txtShippingParty.Text.Length == 0)
                //{
                //    Global.Message("Shipping Party Is Required");
                //    txtShippingParty.Focus();
                //    return;
                //}
                //if (txtBCountry.Text.Length == 0)
                //{
                //    Global.Message("Billing Country Is Required");
                //    txtBCountry.Focus();
                //    return;
                //}
                //if (txtSCountry.Text.Length == 0)
                //{
                //    Global.Message("Shipping Country Is Required");
                //    txtSCountry.Focus();
                //    return;
                //}
                //if (txtSellerName.Text.Length == 0)
                //{
                //    Global.Message("Seller Name Is Required");
                //    txtSellerName.Focus();
                //    return;
                //}
                //if (txtTerms.Text.Length == 0)
                //{
                //    Global.Message("Terms Is Required");
                //    txtTerms.Focus();
                //    return;
                //}

                //#P : 06-10-2019
                DataTable DtReturnStoneList = GetTableOfSelectedRows(GrdDetail, true);
                if (DtReturnStoneList.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records That You Want To Deliver..");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                var SelectedPartyStoneNo = DtReturnStoneList.AsEnumerable().Select(s => s.Field<string>("PARTYSTOCKNO")).ToArray();
                var SelectDiamondType = DtReturnStoneList.AsEnumerable().Select(s => s.Field<string>("DIAMONDTYPE")).ToArray();
                string StrPartyStoneNoList = string.Join(",", SelectedPartyStoneNo);
                string StrPartyStoneType = SelectDiamondType[0];

                LiveStockProperty LStockProperty = new LiveStockProperty();
                LStockProperty.STOCKNO = Val.ToString(StrPartyStoneNoList);
                LStockProperty.STOCKTYPE = Val.ToString(mStrStockType);
                LStockProperty.DIAMONDTYPE = Val.ToString(StrPartyStoneType);
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
                int TotalRow = 12;
                int NewRow = TotalRow - DTab.Rows.Count;

                for (int i = 0; i < NewRow; i++)
                {
                    DataRow DRNew = DTab.NewRow();
                    DTab.Rows.Add(DRNew);
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
                FrmReportViewer.ShowFormMemoPrint("MemoPrint_USD", DS);
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

                int TotalRow = 12;
                int NewRow = TotalRow - DTab.Rows.Count;

                for (int i = 0; i < NewRow; i++)
                {
                    DataRow DRNew = DTab.NewRow();
                    DTab.Rows.Add(DRNew);
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
                FrmReportViewer.ShowFormMemoPrint("MemoPrint_INR", DS);
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
                //if (new[] {"DOLLARBILL", "EXPORT","CONSIGNMENT"}.Contains(cmbBillType.Text.ToUpper())) //#P : 02-02-2022
                if (new[] { "EXPORT", "CONSIGNMENT", "NONE" }.Contains(cmbBillType.Text.ToUpper()))
                {
                    RbDollar.Checked = true;
                }
                else
                {
                    RbRupee.Checked = true;
                }
                if (MenuTagName == "SALEINVOICEENTRY")
                {
                    if ((mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.PURCHASEISSUE) && !txtVoucherNoStr.Text.Trim().Equals(string.Empty) && Val.ToString(cmbAccType.Text) != StrMainACCType)
                    {
                        Global.MessageError("You Can't Change BillType Coz VoucherNo Already Created Once...Pls Contact with you Administrator..");
                        cmbBillType.Text = StrMainBillType;
                        cmbBillType.Focus();
                        return;
                    }
                }
                else
                {
                    if ((mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.PURCHASEISSUE) && !txtVoucherNoStr.Text.Trim().Equals(string.Empty) && Val.ToString(cmbBillType.Text) != StrMainBillType)
                    {
                        Global.MessageError("You Can't Change BillType Coz VoucherNo Already Created Once...Pls Contact with you Administrator..");
                        cmbBillType.Text = StrMainBillType;
                        cmbBillType.Focus();
                        return;
                    }
                }
                if (cmbBillType.Text.ToUpper() == "EXPORT")
                {

                    lblExpAmt.Visible = true;
                    txtExpInvAmt.Visible = true;
                    txtExpInvAmtFE.Visible = true;

                    if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.PURCHASEISSUE)
                    {
                        GrdDetail.Bands["BANDEXPORT"].Visible = true;
                        ChkUpdExport.Visible = true;

                        GrdDetail.Columns["EXPINVOICERATE"].Visible = true;
                        GrdDetail.Columns["EXPINVOICEAMT"].Visible = true;

                        //#P : 14-07-2021
                        DataRow DRow = new BOMST_Ledger().GetDataByPK("FE4C657D-5452-44D3-84F7-C8C71E20446E"); //BranchCompany : HK
                        if (DRow != null)
                        {
                            txtShippingParty.Text = Val.ToString(DRow["LEDGERNAME"]);
                            txtShippingParty.Tag = Val.ToString(DRow["LEDGER_ID"]);
                            txtSAddress1.Text = Val.ToString(DRow["BILLINGADDRESS1"]);
                            txtSAddress2.Text = Val.ToString(DRow["BILLINGADDRESS2"]);
                            txtSAddress3.Text = Val.ToString(DRow["BILLINGADDRESS3"]);
                            txtSCity.Text = Val.ToString(DRow["BILLINGCITY"]);
                            txtSCountry.Tag = Val.ToString(DRow["BILLINGCOUNTRY_ID"]);
                            txtSCountry.Text = Val.ToString(DRow["BILLINGCOUNTRYNAME"]);
                            txtSState.Text = Val.ToString(DRow["BILLINGSTATE"]);
                            txtSZipCode.Text = Val.ToString(DRow["BILLINGZIPCODE"]);
                        }
                        //End : 14-07-2021
                    }
                }
                else
                {
                    GrdDetail.Bands["BANDEXPORT"].Visible = false;
                    ChkUpdExport.Visible = false;

                    lblExpAmt.Visible = false;
                    txtExpInvAmt.Visible = false;
                    txtExpInvAmtFE.Visible = false;

                    if (cmbBillType.Text.ToString().ToUpper() == "RUPEESBILL" && (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM))
                    {
                        //#P : 14-07-2021
                        DataRow DRow = new BOMST_Ledger().GetDataByPK("EBC650E4-80D7-4651-A2B4-625A4F6E5BE9"); //BranchCompany : Mumbai
                        if (DRow != null)
                        {
                            txtShippingParty.Text = Val.ToString(DRow["LEDGERNAME"]);
                            txtShippingParty.Tag = Val.ToString(DRow["LEDGER_ID"]);
                            txtSAddress1.Text = Val.ToString(DRow["BILLINGADDRESS1"]);
                            txtSAddress2.Text = Val.ToString(DRow["BILLINGADDRESS2"]);
                            txtSAddress3.Text = Val.ToString(DRow["BILLINGADDRESS3"]);
                            txtSCity.Text = Val.ToString(DRow["BILLINGCITY"]);
                            txtSCountry.Tag = Val.ToString(DRow["BILLINGCOUNTRY_ID"]);
                            txtSCountry.Text = Val.ToString(DRow["BILLINGCOUNTRYNAME"]);
                            txtSState.Text = Val.ToString(DRow["BILLINGSTATE"]);
                            txtSZipCode.Text = Val.ToString(DRow["BILLINGZIPCODE"]);
                        }
                        //End : 14-07-2021
                    }


                }


                if (MenuTagName == "SALEINVOICEENTRY")
                {
                    if (cmbBillType.Text.ToUpper() == "RUPEESBILL")
                    {
                        //txtExcRate.Text = "1";
                        txtExcRate.Text = ObjMemo.GetExchangeRate(4, Val.SqlDate(DTPMemoDate.Value.ToShortDateString()), mFormType.ToString()).ToString();
                        txtCurrency.Text = "INR";
                        //txtExcRate.Text = "1";

                        //txtExcRate_Validated(null, null);//Comment By Gunjan:16/11/2024 #as per discussion with brijesh bhai bill tyape change kare tyare exc rate par koi effect nay aave 
                        GrdSummuryMNL.Columns["FMEMOPRICEPERCARAT"].Visible = true;
                        GrdSummuryMNL.Columns["FMEMOAMOUNT"].Visible = true;
                        GrdSummuryMNL.Columns["FMEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                        GrdSummuryMNL.Columns["FMEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                        GrdSummuryMNL.Columns["MEMOPRICEPERCARAT"].Visible = false;
                        GrdSummuryMNL.Columns["MEMOAMOUNT"].Visible = false;
                        GrdSummuryMNL.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                        GrdSummuryMNL.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;

                        //txtGrossAmount.Visible = false;
                        //cLabel47.Visible = false;
                        //txtGrossAmountFE.Visible = true;
                        //lblGrossAmountFESymbol.Visible = true;
                        //txtGrossAmountFE.Location = new Point(85, 5);
                        //lblGrossAmountFESymbol.Location = new Point(205, 11);

                        //txtNetAmount.Visible = false;
                        //cLabel13.Visible = false;
                        //txtNetAmountFE.Visible = true;
                        //lblNetAmountFESymbol.Visible = true;
                        //txtNetAmountFE.Location = new Point(454, 5);
                        //lblNetAmountFESymbol.Location = new Point(570, 11);

                        cLabel35.Visible = false;
                        txtCurrency.Visible = false;
                        //txtExcRate.Visible = false;
                    }
                    else if (cmbBillType.Text.ToUpper() == "DOLLARBILL")
                    {
                        //txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString())).ToString();
                        txtCurrency.Text = "USD";
                        txtExcRate.Text = ObjMemo.GetExchangeRate(1, Val.SqlDate(DTPMemoDate.Value.ToShortDateString()), mFormType.ToString()).ToString();
                        //xtExcRate_Validated(null, null);//Comment By Gunjan:16/11/2024 #as per discussion with brijesh bhai bill tyape change kare tyare exc rate par koi effect nay aave 

                        GrdSummuryMNL.Columns["FMEMOPRICEPERCARAT"].Visible = true;
                        GrdSummuryMNL.Columns["FMEMOAMOUNT"].Visible = true;
                        GrdSummuryMNL.Columns["FMEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = true;

                        //GrdSummuryMNL.Columns["FMEMOPRICEPERCARAT"].Visible = false;
                        //GrdSummuryMNL.Columns["FMEMOAMOUNT"].Visible = false;
                        //GrdSummuryMNL.Columns["FMEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                        GrdSummuryMNL.Columns["FMEMOAMOUNT"].OptionsColumn.AllowEdit = false;
                        GrdSummuryMNL.Columns["MEMOPRICEPERCARAT"].Visible = true;
                        GrdSummuryMNL.Columns["MEMOAMOUNT"].Visible = true;
                        GrdSummuryMNL.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                        GrdSummuryMNL.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;

                        //txtGrossAmount.Visible = true;
                        //cLabel47.Visible = true;
                        //txtGrossAmountFE.Visible = false;
                        //lblGrossAmountFESymbol.Visible = false;
                        ////txtGrossAmountFE.Location = new Point(85, 5);
                        ////lblGrossAmountFESymbol.Location = new Point(193, 11);
                        ///

                        //txtNetAmount.Visible = true;
                        //cLabel13.Visible = true;
                        //txtNetAmountFE.Visible = false;
                        //lblNetAmountFESymbol.Visible = false;
                        ////txtNetAmountFE.Location = new Point(454, 5);
                        ////lblNetAmountFESymbol.Location = new Point(561, 11);

                        cLabel35.Visible = true;
                        txtCurrency.Visible = true;


                    }
                    if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE && MenuTagName == "SALEINVOICEENTRY")
                    {
                        txtCGSTPer_Validated(null, null);
                        txtSGSTPer_Validated(null, null);
                        txtIGSTPer_Validated(null, null);
                        txtTCSPer_Validated(null, null);
                    }
                    CalculationNewInvoiceEntry();
                }

                if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
                //if (BOConfiguration.DEPTNAME == "ACCOUNT")
                {
                    if (BOConfiguration.gStrLoginSection != "B")
                    {
                        if (cmbBillType.Text.ToUpper() == "RUPEESBILL")
                        {
                            GrdSummury.Columns["MEMOAVGRATEFE"].Visible = true;
                            GrdSummury.Columns["MEMOTOTALAMOUNTFE"].Visible = true;

                            GrdSummury.Columns["MEMOAVGRATE"].Visible = false;
                            GrdSummury.Columns["MEMOTOTALAMOUNT"].Visible = false;

                            GrdSummury.Columns["SALEAVGRATE"].Visible = false;
                            GrdSummury.Columns["SALETOTALAMOUNT"].Visible = false;

                            //add shiv 20-06-22
                            GrdSummury.Columns["MEMOTOTALAMOUNTFE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                            GrdSummury.Columns["MEMOTOTALAMOUNTFE"].DisplayFormat.FormatString = "0";

                            GrdSummury.Columns["MEMOAVGRATEFE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                            GrdSummury.Columns["MEMOAVGRATEFE"].DisplayFormat.FormatString = "0";

                            GrdSummury.Columns["MEMOAVGRATEFE"].VisibleIndex = 3;
                            GrdSummury.Columns["MEMOTOTALAMOUNTFE"].VisibleIndex = 4;

                        }
                        else if (cmbBillType.Text.ToUpper() == "DOLLARBILL")
                        {
                            GrdSummury.Columns["MEMOAVGRATE"].Visible = true;
                            GrdSummury.Columns["MEMOTOTALAMOUNT"].Visible = true;

                            GrdSummury.Columns["MEMOAVGRATEFE"].Visible = false;
                            GrdSummury.Columns["MEMOTOTALAMOUNTFE"].Visible = false;

                            GrdSummury.Columns["SALEAVGRATE"].Visible = false;
                            GrdSummury.Columns["SALETOTALAMOUNT"].Visible = false;

                            //add shiv 20-06-22
                            GrdSummury.Columns["MEMOTOTALAMOUNT"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                            GrdSummury.Columns["MEMOTOTALAMOUNT"].DisplayFormat.FormatString = "###0.00";

                            GrdSummury.Columns["MEMOAVGRATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                            GrdSummury.Columns["MEMOAVGRATE"].DisplayFormat.FormatString = "###0.00";

                        }
                        else if (cmbBillType.Text.ToUpper() == "EXPORT")
                        {
                            GrdSummury.Columns["MEMOAVGRATE"].Visible = true;
                            GrdSummury.Columns["MEMOTOTALAMOUNT"].Visible = true;

                            GrdSummury.Columns["MEMOAVGRATEFE"].Visible = false;
                            GrdSummury.Columns["MEMOTOTALAMOUNTFE"].Visible = false;

                            GrdSummury.Columns["SALEAVGRATE"].Visible = false;
                            GrdSummury.Columns["SALETOTALAMOUNT"].Visible = false;

                            //add shiv 20-06-22
                            GrdSummury.Columns["MEMOTOTALAMOUNT"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                            GrdSummury.Columns["MEMOTOTALAMOUNT"].DisplayFormat.FormatString = "###0.00";

                            GrdSummury.Columns["MEMOAVGRATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                            GrdSummury.Columns["MEMOAVGRATE"].DisplayFormat.FormatString = "###0.00";

                        }
                    }
                }

                if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE && cmbBillType.Text.ToUpper() == "EXPORT" && lblMode.Text == "Add Mode" && MenuTagName == "SALEINVOICEENTRY" && IsManualEntryAdd == true)
                { xtraTabPage13.PageVisible = true; StoneDetailParcelTab.PageVisible = true; xtraTabPage15.PageVisible = true; }
                else if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE && cmbBillType.Text.ToUpper() == "EXPORT" && lblMode.Text == "Edit Mode" && MenuTagName == "")
                { xtraTabPage13.PageVisible = true; StoneDetailParcelTab.PageVisible = true; xtraTabPage15.PageVisible = true; }
                else if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE && cmbBillType.Text.ToUpper() == "EXPORT" && lblMode.Text == "Edit Mode" && MenuTagName == "SALEINVOICEENTRY")
                { xtraTabPage13.PageVisible = true; StoneDetailParcelTab.PageVisible = true; xtraTabPage15.PageVisible = true; }
                else if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE && cmbBillType.Text.ToUpper() == "EXPORT" && lblMode.Text == "Add Mode" && MenuTagName == "SALEINVOICEENTRY" && mStrStockType == "ALL")
                { xtraTabPage13.PageVisible = false; StoneDetailParcelTab.PageVisible = false; xtraTabPage15.PageVisible = false; }
                else
                { xtraTabPage13.PageVisible = false; StoneDetailParcelTab.PageVisible = false; xtraTabPage15.PageVisible = false; }

                //Kuldeep 15122020
                //if (cmbBillType.Text.ToUpper() == "RUPEESBILL" || cmbBillType.Text.ToUpper() == "DOLLARBILL")
                //{
                //    txtGSTPer.Text = string.Empty;
                //    txtGSTAmount.Text = string.Empty;
                //    txtInsurancePer.Text = string.Empty;
                //    txtInsuranceAmount.Text = string.Empty;
                //    txtShippingPer.Text = string.Empty;
                //    txtShippingAmount.Text = string.Empty;
                //    txtDiscPer.Text = string.Empty;
                //    txtDiscAmount.Text = string.Empty;

                //    //PnlGSTDetail.Visible = true;
                //    //PnlGSTDetail.BringToFront();
                //    //// AutoGstCalculation();
                //}
                //else
                //{
                //    txtIGSTPer.Text = string.Empty;
                //    txtIGSTAmount.Text = string.Empty;
                //    txtCGSTPer.Text = string.Empty;
                //    txtCGSTAmount.Text = string.Empty;
                //    txtSGSTPer.Text = string.Empty;
                //    txtSGSTAmount.Text = string.Empty;

                //    //PnlGSTDetail.Visible = false;
                //    //PnlGSTDetail.SendToBack();
                //}

                if (cmbBillType.Text == "None")
                    CmbPaymentMode.Text = "None";
                else if (cmbBillType.Text == "Cash")
                    CmbPaymentMode.Text = "Cash";
                else if (cmbBillType.Text == "CashPF")
                    CmbPaymentMode.Text = "Cash/P";
                else
                    CmbPaymentMode.Text = "Bank/WeChat Transfer";

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

        //Commnet By shiv
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
                    //txtCGSTPer.Text = "0.125"; //cmnt Cz Not Consider in order  bill
                    //txtSGSTPer.Text = "0.125"; //cmnt Cz Not Consider in order  bill
                    txtCGSTPer.Text = "0.00";
                    txtSGSTPer.Text = "0.00";
                    txtIGSTPer.Text = "0.00";
                    txtIGSTAmount.Text = "0.00";
                    txtIGSTAmountFE.Text = "0.00";

                    //Comment By Gunjan:15/08/2024 As Per Discussion with dhara madam 
                    //if (mFormType == FORMTYPE.SALEINVOICE)
                    //{
                    //    txtCGSTPer.Text = "0.125"; //cmnt Cz Not Consider in order  bill
                    //    txtSGSTPer.Text = "0.125"; //cmnt Cz Not Consider in order  bill
                    //}
                    //End As Gunjan
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


                    //txtCGSTPer.ReadOnly = false;
                    //txtCGSTAmount.ReadOnly = false;
                    //txtCGSTAmountFE.ReadOnly = false;
                    //txtSGSTPer.ReadOnly = false;
                    //txtSGSTAmount.ReadOnly = false;
                    //txtSGSTAmountFE.ReadOnly = false;

                    //txtIGSTPer.ReadOnly = true;
                    //txtIGSTAmount.ReadOnly = true;
                    //txtIGSTAmountFE.ReadOnly = true;
                }
                else
                {
                    txtCGSTPer.Text = "0.00";
                    txtCGSTAmount.Text = "0.00";
                    txtCGSTAmountFE.Text = "0.00";
                    txtSGSTPer.Text = "0.00";
                    txtSGSTAmount.Text = "0.00";
                    txtSGSTAmountFE.Text = "0.00";
                    //txtIGSTPer.Text = "0.250"; //cmnt Cz Not Consider in order  bill
                    txtIGSTPer.Text = "0.00";

                    //if (mFormType == FORMTYPE.SALEINVOICE)
                    //{
                    //    //txtIGSTPer.Text = "0.250"; //cmnt Cz Not Consider in order  bill
                    //}

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

                    //txtCGSTPer.ReadOnly = true;
                    //txtCGSTAmount.ReadOnly = true;
                    //txtCGSTAmountFE.ReadOnly = true;
                    //txtSGSTPer.ReadOnly = true;
                    //txtSGSTAmount.ReadOnly = true;
                    //txtSGSTAmountFE.ReadOnly = true;

                    //txtIGSTPer.ReadOnly = false;
                    //txtIGSTAmount.ReadOnly = false;
                    // txtIGSTAmountFE.ReadOnly = false;
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
                    //txtCGSTAmount.Text = Math.Round((DouGrossAmt * Val.Val(txtCGSTPer.Text)) / 100, 3).ToString();
                    //txtCGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtCGSTAmount.Text) * Val.ToDouble(txtExcRate.Text), 3), "########0.00");
                    if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
                    {
                        txtCGSTAmount.Text = Math.Round((DouGrossAmt * Val.Val(txtCGSTPer.Text)) / 100, 3).ToString();
                        txtCGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtCGSTAmount.Text) * Val.ToDouble(txtExcRate.Text), 0), "########0.000");
                    }
                    else
                    {
                        txtCGSTAmount.Text = Math.Round((DouGrossAmt * Val.Val(txtCGSTPer.Text)) / 100, 3).ToString();
                        txtCGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtCGSTAmount.Text) * Val.ToDouble(txtExcRate.Text), 3), "########0.000");
                    }
                }
                else
                {
                    //txtCGSTAmountFE.Text = Math.Round((DouGrossAmtFe * Val.Val(txtCGSTPer.Text)) / 100, 0).ToString();
                    //txtCGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtCGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.00");
                    if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
                    {
                        txtCGSTAmountFE.Text = Math.Round((DouGrossAmtFe * Val.Val(txtCGSTPer.Text)) / 100, 0).ToString();
                        txtCGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtCGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                    else
                    {
                        txtCGSTAmountFE.Text = Math.Round((DouGrossAmtFe * Val.Val(txtCGSTPer.Text)) / 100, 3).ToString();
                        txtCGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtCGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                }
            }
            else
            {
                txtCGSTPer.Text = "0.000";
                txtCGSTAmount.Text = "0.000";
                txtCGSTAmountFE.Text = "0.000";
            }
            if (MenuTagName == "SALEINVOICEENTRY")
                CalculationNewInvoiceEntry();
            else
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
                        txtCGSTPer.Text = "0.000";
                        txtCGSTAmount.Text = "0.000";
                        txtCGSTAmountFE.Text = "0.000";
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                        CalculationNewInvoiceEntry();
                    else
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
                        txtCGSTPer.Text = Math.Round((Val.Val(txtCGSTAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                        txtCGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtCGSTAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                    else
                    {
                        txtCGSTPer.Text = "0.000";
                        txtCGSTAmount.Text = "0.000";
                        txtCGSTAmountFE.Text = "0.000";
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                        CalculationNewInvoiceEntry();
                    else
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
                    if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
                    {
                        txtSGSTAmount.Text = Math.Round((DouGrossAmt * Val.Val(txtSGSTPer.Text)) / 100, 3).ToString();
                        txtSGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtSGSTAmount.Text) * Val.Val(txtExcRate.Text), 0), "########0.000");
                    }
                    else
                    {
                        txtSGSTAmount.Text = Math.Round((DouGrossAmt * Val.Val(txtSGSTPer.Text)) / 100, 3).ToString();
                        txtSGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtSGSTAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                }
                else
                {
                    if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
                    {
                        txtSGSTAmountFE.Text = Math.Round((DouGrossAmtFe * Val.Val(txtSGSTPer.Text)) / 100, 0).ToString();
                        txtSGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtSGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                    else
                    {
                        txtSGSTAmountFE.Text = Math.Round((DouGrossAmtFe * Val.Val(txtSGSTPer.Text)) / 100, 3).ToString();
                        txtSGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtSGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                }
            }
            else
            {
                txtSGSTPer.Text = "0.000";
                txtSGSTAmount.Text = "0.000";
                txtSGSTAmountFE.Text = "0.000";
            }

            if (MenuTagName == "SALEINVOICEENTRY")
                CalculationNewInvoiceEntry();
            else
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
                        txtSGSTPer.Text = Math.Round((Val.Val(txtSGSTAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                        txtSGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtSGSTAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                    else
                    {
                        txtSGSTPer.Text = "0.000";
                        txtSGSTAmount.Text = "0.000";
                        txtSGSTAmountFE.Text = "0.000";
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                        CalculationNewInvoiceEntry();
                    else
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
                        txtSGSTPer.Text = "0.000";
                        txtSGSTAmount.Text = "0.000";
                        txtSGSTAmountFE.Text = "0.000";
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                        CalculationNewInvoiceEntry();
                    else
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


            double DouDiscAmtFE = Math.Round(Val.Val(txtDiscAmountFE.Text), 3);
            double DouGSTAmtFE = Math.Round(Val.Val(txtGSTAmountFE.Text), 3);
            double DouInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouInsAmt, 3);
            double DouShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouShipAmt, 3);

            double DouGrossAmt = (Val.Val(txtGrossAmount.Text) + DouGSTAmt + DouInsAmt + DouShipAmt) - DouDiscAmt;
            double DouGrossAmtFe = Val.Val(txtGrossAmountFE.Text);

            if (Val.Val(txtGrossAmount.Text) != 0)
            {
                if (txtCurrency.Text == "USD")
                {
                    if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
                    {
                        txtIGSTAmount.Text = Math.Round((DouGrossAmt * Val.Val(txtIGSTPer.Text)) / 100, 3).ToString();
                        txtIGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtIGSTAmount.Text) * Val.Val(txtExcRate.Text), 0), "########0.000");
                    }
                    else
                    {
                        txtIGSTAmount.Text = Math.Round((DouGrossAmt * Val.Val(txtIGSTPer.Text)) / 100, 3).ToString();
                        txtIGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtIGSTAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                }
                else
                {
                    if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
                    {
                        txtIGSTAmountFE.Text = Math.Round((DouGrossAmtFe * Val.Val(txtIGSTPer.Text)) / 100, 0).ToString();
                        txtIGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtIGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                    else
                    {
                        txtIGSTAmountFE.Text = Math.Round((DouGrossAmtFe * Val.Val(txtIGSTPer.Text)) / 100, 3).ToString();
                        txtIGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtIGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                }
            }
            else
            {
                txtIGSTPer.Text = "0.000";
                txtIGSTAmount.Text = "0.000";
                txtIGSTAmountFE.Text = "0.000";
            }

            if (MenuTagName == "SALEINVOICEENTRY")
                CalculationNewInvoiceEntry();
            else
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
                        txtIGSTPer.Text = "0.000";
                        txtIGSTAmount.Text = "0.000";
                        txtIGSTAmountFE.Text = "0.000";
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                        CalculationNewInvoiceEntry();
                    else
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
                        txtIGSTPer.Text = "0.000";
                        txtIGSTAmount.Text = "0.000";
                        txtIGSTAmountFE.Text = "0.000";
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                        CalculationNewInvoiceEntry();
                    else
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
            if (cmbBillType.Text.ToUpper() != "DOLLARBILL")
            {
                txtTCSPer.Text = "0.10";
            }
            if (Val.Val(txtTCSCalcAmt.Text) != 0)
            {
                if (Val.Val(txtGrossAmount.Text) != 0)
                {
                    if (txtCurrency.Text == "USD")
                    {
                        if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
                        {
                            txtTCSAmount.Text = Math.Round((Val.Val(txtTCSCalcAmt.Text) * Val.Val(txtTCSPer.Text)) / 100, 3).ToString();
                            txtTCSAmountFE.Text = Val.Format(Math.Round(Val.Val(txtTCSAmount.Text) * Val.Val(txtExcRate.Text), 0), "########0.000");
                        }
                        else
                        {
                            txtTCSAmount.Text = Math.Round((Val.Val(txtTCSCalcAmt.Text) * Val.Val(txtTCSPer.Text)) / 100, 3).ToString();
                            txtTCSAmountFE.Text = Val.Format(Math.Round(Val.Val(txtTCSAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                        }
                    }
                    else
                    {
                        if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
                        {
                            //txtTCSAmount.Text = Val.Format(Math.Round(Val.Val(txtTCSCalcAmt.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                            //txtTCSAmountFE.Text = Math.Round((Val.Val(txtTCSAmount.Text) * Val.Val(txtTCSPer.Text)) / 100, 0).ToString();
                            //txtTCSAmount.Text = Val.Format(Math.Round(Val.Val(txtTCSAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                            txtTCSAmount.Text = Math.Round((Val.Val(txtTCSCalcAmt.Text) * Val.Val(txtTCSPer.Text)) / 100, 3).ToString();
                            txtTCSAmountFE.Text = Val.Format(Math.Round(Val.Val(txtTCSAmount.Text) * Val.Val(txtExcRate.Text), 0), "########0.000");

                        }
                        else

                        {
                            //txtTCSAmountFE.Text = Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtTCSPer.Text)) / 100, 3).ToString();
                            //txtTCSAmount.Text = Val.Format(Math.Round(Val.Val(txtTCSAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");

                            txtTCSAmount.Text = Math.Round((Val.Val(txtTCSCalcAmt.Text) * Val.Val(txtTCSPer.Text)) / 100, 3).ToString();
                            txtTCSAmountFE.Text = Val.Format(Math.Round(Val.Val(txtTCSAmount.Text) * Val.Val(txtExcRate.Text), 0), "########0.000");
                        }
                    }
                }
                else
                {
                    txtTCSPer.Text = "0.000";
                    txtTCSAmount.Text = "0.000";
                    txtTCSAmountFE.Text = "0.000";
                }

                if (MenuTagName == "SALEINVOICEENTRY")
                    CalculationNewInvoiceEntry();
                else
                    CalculationNew();
            }
        }

        private void txtTCSAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtTCSAmount.Focused)
            {
                try
                {
                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        txtTCSPer.Text = Math.Round((Val.Val(txtTCSAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                        txtTCSAmountFE.Text = Val.Format(Math.Round(Val.Val(txtTCSAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                    else
                    {
                        txtTCSPer.Text = "0.000";
                        txtTCSAmount.Text = "0.000";
                        txtTCSAmountFE.Text = "0.000";
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                        CalculationNewInvoiceEntry();
                    else
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
            //if (txtTCSAmountFE.Focused)
            //{
            //    try
            //    {
            //        if (Val.Val(txtGrossAmountFE.Text) != 0)
            //        {
            //            double DouIGSTAmount = 0;
            //            DouIGSTAmount = Math.Round(Val.Val(txtTCSAmountFE.Text) / Val.Val(txtExcRate.Text), 3);


            //            txtTCSAmount.Text = Val.Format(DouIGSTAmount, "########0.000");
            //            txtTCSPer.Text = Math.Round((Val.Val(DouIGSTAmount) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
            //        }
            //        else
            //        {
            //            txtTCSPer.Text = "0.000";
            //            txtTCSAmount.Text = "0.000";
            //            txtTCSAmountFE.Text = "0.000";
            //        }
            if (MenuTagName == "SALEINVOICEENTRY")
                CalculationNewInvoiceEntry();
            else
                CalculationNew();
            //    }
            //    catch (Exception ex)
            //    {
            //        Global.Message(ex.Message.ToString());
            //    }
            //}
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
                txtGSTPer.Text = "0.000";
                txtGSTAmount.Text = "0.000";
                txtGSTAmountFE.Text = "0.000";
            }
            //End : #P : 25-12-2019
            txtCGSTPer_Validated(sender, e);
            txtSGSTPer_Validated(sender, e);
            txtIGSTPer_Validated(sender, e);

            if (MenuTagName == "SALEINVOICEENTRY")
            {
                CalculationNewInvoiceEntry();
                GetSummuryDetailForGridSaleInvoiceEntry();
            }
            else
            {
                CalculationNew();
                GetSummuryDetailForGrid();
            }

        }

        private void txtGSTAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtGSTAmount.Focused)
                {
                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        txtGSTPer.Text = Math.Round((Val.Val(txtGSTAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                        txtGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtGSTAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                    else
                    {
                        txtGSTPer.Text = "0.000";
                        txtGSTAmount.Text = "0.000";
                        txtGSTAmountFE.Text = "0.000";
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                    {
                        CalculationNewInvoiceEntry();
                        GetSummuryDetailForGridSaleInvoiceEntry();
                    }
                    else
                    {
                        CalculationNew();
                        GetSummuryDetailForGrid();
                    }

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
                        txtGSTPer.Text = "0.000";
                        txtGSTAmount.Text = "0.000";
                        txtGSTAmountFE.Text = "0.000";
                    }

                    if (MenuTagName == "SALEINVOICEENTRY")
                    {
                        CalculationNewInvoiceEntry();
                        GetSummuryDetailForGridSaleInvoiceEntry();
                    }
                    else
                    {
                        CalculationNew();
                        GetSummuryDetailForGrid();
                    }

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
                    txtInsuranceAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtInsurancePer.Text)) / 100, 3).ToString();
                    txtInsuranceAmountFE.Text = Val.Format(Math.Round(Val.Val(txtInsuranceAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                }
                else
                {
                    txtInsuranceAmountFE.Text = Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtInsurancePer.Text)) / 100, 3).ToString();
                    txtInsuranceAmount.Text = Val.Format(Math.Round(Val.Val(txtInsuranceAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                }
            }
            else
            {
                txtInsurancePer.Text = "0.000";
                txtInsuranceAmount.Text = "0.000";
                txtInsuranceAmountFE.Text = "0.000";
            }
            //End : #P : 25-12-2019
            txtCGSTPer_Validated(sender, e);
            txtSGSTPer_Validated(sender, e);
            txtIGSTPer_Validated(sender, e);
            if (MenuTagName == "SALEINVOICEENTRY")
            {
                CalculationNewInvoiceEntry();
                GetSummuryDetailForGridSaleInvoiceEntry();
            }
            else
            {
                CalculationNew();
                GetSummuryDetailForGrid();
            }

        }

        private void txtInsuranceAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtInsuranceAmountFE.Focused)
                {
                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        txtInsurancePer.Text = Math.Round((Val.Val(txtInsuranceAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                        txtInsuranceAmountFE.Text = Val.Format(Math.Round(Val.Val(txtInsuranceAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                    else
                    {
                        txtInsurancePer.Text = "0.000";
                        txtInsuranceAmount.Text = "0.000";
                        txtInsuranceAmountFE.Text = "0.000";
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                    {
                        CalculationNewInvoiceEntry();
                        GetSummuryDetailForGridSaleInvoiceEntry();
                    }
                    else
                    {
                        CalculationNew();
                        GetSummuryDetailForGrid();
                    }

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
                        DouInsAmount = Math.Round(Val.Val(txtInsuranceAmountFE.Text) / Val.Val(txtExcRate.Text), 3);

                        txtInsuranceAmount.Text = Val.Format(DouInsAmount, "########0.000");
                        txtInsurancePer.Text = Math.Round((Val.Val(DouInsAmount) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                    }
                    else
                    {
                        txtInsurancePer.Text = "0.000";
                        txtInsuranceAmount.Text = "0.000";
                        txtInsuranceAmountFE.Text = "0.000";
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                    {
                        CalculationNewInvoiceEntry();
                        GetSummuryDetailForGridSaleInvoiceEntry();
                    }
                    else
                    {
                        CalculationNew();
                        GetSummuryDetailForGrid();
                    }

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
                    txtShippingAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtShippingPer.Text)) / 100, 3).ToString();
                    txtShippingAmountFE.Text = Val.Format(Math.Round(Val.Val(txtShippingAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                }
                else
                {
                    txtShippingAmountFE.Text = Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtShippingPer.Text)) / 100, 3).ToString();
                    txtShippingAmount.Text = Val.Format(Math.Round(Val.Val(txtShippingAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                }
            }
            else
            {
                txtShippingPer.Text = "0.000";
                txtShippingAmount.Text = "0.000";
                txtShippingAmountFE.Text = "0.000";
            }
            //End : #P : 25-12-2019
            txtCGSTPer_Validated(sender, e);
            txtSGSTPer_Validated(sender, e);
            txtIGSTPer_Validated(sender, e);
            if (MenuTagName == "SALEINVOICEENTRY")
            {
                CalculationNewInvoiceEntry();
                GetSummuryDetailForGridSaleInvoiceEntry();
            }
            else
            {
                CalculationNew();
                GetSummuryDetailForGrid();
            }

        }

        private void txtShippingAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtShippingAmountFE_TextChanged(object sender, EventArgs e)
        {

        }


        private void txtDiscPer_Validated(object sender, EventArgs e)
        {
            //#P : 25-12-2019
            if (Val.Val(txtGrossAmount.Text) != 0)
            {
                if (txtCurrency.Text == "USD")
                {
                    txtDiscAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtDiscPer.Text)) / 100, 3).ToString();
                    txtDiscAmountFE.Text = Val.Format(Math.Round(Val.Val(txtDiscAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                }
                else
                {
                    txtDiscAmountFE.Text = Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtDiscPer.Text)) / 100, 3).ToString();
                    txtDiscAmount.Text = Val.Format(Math.Round(Val.Val(txtDiscAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                }
            }
            else
            {
                txtDiscPer.Text = "0.000";
                txtDiscAmount.Text = "0.000";
                txtDiscAmountFE.Text = "0.000";
            }
            //End : #P : 25-12-2019
            txtCGSTPer_Validated(sender, e);
            txtSGSTPer_Validated(sender, e);
            txtIGSTPer_Validated(sender, e);
            if (MenuTagName == "SALEINVOICEENTRY")
            {
                CalculationNewInvoiceEntry();
                GetSummuryDetailForGridSaleInvoiceEntry();
            }
            else
            {
                CalculationNew();
                GetSummuryDetailForGrid();
            }

        }

        private void txtDiscAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtGrossAmountFE.Focused)
                {
                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        txtDiscPer.Text = Math.Round((Val.Val(txtDiscAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                        txtDiscAmountFE.Text = Val.Format(Math.Round(Val.Val(txtDiscAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                    else
                    {
                        txtDiscPer.Text = "0.000";
                        txtDiscAmount.Text = "0.000";
                        txtDiscAmountFE.Text = "0.000";
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                    {
                        CalculationNewInvoiceEntry();
                        GetSummuryDetailForGridSaleInvoiceEntry();
                    }
                    else
                    {
                        CalculationNew();
                        GetSummuryDetailForGrid();
                    }

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

                        DouDiscountAmount = Math.Round(Val.Val(txtDiscAmountFE.Text) / Val.Val(txtExcRate.Text), 3);
                        txtDiscAmount.Text = Val.Format(DouDiscountAmount, "########0.000");
                        txtDiscPer.Text = Math.Round((Val.Val(DouDiscountAmount) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                    }
                    else
                    {
                        txtDiscPer.Text = "0.000";
                    }

                    if (MenuTagName == "SALEINVOICEENTRY")
                    {
                        CalculationNewInvoiceEntry();
                        GetSummuryDetailForGridSaleInvoiceEntry();
                    }
                    else
                    {
                        CalculationNew();
                        GetSummuryDetailForGrid();
                    }

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }


        #region Account


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
            DtAccountingEffect.Columns.Add("ENTRYTRANTYPE");
            DtAccountingEffect.Columns.Add("ISAUTOACCENTRY");
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
            DtExportAccountingEffect.Columns.Add("ENTRYTRANTYPE");
            DtExportAccountingEffect.Columns.Add("ISAUTOACCENTRY");
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


        #endregion

        private void txtBaseBrokeragePer_Validated(object sender, EventArgs e)
        {
            IsChangeBrokPer = true;
            if (MenuTagName == "SALEINVOICEENTRY")
                CalculationNewInvoiceEntry();
            else
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

        //public string EncryptAsymmetric(string data, string key)
        //{
        //    var keyBytes = Convert.FromBase64String(key);
        //    AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
        //    RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
        //    var rsaParameters = new RSAParameters();
        //    rsaParameters.Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned();
        //    rsaParameters.Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned();
        //    var rsa = new RSACryptoServiceProvider();
        //    rsa.ImportParameters(rsaParameters);
        //    var plaintext = Encoding.UTF8.GetBytes(data);
        //    byte[] ciphertext = rsa.Encrypt(plaintext, false);
        //    string cipherresult = Convert.ToBase64String(ciphertext);
        //    return cipherresult;
        //}

        //public string Encrypt(byte[] data, string key)
        //{
        //    var keyBytes = Convert.FromBase64String(key);
        //    AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
        //    RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
        //    var rsaParameters = new RSAParameters();
        //    rsaParameters.Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned();
        //    rsaParameters.Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned();
        //    var rsa = new RSACryptoServiceProvider();
        //    rsa.ImportParameters(rsaParameters);
        //    var plaintext = data;
        //    byte[] ciphertext = rsa.Encrypt(plaintext, false);
        //    string cipherresult = Convert.ToBase64String(ciphertext);
        //    return cipherresult;
        //}


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
                double DouTotalAmountFE = 0.000;
                double DouTotalAvgDiscFE = 0.00;
                double DouTotalRateFE = 0.00;

                for (int IntI = 0; IntI < GrdDetail.RowCount; IntI++)
                {
                    DataRow DRow = GrdDetail.GetDataRow(IntI);

                    DouTotalPCS = DouTotalPCS + Val.ToInt(DRow["PCS"]);
                    DouTotalCarat = Math.Round((DouTotalCarat + Val.Val(DRow["CARAT"])), 2);

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

                DouTotalRateFE = Math.Round((DouTotalAmountFE / DouTotalCarat), 3);
                DouTotalAvgDiscFE = Math.Round(Val.Val(((DouTotalAmountFE - DouTotalRapaport) / DouTotalRapaport) * 100), 3);
                DouTotalMemoRateFE = Math.Round((DouTotalMemoAmountFE / DouTotalCarat), 3);
                DouTotalMemoAvgDiscFE = Math.Round(Val.Val(((DouTotalMemoAmountFE - DouTotalRapaport) / DouTotalRapaport) * 100), 3);

                if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE)
                {
                    if (cmbAccType.SelectedIndex == 2 || cmbAccType.SelectedIndex == 3) //add shiv 22-06-2022
                    {
                        DouTotalMemoAmountFE = Math.Round(DouTotalMemoAmountFE, 3);
                        DouTotalMemoRateFE = Math.Round((DouTotalMemoAmountFE / DouTotalCarat), 3);

                        DouTotalMemoAmount = Math.Round(DouTotalMemoRate, 2) * DouTotalCarat;
                        DouTotalMemoRate = Math.Round(DouTotalMemoRate, 2);
                    }
                    else if (cmbAccType.SelectedIndex == 4)
                    {
                        DouTotalMemoAmount = Math.Round(DouTotalMemoRate, 2) * DouTotalCarat;
                        DouTotalMemoRate = Math.Round(DouTotalMemoRate, 2);
                    }
                    else
                    {
                        DouTotalMemoAmountFE = Math.Round(DouTotalMemoRateFE, 0) * DouTotalCarat;
                        DouTotalMemoAmountFE = Math.Round(DouTotalMemoAmountFE, 0);
                        DouTotalMemoRateFE = Math.Round((DouTotalMemoAmountFE / DouTotalCarat), 2);
                        DouTotalMemoRateFE = Math.Round(DouTotalMemoRateFE, 2);
                    }
                }
                else
                {
                    DouTotalMemoAmountFE = Math.Round(DouTotalMemoAmountFE, 3);
                    DouTotalMemoRateFE = Math.Round((DouTotalMemoAmountFE / DouTotalCarat), 3);
                }

                StrDescription = "Cut & Polished Diamond";

                if (DTabMemoSummury.Rows.Count > 0)
                {
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
                    DTabMemoSummury.Rows[0]["MEMOTOTALAMOUNTFE"] = Val.Format(DouTotalMemoAmountFE, "########0.000");
                    DTabMemoSummury.Rows[0]["MEMOAVGDISCFE"] = DouTotalMemoAvgDiscFE;

                    if (cmbAccType.SelectedText == "Local Sales" && cmbBillType.SelectedText == "RupeesBill")
                    {
                        GrdSummury.Columns["MEMOTOTALAMOUNTFE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                        GrdSummury.Columns["MEMOTOTALAMOUNTFE"].DisplayFormat.FormatString = "0";

                        GrdSummury.Columns["MEMOAVGRATEFE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                        GrdSummury.Columns["MEMOAVGRATEFE"].DisplayFormat.FormatString = "###0.00";
                    }
                    else
                    {
                        GrdSummury.Columns["MEMOTOTALAMOUNT"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                        GrdSummury.Columns["MEMOTOTALAMOUNT"].DisplayFormat.FormatString = "###0.00";

                        GrdSummury.Columns["MEMOAVGRATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                        GrdSummury.Columns["MEMOAVGRATE"].DisplayFormat.FormatString = "###0.00";
                    }
                }


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
                if (MenuTagName == "SALEINVOICEENTRY")
                {
                    CalculationNewInvoiceEntry();
                    GetSummuryDetailForGridSaleInvoiceEntry();
                }
                else
                {
                    GrdDetail_CellValueChanged(null, null);
                    //CalculationNew();
                    //GetSummuryDetailForGrid();
                }
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

        private void txtTermsPer_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //if (RdbParameter.Checked)
                //{
                //    Global.Message("Please Select 'All' Or 'Price' Checkbox For Update Stone Price.");
                //    return;
                //}

                double DouSaleDiscount = 0;
                double DouSaleRapaport = 0;
                double DouSalePricePerCarat = 0;
                double DouSaleAmount = 0;
                double OldCarat = 0;

                foreach (DataRow DRow in DTabMemoDetail.Rows)
                {
                    if (Val.Val(txtTermsAddLessPer.Text) != 0)
                    {
                        DouSalePricePerCarat = Val.Val(DRow["MEMOPRICEPERCARAT"]) + Math.Round(((Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtTermsAddLessPer.Text)) / 100), 2);
                    }
                    else if (Val.Val(txtBlindAddLessPer.Text) != 0)
                    {
                        DouSalePricePerCarat = Val.Val(DRow["MEMOPRICEPERCARAT"]) + Math.Round(((Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtBlindAddLessPer.Text)) / 100), 2);
                    }
                    else if (Val.Val(txtTermsAddLessPer.Text) != 0 && Val.Val(txtBlindAddLessPer.Text) != 0)
                    {
                        DouSalePricePerCarat = Val.Val(DRow["MEMOPRICEPERCARAT"]) + Math.Round(((Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtTermsAddLessPer.Text)) / 100), 2);
                        DouSalePricePerCarat = DouSalePricePerCarat - Math.Round(((Val.Val(DouSalePricePerCarat) * Val.Val(txtBlindAddLessPer.Text)) / 100), 2);
                    }
                    else
                    {
                        DouSalePricePerCarat = Val.Val(DRow["SALEPRICEPERCARAT"]);
                    }

                    if (Val.Val(DRow["MEMORAPAPORT"]) != 0)
                        DouSaleDiscount = Math.Round(((Val.Val(DouSalePricePerCarat) - Val.Val(DRow["MEMORAPAPORT"])) / Val.Val(DRow["MEMORAPAPORT"])) * 100, 2);
                    else
                        DouSaleDiscount = 0;

                    DouSaleRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                    DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                    DRow["MEMODISCOUNT"] = DouSaleDiscount;
                    DRow["MEMOPRICEPERCARAT"] = DouSalePricePerCarat;
                    DRow["FMEMOPRICEPERCARAT"] = DouSalePricePerCarat * Val.Val(DRow["EXCRATE"]);
                    DRow["MEMOAMOUNT"] = DouSaleAmount;
                    DRow["FMEMOAMOUNT"] = DouSaleAmount * Val.Val(DRow["EXCRATE"]);
                }
                DTabMemoDetail.AcceptChanges();
                CalculationNew();
                GetSummuryDetailForGrid();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
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

                double DouExcRate = 0;

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
                                    DouDiscount = Val.Val(DtabBackPriceUpdate.Rows[i]["Disc"]);

                                    if (DouRapaport != 0)
                                        DouPricePerCarat = Math.Round(DouRapaport + ((DouRapaport * DouDiscount) / 100), 2);
                                    else
                                        DouPricePerCarat = 0;

                                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                                    DTabMemoDetail.Rows[j]["JANGEDRAPAPORT"] = DouRapaport;
                                    DTabMemoDetail.Rows[j]["JANGEDPRICEPERCARAT"] = DouPricePerCarat;
                                    DTabMemoDetail.Rows[j]["JANGEDAMOUNT"] = DouAmount;

                                    DTabMemoDetail.Rows[j]["MEMODISCOUNT"] = DouDiscount;
                                    DTabMemoDetail.Rows[j]["MEMOPRICEPERCARAT"] = DouPricePerCarat;
                                    DTabMemoDetail.Rows[j]["MEMOAMOUNT"] = DouAmount;

                                    DTabMemoDetail.Rows[j]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(DTabMemoDetail.Rows[j]["EXCRATE"]), 3); //#P : 27-01-2022
                                    //DTabMemoDetail.Rows[j]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 2);
                                    if (BOConfiguration.gStrLoginSection == "B")
                                    {
                                        DTabMemoDetail.Rows[j]["FMEMOAMOUNT"] = Math.Round((DouAmount * Val.Val(DTabMemoDetail.Rows[j]["EXCRATE"])) / 1000, 3); //#P : 27-01-2022
                                    }
                                    else
                                    {
                                        DTabMemoDetail.Rows[j]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(DTabMemoDetail.Rows[j]["EXCRATE"]), 3); //#P : 27-01-2022
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
                                        DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                                    //DouDiscount = Math.Round(((DouRapaport + DouPricePerCarat) / DouRapaport) * 100, 2); 
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
                                    DTabMemoDetail.Rows[j]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(DTabMemoDetail.Rows[j]["EXCRATE"]), 3); //#P : 27-01-2022
                                    //DTabMemoDetail.Rows[j]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 2);
                                    if (BOConfiguration.gStrLoginSection == "B")
                                    {
                                        DTabMemoDetail.Rows[j]["FMEMOAMOUNT"] = Math.Round((DouAmount * Val.Val(DTabMemoDetail.Rows[j]["EXCRATE"])) / 1000, 3); //#P : 27-01-2022
                                    }
                                    else
                                    {
                                        DTabMemoDetail.Rows[j]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(DTabMemoDetail.Rows[j]["EXCRATE"]), 3); //#P : 27-01-2022
                                    }

                                    DTabMemoDetail.AcceptChanges();
                                }
                                break;
                            }
                        }
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                    {
                        CalculationNewInvoiceEntry();
                        GetSummuryDetailForGridSaleInvoiceEntry();
                    }
                    else
                    {
                        CalculationNew();
                        GetSummuryDetailForGrid();
                    }
                }

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
                    //else if (Val.Val(txtBackAddLess.Text) == 0 && Val.Val(txtBackAddLess.Text) == 0 && Val.Val(txtBackAddLess.Text) == 0)
                    //{
                    //    OLDMEMODISCOUNT = Val.Val(DRow["OLDMEMODISCOUNT"]);
                    //    OLDMEMOPRICEPERCARAT = Val.Val(DRow["OLDMEMOPRICEPERCARAT"]);
                    //    DouSaleDiscount = OLDMEMODISCOUNT;
                    //    DouSalePricePerCarat = OLDMEMOPRICEPERCARAT;
                    //}
                    else
                    {
                        OLDMEMODISCOUNT = Val.Val(DRow["OLDMEMODISCOUNT"]);
                        OLDMEMOPRICEPERCARAT = Val.Val(DRow["OLDMEMOPRICEPERCARAT"]);
                    }

                    if (Val.Val(txtBackAddLess.Text) != 0)
                    {
                        //Gunjan:14/02/2024
                        if (ChkcheckDiscModify.Checked == true)
                        {
                            DouSaleDiscount = Val.Val(txtBackAddLess.Text);
                            DouSaleRapaport = Val.Val(DRow["SALERAPAPORT"]);
                            DouSalePricePerCarat = Math.Round(DouSaleRapaport + ((DouSaleRapaport * DouSaleDiscount) / 100), 2);
                            //DouSalePricePerCarat = Math.Round(DouSaleRapaport - ((DouSaleRapaport * DouSaleDiscount) / 100), 2); //#P:23-04-2021
                            DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                            OLDMEMODISCOUNT = DouSaleDiscount;
                            OLDMEMOPRICEPERCARAT = DouSalePricePerCarat;
                        }
                        else
                        {
                            DouSaleDiscount = Val.Val(DRow["SALEDISCOUNT"]) + Val.Val(txtBackAddLess.Text);
                            DouSaleRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                            DouSalePricePerCarat = Math.Round(DouSaleRapaport + ((DouSaleRapaport * DouSaleDiscount) / 100), 2);
                            //DouSalePricePerCarat = Math.Round(DouSaleRapaport - ((DouSaleRapaport * DouSaleDiscount) / 100), 2); //#P:23-04-2021
                            DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                            OLDMEMODISCOUNT = DouSaleDiscount;
                            OLDMEMOPRICEPERCARAT = DouSalePricePerCarat;
                        }

                        DouSaleRapaport = Val.Val(DRow["SALERAPAPORT"]);
                        DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                        DRow["MEMODISCOUNT"] = DouSaleDiscount;
                        DRow["MEMOPRICEPERCARAT"] = DouSalePricePerCarat;
                        DRow["MEMOAMOUNT"] = DouSaleAmount;

                        DRow["FMEMOPRICEPERCARAT"] = Math.Round(DouSalePricePerCarat * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022

                        DRow["FMEMOAMOUNT"] = Math.Round(DouSaleAmount * Val.Val(DRow["EXCRATE"]), 3); //#P : 27-01-2022


                    }
                    else
                    {
                        DouSaleDiscount = OLDMEMODISCOUNT;
                        DouSalePricePerCarat = OLDMEMOPRICEPERCARAT;
                    }

                    if (Val.Val(txtPricePerCaratDisc.Text) != 0)
                    {
                        DouSaleRapaport = Val.Val(DRow["SALERAPAPORT"]);
                        if (ChkcheckDiscModify.Checked == true)
                        {
                            DouSalePricePerCarat = Val.Val(txtPricePerCaratDisc.Text);
                            DouSaleDiscount = Math.Round(((DouSaleRapaport - DouSalePricePerCarat) / DouSaleRapaport) * -100, 2);
                            DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                            OLDMEMODISCOUNT = DouSaleDiscount;
                            OLDMEMOPRICEPERCARAT = DouSalePricePerCarat;
                        }
                        else
                        {

                            DouSalePricePerCarat = Val.Val(DRow["SALEPRICEPERCARAT"]) + Val.Val(txtPricePerCaratDisc.Text);
                            DouSaleDiscount = Math.Round(((DouSaleRapaport - DouSalePricePerCarat) / DouSaleRapaport) * -100, 2);
                            DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                            OLDMEMODISCOUNT = DouSaleDiscount;
                            OLDMEMOPRICEPERCARAT = DouSalePricePerCarat;
                        }

                        DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                        DRow["MEMODISCOUNT"] = DouSaleDiscount;
                        DRow["MEMOPRICEPERCARAT"] = DouSalePricePerCarat;
                        DRow["MEMOAMOUNT"] = DouSaleAmount;

                        DRow["FMEMOPRICEPERCARAT"] = Math.Round(DouSalePricePerCarat * Val.Val(txtExcRate.Text), 3); //#P : 27-01-2022

                        DRow["FMEMOAMOUNT"] = Math.Round(DouSaleAmount * Val.Val(DRow["EXCRATE"]), 3); //#P : 27-01-2022
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

                if (mFormType == FORMTYPE.PURCHASEISSUE) // Add Khushbu 05-02-2022 purchase time par cost na data same as sale na data karva mate
                {
                    foreach (DataRow DRow in DTabMemoDetail.Rows)
                    {
                        DRow["SALERAPAPORT"] = Val.Val(DRow["MEMORAPAPORT"]);
                        DRow["SALEDISCOUNT"] = Val.Val(DRow["MEMODISCOUNT"]);
                        DRow["SALEPRICEPERCARAT"] = Val.Val(DRow["MEMOPRICEPERCARAT"]);
                        DRow["SALEAMOUNT"] = Val.Val(DRow["MEMOAMOUNT"]);

                        DRow["FSALEPRICEPERCARAT"] = Val.Val(DRow["FMEMOPRICEPERCARAT"]);

                        if (BOConfiguration.gStrLoginSection == "B")
                        {
                            DRow["FSALEAMOUNT"] = Math.Round(Val.Val(DRow["FMEMOAMOUNT"]) / 1000, 3);
                        }
                        else
                        {
                            DRow["FSALEAMOUNT"] = Val.Val(DRow["FMEMOAMOUNT"]);
                        }
                    }
                }
                DTabMemoDetail.AcceptChanges();
                if (MenuTagName == "SALEINVOICEENTRY")
                {
                    CalculationNewInvoiceEntry();
                    GetSummuryDetailForGridSaleInvoiceEntry();
                }
                else
                {
                    CalculationNew();
                    GetSummuryDetailForGrid();
                }

                BulkPriceCalculation();
                //CalculationNew();
                //GetSummuryDetailForGrid();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnParaClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtBackPriceFileName.Text = "";
                txtBackAddLess.Text = "";
                txtTermsAddLessPer.Text = "";
                txtBlindAddLessPer.Text = "";
                txtPricePerCaratDisc.Text = string.Empty;

                foreach (DataRow DR in DTabMemoDetail.Rows)
                {
                    if (mFormType == FORMTYPE.PURCHASEISSUE) //Add if condition by khushbu. purchase time par janged mathi data as it is karva
                    {
                        DR["MEMODISCOUNT"] = Val.Val(DR["JANGEDDISCOUNT"]);
                        DR["MEMOPRICEPERCARAT"] = Val.Val(DR["JANGEDPRICEPERCARAT"]);
                        DR["MEMOAMOUNT"] = Val.Val(DR["JANGEDAMOUNT"]);
                        DR["MEMORAPAPORT"] = Val.Val(DR["JANGEDRAPAPORT"]);
                        DR["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DR["SALEPRICEPERCARAT"]), 3); //#P : 27-01-2022

                        if (BOConfiguration.gStrLoginSection == "B")
                        {
                            DR["FMEMOAMOUNT"] = Math.Round((Val.Val(txtExcRate.Text) * Val.Val(DR["MEMOAMOUNT"])) / 1000, 3); //#P : 27-01-2022
                        }
                        else
                        {
                            DR["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DR["MEMOAMOUNT"]), 3); //#P : 27-01-2022
                        }

                        DR["SALEDISCOUNT"] = Val.Val(DR["JANGEDDISCOUNT"]);
                        DR["SALEPRICEPERCARAT"] = Val.Val(DR["JANGEDPRICEPERCARAT"]);
                        DR["SALEAMOUNT"] = Val.Val(DR["JANGEDAMOUNT"]);
                        DR["SALERAPAPORT"] = Val.Val(DR["JANGEDRAPAPORT"]);
                        DR["FSALEPRICEPERCARAT"] = Val.Val(DR["FMEMOPRICEPERCARAT"]);
                        DR["FSALEAMOUNT"] = Val.Val(DR["FMEMOAMOUNT"]);

                        DR["OLDMEMODISCOUNT"] = Val.Val(DR["JANGEDDISCOUNT"]);
                        DR["OLDMEMOPRICEPERCARAT"] = Val.Val(DR["JANGEDPRICEPERCARAT"]);
                    }
                    else
                    {
                        DR["MEMODISCOUNT"] = Val.Val(DR["SALEDISCOUNT"]);
                        DR["MEMOPRICEPERCARAT"] = Val.Val(DR["SALEPRICEPERCARAT"]);
                        DR["MEMOAMOUNT"] = Val.Val(DR["SALEAMOUNT"]);
                        DR["MEMORAPAPORT"] = Val.Val(DR["SALERAPAPORT"]);
                        DR["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DR["SALEPRICEPERCARAT"]), 3); //#P : 27-01-2022

                        if (BOConfiguration.gStrLoginSection == "B")
                        {
                            DR["FMEMOAMOUNT"] = Math.Round((Val.Val(txtExcRate.Text) * Val.Val(DR["MEMOAMOUNT"])) / 1000, 3); //#P : 27-01-2022
                        }
                        else
                        {
                            DR["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DR["MEMOAMOUNT"]), 3); //#P : 27-01-2022
                        }

                        DR["JANGEDDISCOUNT"] = Val.Val(DR["SALEDISCOUNT"]);
                        DR["JANGEDPRICEPERCARAT"] = Val.Val(DR["SALEPRICEPERCARAT"]);
                        DR["JANGEDAMOUNT"] = Val.Val(DR["SALEAMOUNT"]);
                        DR["JANGEDRAPAPORT"] = Val.Val(DR["SALERAPAPORT"]);

                        DR["OLDMEMODISCOUNT"] = Val.Val(DR["SALEDISCOUNT"]);
                        DR["OLDMEMOPRICEPERCARAT"] = Val.Val(DR["SALEPRICEPERCARAT"]);
                    }
                }
                CalculationNew();
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
                    foreach (DataRow DRow in DTabMemoDetail.Rows)
                    {
                        DRow["FINALBUYER_ID"] = Val.ToString(txtBuyer.Tag);
                        DRow["FINALBUYERNAME"] = Val.ToString(txtBuyer.Text);
                    }
                    DTabMemoDetail.AcceptChanges();

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

        private void txtAdatPer_Validated(object sender, EventArgs e)
        {
            CalculationNew();
        }

        private void txtBState_Validated(object sender, EventArgs e)
        {
            AutoGstCalculation();
            AutoGstCalculationNew();
        }

        private void RbDollar_CheckedChanged(object sender, EventArgs e)
        {
            //if (RbDollar.Checked && GrdDetail.DataSource != null)
            //{
            //    GrdSummury.Columns["SALEAVGRATEFE"].Visible = false;
            //    GrdSummury.Columns["SALETOTALAMOUNTFE"].Visible = false;
            //    GrdSummury.Columns["SALEAVGDISCFE"].Visible = false;
            //    GrdSummury.Columns["MEMOAVGRATEFE"].Visible = false;
            //    GrdSummury.Columns["MEMOTOTALAMOUNTFE"].Visible = false;
            //    GrdSummury.Columns["MEMOAVGDISCFE"].Visible = false;

            //    GrdSummury.Columns["MEMOAVGRATE"].Visible = true;
            //    GrdSummury.Columns["MEMOTOTALAMOUNT"].Visible = true;
            //    GrdSummury.Columns["MEMOAVGDISC"].Visible = false;

            //    GrdDetail.Columns["MEMOPRICEPERCARAT"].Visible = true;
            //    GrdDetail.Columns["MEMOAMOUNT"].Visible = true;
            //    GrdDetail.Columns["FMEMOPRICEPERCARAT"].Visible = false;
            //    GrdDetail.Columns["FMEMOAMOUNT"].Visible = false;

            //    if (mFormType == FORMTYPE.PURCHASEISSUE)
            //    {
            //        GrdDetail.Columns["SALEPRICEPERCARAT"].Visible = true;
            //        GrdDetail.Columns["SALEAMOUNT"].Visible = true;
            //        GrdDetail.Columns["FSALEPRICEPERCARAT"].Visible = false;
            //        GrdDetail.Columns["FSALEAMOUNT"].Visible = false;

            //    }

            //}
        }

        private void RbRupee_CheckedChanged(object sender, EventArgs e)
        {
            if (RbRupee.Checked && GrdDetail.DataSource != null)
            {
                //GrdSummury.Columns["MEMOAVGRATE"].Visible = false;
                //GrdSummury.Columns["MEMOTOTALAMOUNT"].Visible = false;
                //GrdSummury.Columns["MEMOAVGDISC"].Visible = false;

                //GrdSummury.Columns["SALEAVGRATEFE"].Visible = false;
                //GrdSummury.Columns["SALETOTALAMOUNTFE"].Visible = false;
                //GrdSummury.Columns["SALEAVGDISCFE"].Visible = false;

                //GrdSummury.Columns["MEMOAVGRATEFE"].Visible = true;
                //GrdSummury.Columns["MEMOTOTALAMOUNTFE"].Visible = true;

                //GrdSummury.Columns["MEMOAVGDISCFE"].Visible = false;

                //GrdDetail.Columns["MEMOPRICEPERCARAT"].Visible = false;
                //GrdDetail.Columns["MEMOAMOUNT"].Visible = false;
                //GrdDetail.Columns["FMEMOPRICEPERCARAT"].Visible = true;
                //GrdDetail.Columns["FMEMOAMOUNT"].Visible = true;

                if (mFormType == FORMTYPE.PURCHASEISSUE)
                {
                    GrdDetail.Columns["SALEPRICEPERCARAT"].Visible = false;
                    GrdDetail.Columns["SALEAMOUNT"].Visible = false;
                    GrdDetail.Columns["FSALEPRICEPERCARAT"].Visible = true;
                    GrdDetail.Columns["FSALEAMOUNT"].Visible = true;

                }

            }
        }

        private void MainGridSummury_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
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

                DataTable DtReturnStoneList = new DataTable();

                //#P : 06-10-2019
                DtReturnStoneList = GetTableOfSelectedRows(GrdDetail, true);
                if (DtReturnStoneList.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records That You Want To Return..");
                    return;
                }


                this.Cursor = Cursors.WaitCursor;
                //var SelectedPartyStoneNo = DtReturnStoneList.AsEnumerable().Select(s => s.Field<string>("PARTYSTOCKNO")).ToArray();
                //string StrPartyStoneNoList = string.Join(",", SelectedPartyStoneNo);

                //LiveStockProperty LStockProperty = new LiveStockProperty();
                //LStockProperty.STOCKNO = Val.ToString(StrPartyStoneNoList);
                //LStockProperty.STOCKTYPE = "All";
                ////DataSet DsLiveStock = new BOTRN_StockUpload().GetLiveStockData(LStockProperty); //Cmnt : Pinali : 18-11-2019 Coz Cntain Pagination
                //DataSet DsLiveStock = new BOTRN_StockUpload().GetStoneDetailForMemoForm(LStockProperty);

                DataTable DtabInvoiceDetail = new DataTable();
                // DtabInvoiceDetail = DsLiveStock.Tables[0];
                DtabInvoiceDetail = DtReturnStoneList;

                if (DtabInvoiceDetail.Rows.Count > 0)
                {
                    this.Cursor = Cursors.Default;
                    FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                    FrmMemoEntry.MdiParent = Global.gMainRef;
                    FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);

                    if (mFormType == FORMTYPE.PURCHASEISSUE)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.PURCHASERETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                    else if (mFormType == FORMTYPE.MEMOISSUE)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.MEMORETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                    else if (mFormType == FORMTYPE.ORDERCONFIRM)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.ORDERCONFIRMRETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                    else if (mFormType == FORMTYPE.SALEINVOICE)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALESDELIVERYRETURN, DtabInvoiceDetail, DTabMemo, mStrStockType, Val.ToString(lblMemoNo.Tag));
                    else if (mFormType == FORMTYPE.LABISSUE)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.LABRETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                    else if (mFormType == FORMTYPE.CONSIGNMENTISSUE)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.CONSIGNMENTRETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));

                    else if (mFormType == FORMTYPE.HOLD)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.RELEASE, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                }

                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
            //FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
            //FrmMemoEntry.MdiParent = Global.gMainRef;
            //FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
            //FrmMemoEntry.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")), mStrStockType);
        }

        private void BtnJangedNo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Val.ToString(txtJangedNo.Text) == "")
                {
                    Global.Message("Memo No Is Required");
                    txtJangedNo.Focus();
                    return;
                }
            if (Global.Confirm("Are You Sure to Print Janged?") == System.Windows.Forms.DialogResult.Yes)
            {
                this.Cursor = Cursors.Default;
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    Int64 StrJangedNo = Val.ToInt64(lblMemoNo.Text);

                    if (StrJangedNo == 0)
                    {
                        Global.Message("Memo No Not Found , Please Check !!!");
                    }

                    //DataTable DTab = ObjMemo.GetJangedPrintData(StrJangedNo);
                    DataTable DTab = ObjMemo.GetJangedPrintDataRJ(StrJangedNo);

                    if (DTab.Rows.Count == 0)
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("There Is No Data Found For Print");
                        return;
                    }

                    int TotalRow = 20;
                    int NewRow = TotalRow - DTab.Rows.Count;

                    for (int i = 0; i < NewRow; i++)
                    {
                        DataRow DRNew = DTab.NewRow();
                        DTab.Rows.Add(DRNew);
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
                    FrmReportViewer.ShowFormJangadPrintWithDuplicate("RPT_JangedPrintSubReportnew", DS);
                    this.Cursor = Cursors.Default;
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message(ex.Message);
                }
            }
            this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
              this.Cursor = Cursors.Default;
            }  
        }

        private void btnTrialEInvoiceUpload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Global.Confirm("Do You Want To  Upload E-Invoice On GSTR ?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                else
                {
                    MemoEntryProperty Property = new MemoEntryProperty();
                    string ReturnMessageType = "";

                    Property.MEMO_ID = (SaleInvoiceMemoID == "") ? "00000000-0000-0000-0000-000000000000" : SaleInvoiceMemoID;
                    Property = ObjMemo.UpdateISPrintFlag(Property);
                    ReturnMessageType = Property.ReturnValue;

                    if (ReturnMessageType == "1")
                    {
                        Autosave = true;
                        BtnSave_Click(null, null);
                    }
                    EInvoiceUploadGenerateIRN();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                return;
            }
        }

        private void btntrialEInvoiceCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Global.Confirm("Do You Want To Cancel E-Invoice On GSTR ?") == System.Windows.Forms.DialogResult.No)
                    return;
                else
                    EInvoiceCancel();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                return;
            }
        }

        private void btnAccPrint_Click(object sender, EventArgs e)
        {
            try
            {
                //PnlAccPrint.Visible = true;
                PnlAccPrint.Visible = true;
                PnlPrintState.Visible = true;

                MemoEntryProperty Property = new MemoEntryProperty();
                string ReturnMessageType = "";

                Property.MEMO_ID = (SaleInvoiceMemoID == "") ? "00000000-0000-0000-0000-000000000000" : SaleInvoiceMemoID;
                Property = ObjMemo.UpdateISPrintFlag(Property);
                ReturnMessageType = Property.ReturnValue;

                if (ReturnMessageType == "1")
                {
                    if (IsEInvoiceDone == false)
                    {
                        Autosave = true;
                        BtnSave_Click(null, null);
                    }
                }
                if (cmbAccType.SelectedIndex == 4 || cmbAccType.SelectedIndex == 3)
                {
                    cLabel84.Text = "SIGNATURE :";
                    cLabel84.Font = new Font("Verdana", 7, FontStyle.Bold);
                    txtAccState.Text = "ARVINDBHAI S. GAJERA";
                    btnPacktingListPrintRs.Text = "PackingList ($)";
                    btnPrintLocal.Text = "Print ($)";
                    btnExportWraper.Visible = true;
                    if (cmbAccType.SelectedIndex == 4)
                        PnlExportPrint.Visible = true;
                    else
                        PnlExportPrint.Visible = false;
                }
                else
                {
                    //txtAccState.Text = txtBState.Text;
                    txtAccState.Text = "MAHARASHTRA";
                    btnPacktingListPrintRs.Text = "PackingList (Rs.)";
                    btnPrintLocal.Text = "Print (Rs.)";
                    btnExportWraper.Visible = false;
                    PnlExportPrint.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                return;
            }
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private void btnAccInvoicePrintRs_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbAccType.Text == "Local Sales")
                {
                    PnlPrintState.Visible = true;
                    txtAccState.Text = txtBState.Text;
                    btnPanelPrint_Click(null, null);
                    //txtAccState.Select();
                }
                else { btnPanelPrint_Click(null, null); }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void RepsTxtItemName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "ITEMNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = ObjMemo.GetDistinctItemName();
                    FrmSearch.mBoolISPostBack = true;
                    FrmSearch.mStrISPostBackColumn = "ITEMNAME";
                    FrmSearch.mStrColumnsToHide = "";

                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdSummuryMNL.SetFocusedRowCellValue("ITEMNAME", Val.ToString(FrmSearch.DRow["ITEMNAME"]));
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

        private void RepsTxtHSNCODE_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "HSNCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = ObjMemo.GetDistinctHSNCODE();
                    FrmSearch.mBoolISPostBack = true;
                    FrmSearch.mStrISPostBackColumn = "HSNCODE";
                    FrmSearch.mStrColumnsToHide = "";

                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdSummuryMNL.SetFocusedRowCellValue("HSNCODE", Val.ToString(FrmSearch.DRow["HSNCODE"]));
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

        private void repTxtCostPricePerCaratSaleEntry_Validating(object sender, CancelEventArgs e)
        {
            if (GrdSummuryMNL.FocusedRowHandle < 0)
                return;

            try
            {
                GrdSummuryMNL.PostEditor();

                double DouCarat = 0;
                double DouCostDiscount = 0;
                double DouCostPricePerCarat = 0;
                double DouCostRapaport = 0;
                double DouCostAmount = 0;
                double DouCostFEAmount = 0;

                DataRow DR = GrdSummuryMNL.GetFocusedDataRow();
                DouCarat = Val.Val(GrdSummuryMNL.GetFocusedRowCellValue("CARAT"));
                DouCostPricePerCarat = Val.Val(GrdSummuryMNL.EditingValue);

                DouCostRapaport = Val.Val(GrdSummuryMNL.GetFocusedRowCellValue("MEMORAPAPORT"));

                if (DouCostRapaport != 0)
                    DouCostDiscount = Math.Round((DouCostPricePerCarat - DouCostRapaport) / DouCostRapaport * 100, 2);
                else
                    DouCostDiscount = 0;
                if (Val.ToString(cmbBillType.SelectedItem).ToUpper() == "DOLLARBILL")
                {
                    DouCostAmount = Math.Round(DouCostPricePerCarat * DouCarat, 2);
                    DouCostFEAmount = Math.Round(DouCostAmount * Val.Val(txtExcRate.Text), 2);
                    //DR["FMEMOPRICEPERCARAT"] = Math.Round(DouCostFEAmount / DouCarat, 2);
                }
                //else if (Val.ToString(cmbBillType.SelectedItem).ToUpper() == "RUPEESBILL")
                //{
                //    DouCostFEAmount = Math.Round(DouCostPricePerCarat * DouCarat, 2);
                //    if (Val.Val(txtExcRate.Text) != 0)
                //    {
                //        DouCostAmount = Math.Round(DouCostFEAmount / Val.Val(txtExcRate.Text), 2);
                //    }
                //    else
                //    {
                //        DouCostAmount = 0;
                //    }

                //    DR["MEMOPRICEPERCARAT"] = Math.Round(DouCostAmount / DouCarat, 2);
                //}

                DR["MEMODISCOUNT"] = DouCostDiscount;
                DR["MEMOAMOUNT"] = DouCostAmount;
                //DR["FMEMOAMOUNT"] = DouCostFEAmount;

                //Calculation();
                CalculationNew();

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtBrokerExcRate_Validated(object sender, EventArgs e)
        {
            CalculationNewInvoiceEntry();
        }

        private void rdoBroTDS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoBroTDS.SelectedIndex == 1)
                {
                    txtBroTdsDollar.Enabled = false;
                    txtBroTdsPer.Enabled = false;
                    txtBroTdsRs.Enabled = false;
                    txtBroTAmt.Enabled = false;
                    txtBroTdsDollar.Text = Val.ToString("0.000");
                    txtBroTdsPer.Text = Val.ToString("0.000");
                    txtBroTdsRs.Text = Val.ToString("0.000");
                    txtBroTAmt.Text = Val.ToString("0.000");
                }
                else
                {
                    txtBroTAmt.Enabled = false;
                    txtBroTdsDollar.Enabled = false;
                    txtBroTdsPer.Enabled = true;
                    txtBroTdsRs.Enabled = false;
                    txtBroTdsPer_Validated(null, null); //Add shiv 21-0-2022
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtBroTdsPer_Validated(object sender, EventArgs e)
        {
            try
            {
                txtBroTdsPer.Text = "5.00";
                if (Val.Val(txtBrokerAmtFE.Text) != 0)
                {
                    if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
                    {
                        if (cmbAccType.SelectedIndex == 3 || cmbAccType.SelectedIndex == 2 || cmbAccType.SelectedIndex == 4)
                        {
                            txtBroTdsRs.Text = Val.Format(Math.Round((Val.Val(txtBrokerAmtFE.Text) * Val.Val(txtBroTdsPer.Text)) / 100, 0), "0");
                            txtBroTdsDollar.Text = Val.Format(Math.Round(Val.Val(txtBroTdsRs.Text) / Val.Val(txtBrokerExcRate.Text), 2), "########0.00");
                            txtBroTAmt.Text = Val.Format(Math.Round(Val.Val(txtBrokerAmtFE.Text) - Val.Val(txtBroTdsRs.Text), 2), "########0.00");
                            txtBroTdsDollar.Enabled = false;
                            txtBroTdsRs.Enabled = false;
                        }
                        else
                        {
                            txtBroTdsRs.Text = Val.Format(Math.Round((Val.Val(txtBrokerAmtFE.Text) * Val.Val(txtBroTdsPer.Text)) / 100, 0), "0");
                            txtBroTAmt.Text = Val.Format(Math.Round(Val.Val(txtBrokerAmtFE.Text) - Val.Val(txtBroTdsRs.Text), 2), "########0.00");
                            txtBroTdsDollar.Enabled = false;
                            txtBroTdsRs.Enabled = false;
                        }
                    }
                    else
                    {
                        txtBroTdsDollar.Text = Math.Round((Val.Val(txtBrokerAmtFE.Text) * Val.Val(txtBroTdsPer.Text)) / 100, 3).ToString();
                        txtBroTdsRs.Text = Val.Format(Math.Round(Val.Val(txtBroTdsDollar.Text) * Val.Val(txtBrokerExcRate.Text), 3), "########0.000");
                    }
                }
                else
                {
                    txtBroTdsPer.Text = "0.000";
                    txtBroTdsDollar.Text = "0.000";
                    txtBroTdsRs.Text = "0.000";
                }

                if (MenuTagName == "SALEINVOICEENTRY")
                    CalculationNewInvoiceEntry();
                else
                    CalculationNew();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void btnBrodebitPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdBroGST.SelectedIndex != 1)
                {
                    if (Global.Confirm("Are You Sure For Print Entry") == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        if (Val.ToString(lblMemoNo.Tag) == "")
                        {
                            Global.Message("No Memo Found");
                            return;
                        }
                        this.Cursor = Cursors.WaitCursor;
                        DataTable DTab = new DataTable();
                        if (MenuTagName == "SALEINVOICEENTRY")
                        {
                            DTab = ObjMemo.PrintBroDebitNote(Val.ToString(lblMemoNo.Tag), "INR");
                        }
                        else
                        {
                            DTab = ObjMemo.PrintBroDebitNote(Val.ToString(lblMemoNo.Tag), "INR");
                        }

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
                        DTabDuplicate.AcceptChanges();
                        DS.Tables.Add(DTabDuplicate);

                        string BrokerName = "";
                        if (DTabDuplicate.Rows.Count > 0)
                        {
                            BrokerName = Val.ToString(DTabDuplicate.Rows[0]["BrokerName"]);
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("AccSaleBrokerGSTDebitNote", DTabDuplicate, BrokerName, "");
                        this.Cursor = Cursors.Default;
                    }
                }
                else
                {
                    if (Global.Confirm("Are You Sure For Print Entry") == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        if (Val.ToString(lblMemoNo.Tag) == "")
                        {
                            Global.Message("No Memo Found");
                            return;
                        }
                        this.Cursor = Cursors.WaitCursor;
                        DataTable DTab = new DataTable();
                        if (MenuTagName == "SALEINVOICEENTRY")
                        {
                            DTab = ObjMemo.PrintBroDebitNote(Val.ToString(lblMemoNo.Tag), "INR");
                        }
                        else
                        {
                            DTab = ObjMemo.PrintBroDebitNote(Val.ToString(lblMemoNo.Tag), "INR");
                        }

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
                        DTabDuplicate.AcceptChanges();
                        DS.Tables.Add(DTabDuplicate);

                        string BrokerName = "";
                        if (DTabDuplicate.Rows.Count > 0)
                        {
                            BrokerName = Val.ToString(DTabDuplicate.Rows[0]["BrokerName"]);
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormMemoPrint("AccBrokerSaleDebitNote", DS, BrokerName);
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void txtBroTdsPer_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnExportWraper_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (Global.Confirm("Are You Sure For Print Entry") == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        if (Val.ToString(lblMemoNo.Tag) == "")
                        {
                            Global.Message("No Memo Found");
                            return;
                        }
                        this.Cursor = Cursors.WaitCursor;
                        DataTable DTab = new DataTable();
                        if (MenuTagName == "SALEINVOICEENTRY")
                        {
                            DTab = ObjMemo.PrintExpWraper(Val.ToString(lblMemoNo.Tag), "INR");
                        }
                        else
                        {
                            DTab = ObjMemo.PrintExpWraper(Val.ToString(lblMemoNo.Tag), "INR");
                        }

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
                        DTabDuplicate.AcceptChanges();
                        DS.Tables.Add(DTabDuplicate);

                        if (DTabDuplicate.Rows.Count > 0)
                        {
                            //string BrokerName = "";
                            //BrokerName = Val.ToString(DTabDuplicate.Rows[0]["BrokerName"]);
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("AccExportWaraperPrint", DTabDuplicate);
                        this.Cursor = Cursors.Default;
                    }
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GrdSummuryMNL_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }
            DataRow DRow = GrdSummuryMNL.GetDataRow(e.RowHandle);

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
                        FindRapSaleEntry(DRow, e.RowHandle);
                    }

                    break;
                case "BALANCEPCS":
                case "BALANCECARAT":


                    DTabMemoDetail.Rows[e.RowHandle]["PCS"] = DTabMemoDetail.Rows[e.RowHandle]["BALANCEPCS"];
                    DTabMemoDetail.Rows[e.RowHandle]["CARAT"] = DTabMemoDetail.Rows[e.RowHandle]["BALANCECARAT"];
                    FindRapSaleEntry(DRow, e.RowHandle);
                    break;

                case "SHAPENAME":
                case "COLORNAME":
                case "CLARITYNAME":
                case "CARAT":

                    FindRapSaleEntry(DRow, e.RowHandle);

                    break;

                case "RETURNCARAT":
                    FindRapSaleRetEntry(DRow, e.RowHandle);
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

                    if (cmbBillType.Text.ToUpper() != "RUPEESBILL")
                    {
                        if (DouRapaport != 0)
                            DouPricePerCarat = Math.Round(DouRapaport - ((DouRapaport * DouDiscount) / 100), 2); //#S
                        else
                            DouPricePerCarat = 0;
                    }
                    else
                    {
                        if (DouRapaport != 0)
                            DouPricePerCarat = Math.Round(DouRapaport - ((DouRapaport * DouDiscount) / 100), 0); //#P
                        else
                            DouPricePerCarat = 0;
                    }



                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 0);

                    DTabMemoDetail.Rows[e.RowHandle]["SALEPRICEPERCARAT"] = DouPricePerCarat;
                    DTabMemoDetail.Rows[e.RowHandle]["SALEAMOUNT"] = DouAmount;
                    DTabMemoDetail.AcceptChanges();
                    GrdSummuryMNL.PostEditor();
                    MainGridSummuryMNL.Refresh();

                    break;

                case "SALEPRICEPERCARAT":
                    DouCarat = Val.Val(DRow["CARAT"]);
                    DouRapaport = Val.Val(DRow["SALERAPAPORT"]);
                    DouPricePerCarat = Val.Val(DRow["SALEPRICEPERCARAT"]);
                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                    if (cmbBillType.Text.ToUpper() != "RUPEESBILL")
                    {
                        if (DouRapaport != 0)
                            //DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                            DouDiscount = Math.Round(((DouRapaport - DouPricePerCarat) / DouRapaport) * 100, 2);  //#P
                        else
                            DouDiscount = 0;
                    }
                    else
                    {
                        if (DouRapaport != 0)
                            //DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                            DouDiscount = Math.Round(((DouRapaport - DouPricePerCarat) / DouRapaport) * 100, 0);  //#P
                        else
                            DouDiscount = 0;
                    }

                    DTabMemoDetail.Rows[e.RowHandle]["SALEDISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["SALEAMOUNT"] = DouAmount;
                    DTabMemoDetail.AcceptChanges();
                    GrdSummuryMNL.PostEditor();
                    break;

                case "MEMORAPAPORT":
                case "MEMODISCOUNT":
                    DouCarat = Val.Val(DRow["CARAT"]);
                    DouRapaport = Val.Val(DRow["SALERAPAPORT"]);
                    DouDiscount = Val.Val(DRow["MEMODISCOUNT"]);

                    if (cmbBillType.Text.ToUpper() != "RUPEESBILL")
                    {
                        if (DouRapaport != 0)
                            DouPricePerCarat = Math.Round(DouRapaport - ((DouRapaport * DouDiscount) / 100), 2);   //#P
                        else
                            DouPricePerCarat = Val.Val(DRow["SALEPRICEPERCARAT"]);
                    }
                    else
                    {
                        if (DouRapaport != 0)
                            DouPricePerCarat = Math.Round(DouRapaport - ((DouRapaport * DouDiscount) / 100), 0);   //#P
                        else
                            DouPricePerCarat = Val.Val(DRow["SALEPRICEPERCARAT"]);
                    }

                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                    DTabMemoDetail.Rows[e.RowHandle]["MEMOPRICEPERCARAT"] = DouPricePerCarat;
                    DTabMemoDetail.Rows[e.RowHandle]["MEMOAMOUNT"] = DouAmount;
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(txtExcRate.Text), 0);
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 0);
                    DTabMemoDetail.AcceptChanges();
                    GrdSummuryMNL.PostEditor();
                    MainGridSummuryMNL.Refresh();

                    break;

                case "MEMOPRICEPERCARAT":
                    DouCarat = Val.Val(DRow["CARAT"]);
                    DouRapaport = Val.Val(DRow["SALERAPAPORT"]);
                    DouPricePerCarat = Val.Val(DRow["MEMOPRICEPERCARAT"]);
                    if (cmbBillType.Text.ToUpper() != "RUPEESBILL")
                    {
                        DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);
                    }
                    else
                    {
                        DouAmount = Math.Round(DouPricePerCarat * DouCarat, 0);
                    }

                    if (cmbBillType.Text.ToUpper() != "RUPEESBILL")
                    {
                        if (DouRapaport != 0)
                            //DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                            DouDiscount = Math.Round(((DouRapaport - DouPricePerCarat) / DouRapaport) * 100, 2);  //#P
                        else
                            DouDiscount = 0;
                    }
                    else
                    {
                        if (DouRapaport != 0)
                            //DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                            DouDiscount = Math.Round(((DouRapaport - DouPricePerCarat) / DouRapaport) * 100, 0);  //#P
                        else
                            DouDiscount = 0;
                    }


                    DTabMemoDetail.Rows[e.RowHandle]["MEMODISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["MEMOAMOUNT"] = DouAmount;
                    //DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(txtExcRate.Text), 2);
                    //DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 2);

                    DTabMemoDetail.Rows[e.RowHandle]["SALEDISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["SALEAMOUNT"] = DouAmount;

                    DTabMemoDetail.AcceptChanges();
                    GrdSummuryMNL.PostEditor();
                    BtnContinue_Click(null, null);
                    break;
                case "FMEMOPRICEPERCARAT":
                    DouCarat = Val.Val(DRow["CARAT"]);
                    DouRapaport = Val.Val(DRow["SALERAPAPORT"]);
                    DouPricePerCarat = Val.Val(DRow["FMEMOPRICEPERCARAT"]);
                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 0);

                    if (cmbBillType.Text.ToUpper() != "RUPEESBILL")
                    {
                        if (DouRapaport != 0)
                            //DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                            DouDiscount = Math.Round(((DouRapaport - DouPricePerCarat) / DouRapaport) * 100, 2);  //#P
                        else
                            DouDiscount = 0;
                    }
                    else
                    {
                        if (DouRapaport != 0)
                            //DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                            DouDiscount = Math.Round(((DouRapaport - DouPricePerCarat) / DouRapaport) * 100, 0);  //#P
                        else
                            DouDiscount = 0;
                    }


                    //DTabMemoDetail.Rows[e.RowHandle]["MEMODISCOUNT"] = DouDiscount;
                    //DTabMemoDetail.Rows[e.RowHandle]["MEMOAMOUNT"] = DouAmount;
                    //DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(txtExcRate.Text), 2);
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 0);

                    DTabMemoDetail.Rows[e.RowHandle]["SALEDISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["SALEAMOUNT"] = DouAmount;

                    DTabMemoDetail.AcceptChanges();
                    GrdSummuryMNL.PostEditor();
                    BtnContinue_Click(null, null);
                    break;
                default:
                    break;
            }
            //Calculation();
            //CalculationNew();
            CalculationNewInvoiceEntry();
        }

        private void GrdSummuryMNL_MouseLeave(object sender, EventArgs e)
        {
            try
            {

                if (GrdSummuryMNL.FocusedRowHandle < 0)
                    return;

                if (GrdSummuryMNL.FocusedColumn.FieldName == "FMEMOAMOUNT")
                {
                    //editAmountTSM.Enabled = true;
                }
                else
                {
                    //editAmountTSM.Enabled = false;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void editAmountTSM_Click(object sender, EventArgs e)
        {
            //if (GrdSummuryMNL.FocusedColumn.FieldName == "FMEMOAMOUNT")
            //{
            //    if (Global.Confirm("Are You Sure You Want To UPDATE Amount ?") == System.Windows.Forms.DialogResult.Yes)
            //    {
            //        FrmPassword FrmPassword = new FrmPassword();
            //        ObjPer.PASSWORD = "123!@#";
            //        if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
            //        {
            //            GrdSummuryMNL.Columns["FMEMOAMOUNT"].OptionsColumn.AllowEdit = true;
            //        }
            //        else
            //        {
            //            GrdSummuryMNL.Columns["FMEMOAMOUNT"].OptionsColumn.AllowEdit = false;
            //            return;
            //        }
            //    }
            //}
            //else
            //{
            //    editAmountTSM.Enabled = false;
            //}
        }

        private void repTxtStoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (mFormType == FORMTYPE.PURCHASEISSUE)
                {
                    return;
                }

                if (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.SALEINVOICE)
                {
                    if (Global.OnKeyPressEveToPopup(e))
                    {
                        FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                        FrmSearch.mStrSearchField = "STOCKNO";
                        FrmSearch.mStrSearchText = e.KeyChar.ToString();
                        this.Cursor = Cursors.WaitCursor;
                        FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.TRN_STOCKFORMEMO);

                        for (int i = 0; i < DTabMemoDetail.Rows.Count; i++)
                        {
                            var VarPartyNameRows = FrmSearch.mDTab.Rows.Cast<DataRow>().Where(row => Val.ToString(row["STOCKNO"]) == Val.ToString(DTabMemoDetail.Rows[i]["STOCKNO"])).ToArray();
                            foreach (DataRow dr in VarPartyNameRows)
                                FrmSearch.mDTab.Rows.Remove(dr);
                        }

                        FrmSearch.mStrColumnsToHide = "STOCK_ID";
                        this.Cursor = Cursors.Default;
                        FrmSearch.ShowDialog();
                        e.Handled = true;
                        if (FrmSearch.DRow != null)
                        {
                            repTxtStoneNo.Tag = Val.ToString(FrmSearch.DRow["STOCK_ID"]);
                            DataTable DTab = ObjMemo.GetStockDataForMemoDetail(Guid.Parse(Val.ToString(repTxtStoneNo.Tag)));


                            foreach (DataRow DRow in DTab.Rows)
                            {
                                DataRow DRNew = DTabMemoDetail.Rows[DTabMemoDetail.Rows.Count - 1];

                                DRNew["MEMODETAIL_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                                DRNew["STOCK_ID"] = DRow["STOCK_ID"];
                                DRNew["STOCKNO"] = DRow["STOCKNO"];
                                DRNew["PARTYSTOCKNO"] = DRow["PARTYSTOCKNO"];
                                DRNew["STOCKTYPE"] = DRow["STOCKTYPE"];
                                DRNew["PCS"] = DRow["PCS"];
                                DRNew["CARAT"] = DRow["CARAT"];
                                DRNew["BALANCEPCS"] = DRow["BALANCEPCS"];
                                DRNew["BALANCECARAT"] = DRow["BALANCECARAT"];
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
                                if (mFormType == FORMTYPE.LABRETURN)
                                {
                                    DRNew["LABSERVICECODE_ID"] = DRow["LABSERVICECODE_ID"];
                                    DRNew["LABSERVICECODE"] = DRow["LABSERVICECODE"];
                                }
                                DRNew["MEMORAPAPORT"] = DRow["MEMORAPAPORT"];
                                DRNew["MEMODISCOUNT"] = DRow["MEMODISCOUNT"];
                                DRNew["MEMOPRICEPERCARAT"] = DRow["MEMOPRICEPERCARAT"];
                                DRNew["MEMOAMOUNT"] = DRow["MEMOAMOUNT"];
                                DRNew["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOPRICEPERCARAT"]), 2);
                                DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"]), 2);
                                DRNew["JANGEDRAPAPORT"] = DRow["MEMORAPAPORT"];
                                DRNew["JANGEDDISCOUNT"] = DRow["MEMODISCOUNT"];
                                DRNew["JANGEDPRICEPERCARAT"] = DRow["MEMOPRICEPERCARAT"];
                                DRNew["JANGEDAMOUNT"] = DRow["MEMOAMOUNT"];
                                DRNew["STATUS"] = "PENDING";
                                DRNew["REMARK"] = DRow["REMARK"];
                                DRNew["LOCATION_ID"] = DRow["LOCATION_ID"];
                                DRNew["LOCATIONNAME"] = DRow["LOCATIONNAME"];
                                DRNew["SIZE_ID"] = DRow["SIZE_ID"];
                                DRNew["SIZENAME"] = DRow["SIZENAME"];
                                DRNew["LAB_ID"] = DRow["LAB_ID"];
                                DRNew["LABNAME"] = DRow["LABNAME"];
                                DRNew["ISPURCHASE"] = DRow["ISPURCHASE"];
                                DRNew["LABREPORTNO"] = DRow["LABREPORTNO"];

                                if (Val.ToString(lblMode.Text).ToUpper() != "EDIT MODE")
                                {
                                    DRNew["MAINMEMO_ID"] = Val.ToString(Guid.Empty);
                                    DRNew["MEMO_ID"] = Val.ToString(Guid.Empty);
                                }
                                else
                                {
                                    DRNew["MAINMEMO_ID"] = Val.ToString(txtJangedNo.Tag);
                                    DRNew["MEMO_ID"] = Val.ToString(txtJangedNo.Tag);
                                }
                                DRNew["JANGEDNOSTR"] = Val.ToString(txtJangedNo.Text);
                                DRNew["BILLINGPARTYNAME"] = txtBillingParty.Text;
                                DRNew["SHIPPINGPARTYNAME"] = txtShippingParty.Text;
                                DRNew["EXPINVOICERATE"] = DRow["MEMOPRICEPERCARAT"];
                                DRNew["EXPINVOICERATEFE"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOPRICEPERCARAT"]), 2);
                                DRNew["EXPINVOICEAMT"] = DRow["MEMOAMOUNT"];
                                DRNew["EXPINVOICEAMTFE"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"]), 2);
                                DRNew["COSTRAPAPORT"] = DRow["COSTRAPAPORT"];
                                DRNew["COSTDISCOUNT"] = DRow["COSTDISCOUNT"];
                                DRNew["COSTPRICEPERCARAT"] = DRow["COSTPRICEPERCARAT"];
                                DRNew["COSTAMOUNT"] = DRow["COSTAMOUNT"];
                                DRNew["DIAMONDTYPE"] = DRow["DIAMONDTYPE"];
                                DRNew["EXCRATE"] = Val.Val(txtExcRate.Text);
                                DRNew["FMEMOPRICEPERCARAT"] = Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOPRICEPERCARAT"]);
                                DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"]), 3);
                                // DTabMemoDetail.Rows.Add(DRNew);
                            }

                            //for (int i = DTabMemoDetail.Rows.Count - 1; i >= 0; i--)
                            //{
                            //    if (DTabMemoDetail.Rows[i][1] == DBNull.Value)
                            //    {
                            //        DTabMemoDetail.Rows[i].Delete();
                            //    }
                            //}
                            DTabMemoDetail.AcceptChanges();
                            CalculationNew();
                            BulkPriceCalculation();
                            BtnModifyPrice_Click(null, null);
                        }

                        FrmSearch.Hide();
                        FrmSearch.Dispose();
                        FrmSearch = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void BtnBrowseOutSideStone_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OpenFileDialog = new OpenFileDialog();
                OpenFileDialog.Filter = "Excel Files (*.xls,*.xlsx)|*.xls;*.xlsx;";
                if (OpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtOutSideStoneFileName.Text = OpenFileDialog.FileName;

                    string extension = Path.GetExtension(txtOutSideStoneFileName.Text.ToString());
                    string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(txtOutSideStoneFileName.Text);
                    destinationPath = destinationPath.Replace(extension, ".xlsx");
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    File.Copy(txtOutSideStoneFileName.Text, destinationPath);
                    DtabOutSideStone = Global.GetDataTableFromExcel(destinationPath, true);

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

        private void BtnFileUploadOutSideStone_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtOutSideStoneFileName.Text.Length == 0)
                {
                    Global.Message("Please Select Excel File..");
                    BtnBrowseOutSideStone.Focus();
                    return;
                }

                if (DtabOutSideStone != null)
                {
                    DataTable DTShape = DtabPara.Select("PARATYPE = 'SHAPE'").CopyToDataTable();
                    DataTable DTColor = DtabPara.Select("PARATYPE = 'COLOR'").CopyToDataTable();
                    DataTable DTClarity = DtabPara.Select("PARATYPE = 'CLARITY'").CopyToDataTable();
                    DataTable DTCut = DtabPara.Select("PARATYPE = 'CUT'").CopyToDataTable();
                    DataTable DTPolish = DtabPara.Select("PARATYPE = 'POLISH'").CopyToDataTable();
                    DataTable DTSym = DtabPara.Select("PARATYPE = 'SYMMETRY'").CopyToDataTable();
                    DataTable DTFL = DtabPara.Select("PARATYPE = 'FLUORESCENCE'").CopyToDataTable();
                    DataTable DTLocation = DtabPara.Select("PARATYPE = 'LOCATION'").CopyToDataTable();
                    DataTable DTColorShade = DtabPara.Select("PARATYPE = 'COLORSHADE'").CopyToDataTable();
                    DataTable DTMilky = DtabPara.Select("PARATYPE = 'MILKY'").CopyToDataTable();
                    DataTable DTEyeClean = DtabPara.Select("PARATYPE = 'EYECLEAN'").CopyToDataTable();
                    DataTable DTSize = DtabPara.Select("PARATYPE = 'SIZE'").CopyToDataTable();
                    DataTable DTLab = DtabPara.Select("PARATYPE = 'LAB'").CopyToDataTable();
                    DataTable DTLuster = DtabPara.Select("PARATYPE = 'LUSTER'").CopyToDataTable();
                    DataTable DTHeartArrow = DtabPara.Select("PARATYPE = 'HEARTANDARROW'").CopyToDataTable();
                    DataTable DTCulet = DtabPara.Select("PARATYPE = 'CULET'").CopyToDataTable();
                    DataTable DTGirdle = DtabPara.Select("PARATYPE = 'GIRDLE'").CopyToDataTable();
                    DataTable DTTableInc = DtabPara.Select("PARATYPE = 'TABLEINC'").CopyToDataTable();
                    DataTable DTTableOpenInc = DtabPara.Select("PARATYPE = 'TABLEOPENINC'").CopyToDataTable();
                    DataTable DTSideTable = DtabPara.Select("PARATYPE = 'SIDETABLEINC'").CopyToDataTable();
                    DataTable DTSideOpen = DtabPara.Select("PARATYPE = 'SIDEOPENINC'").CopyToDataTable();
                    DataTable DTTableBlack = DtabPara.Select("PARATYPE = 'TABLEBLACKINC'").CopyToDataTable();
                    DataTable DTSideBlack = DtabPara.Select("PARATYPE = 'SIDEBLACKINC'").CopyToDataTable();
                    DataTable DTRedSport = DtabPara.Select("PARATYPE = 'REDSPORTINC'").CopyToDataTable();
                    DataTable DTFancy = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_FANCYCOLOR);

                    int IntCheck = 0, cnt = 1;
                    string StrMessage = "";
                    foreach (DataRow DRow in DtabOutSideStone.Rows)
                    {
                        DataRow DRNew = DTabMemoDetail.NewRow();

                        DRNew["STOCKTYPE"] = DRow["StoneType"];
                        string StockType = Val.ToString(DRow["StoneType"]);
                        StockType = StockType.Trim();
                        StockType = StockType.ToUpper();
                        string StrIsOtherStone = Val.ToString(DRow["IsOtherStone"]);
                        StrIsOtherStone = StrIsOtherStone.Trim();
                        StrIsOtherStone = StrIsOtherStone.ToUpper();
                        if (StrIsOtherStone == "YES")
                            DRNew["ISOTHERSTONE"] = Val.ToBoolean(true);
                        else
                            DRNew["ISOTHERSTONE"] = Val.ToBoolean(false);

                        string StrOnlyPacking = Val.ToString(DRow["OnlyPacking"]);
                        StrOnlyPacking = StrOnlyPacking.Trim();
                        StrOnlyPacking = StrOnlyPacking.ToUpper();
                        if (StrOnlyPacking == "YES")
                            DRNew["ONLYPACKING"] = Val.ToBoolean(true);
                        else
                            DRNew["ONLYPACKING"] = Val.ToBoolean(false);

                        DRNew["MEMODETAIL_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                        DRNew["STOCK_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                        string StrStoneNo = Val.ToString(DRow["StoneNo"]);
                        string PCS = Val.ToString(DRow["PCS"]);
                        if (PCS == "")
                        {
                            DRNew["PCS"] = 0;
                            DRNew["BALANCEPCS"] = 0;
                        }
                        else
                        {
                            DRNew["PCS"] = DRow["PCS"];
                            DRNew["BALANCEPCS"] = DRow["PCS"];
                        }
                        DRNew["PSTATE"] = DRow["PCT"];
                        if (StockType == "SINGLE")
                        {
                            DRNew["STOCKNO"] = DRow["StoneNo"];
                            DRNew["PARTYSTOCKNO"] = DRow["StoneNo"];
                        }
                        else
                        {
                            string DateTimeString = DateTime.Now.ToString("ddMMyy-HHmmss");
                            string ParcelStone = "PRCL" + DateTimeString + cnt;
                            DRNew["STOCKNO"] = ParcelStone;
                            DRNew["PARTYSTOCKNO"] = ParcelStone;
                            //DRNew["PCS"] = 1;
                            //DRNew["BALANCEPCS"] = 1;
                        }
                        DRNew["CARAT"] = DRow["Carat"];
                        DRNew["BALANCECARAT"] = DRow["Carat"];
                        IntCheck = 0;
                        StrMessage = "";
                        DRNew["SHAPE_ID"] = FindID(DTShape, Val.ToString(DRow["Shape"]), StrStoneNo, "Shape", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                        DRNew["SHAPENAME"] = DRow["Shape"];
                        DRNew["COLOR_ID"] = FindID(DTColor, Val.ToString(DRow["Color"]), StrStoneNo, "Color", ref IntCheck, ref StrMessage);
                        DRNew["COLORNAME"] = DRow["Color"];
                        DRNew["CLARITY_ID"] = FindID(DTClarity, Val.ToString(DRow["Clarity"]), StrStoneNo, "Clarity", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                        DRNew["CLARITYNAME"] = DRow["Clarity"];
                        DRNew["CUT_ID"] = FindID(DTCut, Val.ToString(DRow["Cut"]), StrStoneNo, "Cut", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                        DRNew["CUTNAME"] = DRow["Cut"];
                        DRNew["POL_ID"] = FindID(DTPolish, Val.ToString(DRow["Polish"]), StrStoneNo, "Polish", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                        DRNew["POLNAME"] = DRow["Polish"];
                        DRNew["SYM_ID"] = FindID(DTSym, Val.ToString(DRow["Symm"]), StrStoneNo, "Symmetry", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                        DRNew["SYMNAME"] = DRow["Symm"];
                        DRNew["FL_ID"] = FindID(DTFL, Val.ToString(DRow["Flour"]), StrStoneNo, "Fluorescence", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                        DRNew["FLNAME"] = DRow["Flour"];
                        DRNew["MEASUREMENT"] = DRow["Measurements"];
                        DRNew["SALERAPAPORT"] = DRow["RapPrice"];
                        DRNew["SALEDISCOUNT"] = DRow["Disc"];
                        DRNew["SALEPRICEPERCARAT"] = DRow["NetRate"];
                        DRNew["SALEAMOUNT"] = DRow["NetValue"];
                        DRNew["MEMORAPAPORT"] = DRow["RapPrice"];
                        DRNew["MEMODISCOUNT"] = DRow["Disc"];
                        DRNew["MEMOPRICEPERCARAT"] = DRow["NetRate"];
                        DRNew["MEMOAMOUNT"] = DRow["NetValue"];
                        DRNew["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["NetRate"]), 2);
                        DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["NetValue"]), 2);
                        DRNew["JANGEDRAPAPORT"] = DRow["RapPrice"];
                        DRNew["JANGEDDISCOUNT"] = DRow["Disc"];
                        DRNew["JANGEDPRICEPERCARAT"] = DRow["NetRate"];
                        DRNew["JANGEDAMOUNT"] = DRow["NetValue"];
                        DRNew["STATUS"] = "PENDING";
                        DRNew["REMARK"] = DRow["REMARKS"];
                        //DRNew["LOCATION_ID"] = DRow["LOCATION_ID"];
                        //DRNew["LOCATIONNAME"] = DRow["LOCATIONNAME"];
                        //DRNew["SIZE_ID"] = FindID(DTSize, Val.ToString(DRow["Size"]), StrStoneNo, "Size", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                        //DRNew["SIZENAME"] = DRow["Size"];
                        DRNew["LAB_ID"] = FindID(DTLab, Val.ToString(DRow["Lab"]), StrStoneNo, "Lab", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                        DRNew["LABNAME"] = DRow["Lab"];
                        DRNew["ISPURCHASE"] = 0;
                        DRNew["LABREPORTNO"] = DRow["ReportNo"];
                        if (Val.ToString(lblMode.Text).ToUpper() != "EDIT MODE")
                        {
                            DRNew["MAINMEMO_ID"] = Val.ToString(Guid.Empty);
                            DRNew["MEMO_ID"] = Val.ToString(Guid.Empty);
                        }
                        else
                        {
                            DRNew["MAINMEMO_ID"] = Val.ToString(txtJangedNo.Tag);
                            DRNew["MEMO_ID"] = Val.ToString(txtJangedNo.Tag);
                        }
                        //DRNew["MAINMEMO_ID"] = Val.ToString(txtJangedNo.Tag);
                        //DRNew["MEMO_ID"] = Val.ToString(txtJangedNo.Tag);
                        DRNew["JANGEDNOSTR"] = Val.ToString(txtJangedNo.Text);
                        DRNew["BILLINGPARTYNAME"] = txtBillingParty.Text;
                        DRNew["SHIPPINGPARTYNAME"] = txtShippingParty.Text;
                        DRNew["DEPTHPER"] = DRow["Depth"];
                        DRNew["TABLEPER"] = DRow["Table"];
                        DRNew["EXPINVOICERATE"] = DRow["NetRate"];
                        DRNew["EXPINVOICERATEFE"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["NetRate"]), 2);
                        DRNew["EXPINVOICEAMT"] = DRow["NetValue"];
                        DRNew["EXPINVOICEAMTFE"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["NetValue"]), 2);
                        DRNew["ITEMNAME"] = "Cut & Polished Diamond";
                        DRNew["HSNCODE"] = "71023910";
                        DRNew["REMARKM"] = DRow["REMARKS"];
                        DTabMemoDetail.Rows.Add(DRNew);
                        cnt++;
                    }
                }


                var DRowParcel = DTabMemoDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["STOCKTYPE"]) == "PARCEL" && Val.ToString(row["ENTRYSRNO"]) == "").ToArray();
                foreach (DataRow dr in DRowParcel)
                    DTabMemoDetailParcelFile = DTabMemoDetail.AsEnumerable().Where(rows1 => Val.ToString(rows1["STOCKTYPE"]) == "PARCEL").CopyToDataTable();

                var DRowSingle = DTabMemoDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["STOCKTYPE"]) == "PARCEL" && Val.ToString(row["ENTRYSRNO"]) == "" && Val.ToBoolean(row["ISOTHERSTONE"]) == false && Val.ToBoolean(row["ONLYPACKING"]) == true).ToArray();
                foreach (DataRow dr in DRowSingle)
                    DTabMemoDetail.Rows.Remove(dr);

                var DRowSingle1 = DTabMemoDetailParcelFile.Rows.Cast<DataRow>().Where(row => Val.ToString(row["STOCKTYPE"]) == "PARCEL" && Val.ToString(row["ENTRYSRNO"]) == "" && Val.ToBoolean(row["ISOTHERSTONE"]) == true && Val.ToBoolean(row["ONLYPACKING"]) == false).ToArray();
                foreach (DataRow dr in DRowSingle1)
                    DTabMemoDetailParcelFile.Rows.Remove(dr);

                var DRowParcelRmv = DTabMemoDetailParcelFile.Rows.Cast<DataRow>().Where(row => Val.ToString(row["STOCKTYPE"]) == "PARCEL" && Val.ToString(row["ENTRYSRNO"]) != "").ToArray();
                foreach (DataRow dr in DRowParcelRmv)
                    DTabMemoDetailParcelFile.Rows.Remove(dr);

                var DRowParcelRmvfalse = DTabMemoDetailParcelFile.Rows.Cast<DataRow>().Where(row => Val.ToString(row["STOCKTYPE"]) == "SINGLE" && Val.ToString(row["ENTRYSRNO"]) == "" && Val.ToBoolean(row["ISOTHERSTONE"]) == false && Val.ToBoolean(row["ONLYPACKING"]) == false).ToArray();
                foreach (DataRow dr in DRowParcelRmvfalse)
                    DTabMemoDetailParcelFile.Rows.Remove(dr);

                DTabMemoDetail.AcceptChanges();

                MainGrdDetailParcel.DataSource = DTabMemoDetailParcelFile;
                MainGrdDetailParcel.Refresh();

                MainGridSummuryMNLParcel.DataSource = DTabMemoDetailParcelFile;
                MainGridSummuryMNLParcel.Refresh();

                if (MenuTagName == "SALEINVOICEENTRY")
                {
                    if (lblMode.Text == "Add Mode")
                    {
                        //CalculationNew();
                        //GetSummuryDetailForGrid();
                        CalculationNewInvoiceEntry();
                        //GetSummuryDetailForGridSaleInvoiceEntry();
                    }
                    else
                    {
                        CalculationNewInvoiceEntry();
                        GetSummuryDetailForGridSaleInvoiceEntry();
                    }
                }
                else
                {
                    CalculationNew();
                    GetSummuryDetailForGrid();
                }
                IsOutSideStone = true;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public void FindRapSaleEntry(DataRow Dr, int pIntRowIndex)
        {
            double Rapaport = 0;
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

            var VarQry = (from DrRapaport in DtabRapaport.AsEnumerable()
                          where Val.ToString(DrRapaport["SHAPE"]).ToUpper() == StrShape.ToUpper()
                          && Val.ToString(DrRapaport["COLOR"]).ToUpper() == Val.ToString(Dr["COLORNAME"]).ToUpper()
                          && Val.ToString(DrRapaport["CLARITY"]).ToUpper() == Val.ToString(Dr["CLARITYNAME"]).ToUpper()
                          && Val.Val(Dr["CARAT"]) >= Val.Val(DrRapaport["FROMCARAT"])
                          && Val.Val(Dr["CARAT"]) <= Val.Val(DrRapaport["TOCARAT"])
                          select DrRapaport).ToList();

            if (VarQry.Any())
            {
                Rapaport = Val.Val(VarQry.FirstOrDefault()["NVALUE"]);
                CostPricePerCarat = Rapaport + (Rapaport * CostDiscount / 100);
                CostAmount = CostPricePerCarat * Carat;

                Rapaport = Val.Val(VarQry.FirstOrDefault()["NVALUE"]);
                SalePricePerCarat = Rapaport + (Rapaport * SaleDiscount / 100);
                SaleAmount = SalePricePerCarat * Carat;

                DTabMemoDetail.Rows[pIntRowIndex]["SALERAPAPORT"] = Rapaport;
                DTabMemoDetail.Rows[pIntRowIndex]["SALEPRICEPERCARAT"] = CostPricePerCarat;
                DTabMemoDetail.Rows[pIntRowIndex]["SALEAMOUNT"] = CostAmount;

                DTabMemoDetail.Rows[pIntRowIndex]["MEMORAPAPORT"] = Rapaport;
                DTabMemoDetail.Rows[pIntRowIndex]["MEMOPRICEPERCARAT"] = SalePricePerCarat;
                DTabMemoDetail.Rows[pIntRowIndex]["MEMOAMOUNT"] = SaleAmount;
                DTabMemoDetail.Rows[pIntRowIndex]["FMEMOPRICEPERCARAT"] = Math.Round(SalePricePerCarat * Val.Val(txtExcRate.Text), 2);
                DTabMemoDetail.Rows[pIntRowIndex]["FMEMOAMOUNT"] = Math.Round(SaleAmount * Val.Val(txtExcRate.Text), 2);

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

                Rapaport = Val.Val(Dr["MEMORAPAPORT"]);
                SalePricePerCarat = Val.Val(Dr["MEMOPRICEPERCARAT"]);
                SaleAmount = SalePricePerCarat * Carat;

                //if (cmbAccType.Text.ToUpper() == "EXPORT")
                //{
                //    SaleAmount = Math.Round(SaleAmount, 2);
                //}
                //else
                //{
                //    SaleAmount = Math.Round(SaleAmount, 3);
                //}

                if (cmbAccType.Text.ToUpper() == "LOCAL SALES")
                {
                    Rapaport = Val.Val(Dr["MEMORAPAPORT"]);
                    SalePricePerCarat = Val.Val(Dr["FMEMOPRICEPERCARAT"]);
                    SaleAmount = SalePricePerCarat * Carat;
                    SaleAmount = Math.Round(SaleAmount, 0);
                }
                else
                {
                    SaleAmount = Math.Round(SaleAmount, 2);
                    Rapaport = Val.Val(Dr["MEMORAPAPORT"]);
                    SalePricePerCarat = Val.Val(Dr["MEMOPRICEPERCARAT"]);
                    //SaleAmount = SalePricePerCarat * Carat;
                }

                DTabMemoDetail.Rows[pIntRowIndex]["SALERAPAPORT"] = Rapaport;
                DTabMemoDetail.Rows[pIntRowIndex]["SALEPRICEPERCARAT"] = CostPricePerCarat;
                DTabMemoDetail.Rows[pIntRowIndex]["SALEAMOUNT"] = CostAmount;

                DTabMemoDetail.Rows[pIntRowIndex]["MEMORAPAPORT"] = Rapaport;
                DTabMemoDetail.Rows[pIntRowIndex]["MEMOPRICEPERCARAT"] = SalePricePerCarat;
                DTabMemoDetail.Rows[pIntRowIndex]["MEMOAMOUNT"] = SaleAmount;
                DTabMemoDetail.Rows[pIntRowIndex]["FMEMOPRICEPERCARAT"] = Math.Round(SalePricePerCarat * Val.Val(txtExcRate.Text), 2);
                DTabMemoDetail.Rows[pIntRowIndex]["FMEMOAMOUNT"] = Math.Round(SaleAmount * Val.Val(txtExcRate.Text), 2);

                DTabMemoDetail.AcceptChanges();
            }
            //Calculation();
            CalculationNew();
            CalculationNewInvoiceEntry();
        }

        private void rdBroGST_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBroGST.SelectedIndex == 1)
                {
                    txtBroCgstPer.Enabled = false;
                    txtBroSgstPer.Enabled = false;
                    txtBroIgstPer.Enabled = false;
                    txtBroCgstPer.Text = "0.00";
                    txtBroSgstPer.Text = "0.00";
                    txtBroIgstPer.Text = "0.00";
                    txtBroCgstAmt.Text = "0.00";
                    txtBroSgstAmt.Text = "0.00";
                    txtBroIgstAmt.Text = "0.00";
                }
                else
                {
                    txtBroCgstPer.Enabled = true;
                    txtBroSgstPer.Enabled = true;
                    txtBroIgstPer.Enabled = true;
                    txtBroCgstPer.Focus();
                    string StrTxtBState = txtBState.Text.ToUpper();
                    if (StrTxtBState == "MAHARASHTRA")
                    {
                        txtBroCgstPer_Validated(null, null);
                        txtBroSgstPer_Validated(null, null);
                    }
                    else
                    {
                        txtBroIgstPer_Validated(null, null);
                    }
                }

                if (MenuTagName == "SALEINVOICEENTRY")
                    CalculationNewInvoiceEntry();
                else
                    CalculationNew();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtBroCgstPer_Validated(object sender, EventArgs e)
        {
            txtBroCgstPer.Text = "9.00";
            if (Val.Val(txtBrokerAmtFE.Text) != 0)
            {
                if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE)
                {
                    txtBroCgstAmt.Text = Math.Round((Val.Val(txtBrokerAmtFE.Text) * Val.Val(txtBroCgstPer.Text)) / 100, 0).ToString();
                }
            }
            else
            {
                txtBroCgstPer.Text = "0.000";
                txtBroCgstAmt.Text = "0.000";
                txtBroSgstPer.Text = "0.000";
                txtBroSgstAmt.Text = "0.000";
                txtBroIgstPer.Text = "0.000";
                txtBroIgstAmt.Text = "0.000";
            }

            if (MenuTagName == "SALEINVOICEENTRY")
                CalculationNewInvoiceEntry();
            else
                CalculationNew();
        }

        private void txtBroSgstPer_Validated(object sender, EventArgs e)
        {
            txtBroSgstPer.Text = "9.00";
            if (Val.Val(txtBrokerAmtFE.Text) != 0)
            {
                if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE)
                {
                    txtBroSgstAmt.Text = Math.Round((Val.Val(txtBrokerAmtFE.Text) * Val.Val(txtBroSgstPer.Text)) / 100, 0).ToString();
                }
            }
            else
            {
                txtBroCgstPer.Text = "0.000";
                txtBroCgstAmt.Text = "0.000";
                txtBroSgstPer.Text = "0.000";
                txtBroSgstAmt.Text = "0.000";
                txtBroIgstPer.Text = "0.000";
                txtBroIgstAmt.Text = "0.000";
            }

            if (MenuTagName == "SALEINVOICEENTRY")
                CalculationNewInvoiceEntry();
            else
                CalculationNew();
        }

        private void txtBroIgstPer_Validated(object sender, EventArgs e)
        {
            txtBroIgstPer.Text = "18.00";
            if (Val.Val(txtBrokerAmtFE.Text) != 0)
            {
                if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE)
                {
                    txtBroIgstAmt.Text = Math.Round((Val.Val(txtBrokerAmtFE.Text) * Val.Val(txtBroIgstPer.Text)) / 100, 0).ToString();
                }
            }
            else
            {
                txtBroCgstPer.Text = "0.000";
                txtBroCgstAmt.Text = "0.000";
                txtBroSgstPer.Text = "0.000";
                txtBroSgstAmt.Text = "0.000";
                txtBroIgstPer.Text = "0.000";
                txtBroIgstAmt.Text = "0.000";
            }

            if (MenuTagName == "SALEINVOICEENTRY")
                CalculationNewInvoiceEntry();
            else
                CalculationNew();
        }

        private void GrdSummuryMNLParcel_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                DataRow DRow = GrdSummuryMNLParcel.GetDataRow(e.RowHandle);

                switch (e.Column.FieldName.ToUpper())
                {
                    case "CARAT":
                        double DouCarat = Val.Val(DRow["CARAT"]);
                        double DouPricePerCarat = Val.Val(DRow["MEMOPRICEPERCARAT"]);
                        DTabMemoDetailParcelFile.Rows[e.RowHandle]["MEMOAMOUNT"] = Math.Round(DouCarat * DouPricePerCarat, 3);
                        double DouMemoAmtPrcl = Val.Val(DRow["MEMOAMOUNT"]);
                        DTabMemoDetailParcelFile.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(txtExcRate.Text), 3);
                        DTabMemoDetailParcelFile.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouMemoAmtPrcl * Val.Val(txtExcRate.Text), 3);
                        break;
                    case "MEMOPRICEPERCARAT":
                        double DouCarat1 = Val.Val(DRow["CARAT"]);
                        double DouPricePerCarat1 = Val.Val(DRow["MEMOPRICEPERCARAT"]);
                        DTabMemoDetailParcelFile.Rows[e.RowHandle]["MEMOAMOUNT"] = Math.Round(DouCarat1 * DouPricePerCarat1, 3);
                        double DouMemoAmtPrcl1 = Val.Val(DRow["MEMOAMOUNT"]);
                        DTabMemoDetailParcelFile.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat1 * Val.Val(txtExcRate.Text), 3);
                        DTabMemoDetailParcelFile.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouMemoAmtPrcl1 * Val.Val(txtExcRate.Text), 3);
                        break;
                }
            }

            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void lblExportSampleFileOSStone_Click(object sender, EventArgs e)
        {
            try
            {
                string StrFilePathDestination = "";

                StrFilePathDestination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\OutSideStoneFormat" + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.Day.ToString() + ".xlsx";
                if (File.Exists(StrFilePathDestination))
                {
                    File.Delete(StrFilePathDestination);
                }
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\Format\\OutSideStoneFormat.xlsx", StrFilePathDestination);

                System.Diagnostics.Process.Start(StrFilePathDestination, "CMD");
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public void BulkPriceCalculation()
        {
            try
            {
                int IntPcs = 0;
                double DouCarat = 0;
                double DouCostRapAmt = 0;
                double DouCostAmt = 0;
                double DouSaleRapAmt = 0;
                double DouSaleAmt = 0;
                double DouMemoRapAmt = 0;
                double DouMemoAmt = 0;

                for (int IntI = 0; IntI < GrdDetail.RowCount; IntI++)
                {
                    DataRow DRow = GrdDetail.GetDataRow(IntI);

                    IntPcs = IntPcs + Val.ToInt(DRow["PCS"]);
                    DouCarat = DouCarat + Val.Val(DRow["CARAT"]);

                    DouCostRapAmt = DouCostRapAmt + (Val.Val(DRow["COSTRAPAPORT"]) * Val.Val(DRow["CARAT"]));
                    DouCostAmt = DouCostAmt + (Val.Val(DRow["COSTPRICEPERCARAT"]) * Val.Val(DRow["CARAT"]));

                    DouSaleRapAmt = DouSaleRapAmt + (Val.Val(DRow["SALERAPAPORT"]) * Val.Val(DRow["CARAT"]));
                    DouSaleAmt = DouSaleAmt + Val.Val(DRow["SALEAMOUNT"]);

                    DouMemoRapAmt = DouMemoRapAmt + (Val.Val(DRow["MEMORAPAPORT"]) * Val.Val(DRow["CARAT"]));
                    DouMemoAmt = DouMemoAmt + Val.Val(DRow["MEMOAMOUNT"]);
                }

                txtSaleDisc.Text = Val.Format(((DouMemoRapAmt - DouMemoAmt) / DouMemoRapAmt) * -100, "########0.00");
                txtStockDisc.Text = Val.Format(((DouSaleRapAmt - DouSaleAmt) / DouSaleRapAmt) * -100, "########0.00");
                txtSRTDisc.Text = Val.Format(((DouCostRapAmt - DouCostAmt) / DouCostRapAmt) * -100, "########0.00");

                txtSalePerCts.Text = Val.Format(DouMemoAmt / DouCarat, "########0.000");
                txtStockPerCts.Text = Val.Format(DouSaleAmt / DouCarat, "########0.000");
                txtSRTPerCts.Text = Val.Format(DouCostAmt / DouCarat, "########0.000");

                txtSaleAmount.Text = Val.Format(DouMemoAmt, "########0.00");
                txtStockAmount.Text = Val.Format(DouSaleAmt, "########0.00");
                txtSRTAmt.Text = Val.Format(DouCostAmt, "########0.00");

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        public void CalculationNewInvoiceEntry()
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

                if (mStrStockType == "PARCEL" || mStrStockType == "ALL")
                {
                    for (int Int = 0; Int < GrdSummury.RowCount; Int++)
                    {
                        DataRow DR = GrdSummury.GetDataRow(Int);

                        IntPcs = IntPcs + Val.ToInt(DR["TOTALPCS"]);
                        DouCarat = DouCarat + Val.Val(DR["TOTALCARAT"]);


                        //DouSaleRapAmt = DouSaleRapAmt + (Val.Val(DR["SALERAPAPORT"]) * Val.Val(DR["CARAT"]));
                        if (cmbBillType.Text.ToUpper() == "RUPEESBILL")
                        {
                            DouMemoTotalAmtFE = DouMemoTotalAmtFE + Val.Val(DR["MEMOTOTALAMOUNTFE"]);
                            DouSaleAmt = DouMemoTotalAmtFE;
                        }
                        else
                        {
                            DouSaleAmt = DouSaleAmt + Val.Val(DR["MEMOTOTALAMOUNT"]);
                        }
                        DouMemoTotalAmt = DouMemoTotalAmt + Val.Val(DR["MEMOTOTALAMOUNT"]);

                        for (int IntI = 0; IntI < GrdDetail.RowCount; IntI++)
                        {
                            DataRow DRow = GrdDetail.GetDataRow(IntI);

                            if (!ChkUpdExport.Checked)
                            {
                                DRow["EXPINVOICERATE"] = DRow["MEMOPRICEPERCARAT"];
                                DRow["EXPINVOICEAMT"] = DRow["MEMOAMOUNT"];
                                DRow["EXPINVOICERATEFE"] = DRow["FMEMOPRICEPERCARAT"];
                                DRow["EXPINVOICEAMTFE"] = DRow["FMEMOAMOUNT"];
                            }

                            DouExpInvAmt = DouExpInvAmt + Val.Val(DRow["EXPINVOICEAMT"]);
                            DouExpInvAmtFE = DouExpInvAmtFE + Val.Val(DRow["EXPINVOICEAMTFE"]);
                            DouExpInvAmt = DouExpInvAmt + Val.Val(DRow["EXPINVOICEAMT"]);
                            DouExpInvAmtFE = DouExpInvAmtFE + Val.Val(DRow["EXPINVOICEAMTFE"]);
                        }

                    }

                }
                else
                {
                    for (int IntI = 0; IntI < GrdSummuryMNL.RowCount; IntI++)
                    {
                        DataRow DRow = GrdSummuryMNL.GetDataRow(IntI);

                        IntPcs = IntPcs + Val.ToInt(DRow["PCS"]);
                        DouCarat = DouCarat + Val.Val(DRow["CARAT"]);


                        DouSaleRapAmt = DouSaleRapAmt + (Val.Val(DRow["SALERAPAPORT"]) * Val.Val(DRow["CARAT"]));
                        if (cmbBillType.Text.ToUpper() == "RUPEESBILL")
                        {
                            DouMemoTotalAmtFE = DouMemoTotalAmtFE + Val.Val(DRow["FMEMOAMOUNT"]);
                            DouSaleAmt = DouMemoTotalAmtFE;
                        }
                        else
                        {
                            DouSaleAmt = DouSaleAmt + Math.Round(Val.Val(DRow["MEMOAMOUNT"]), 2);
                        }
                        DouMemoTotalAmt = DouMemoTotalAmt + Val.Val(DRow["MEMOAMOUNT"]);
                    }
                }

                txtTotalPcs.Text = Val.Format(IntPcs, "########0");
                txtTotalCarat.Text = Val.Format(DouCarat, "########0.00");

                if (DouSaleRapAmt != 0)
                    lblTotalAvgDisc.Text = Val.Format(((DouSaleRapAmt - DouSaleAmt) / DouSaleRapAmt) * 100, "########0.00");
                else
                    lblTotalAvgDisc.Text = "0.00";

                lblTotalAmount.Text = Val.Format(DouSaleAmt, "########0.00");
                lblTotalAvgRate.Text = Val.Format(DouSaleAmt / DouCarat, "########0.00");

                if ((mFormType != FORMTYPE.PURCHASEISSUE) && (DouSaleRapAmt != 0)) // Cndtn Changed & add Else if : Pinali : 29-08-2019
                    txtMemoAvgDisc.Text = Val.Format(((DouSaleRapAmt - DouMemoTotalAmt) / DouSaleRapAmt) * 100, "########0.00");
                else if ((mFormType == FORMTYPE.PURCHASEISSUE) && (DouSaleRapAmt != 0)) // For Purchase Invoice
                    txtMemoAvgDisc.Text = Val.Format(((DouSaleRapAmt - DouSaleAmt) / DouSaleRapAmt) * 100, "########0.00");
                else
                    txtMemoAvgDisc.Text = "0.00";

                txtMemoAmount.Text = Val.Format(DouSaleAmt, "########0.00");
                txtGrossAmount.Text = Val.Format(DouSaleAmt, "########0.00");
                txtGrossAmountFE.Text = Val.Format(Val.Val(DouSaleAmt) * Val.Val(txtExcRate.Text), "########0.00");
                txtMemoAvgRate.Text = Val.Format(DouSaleAmt / DouCarat, "########0.00");

                //commnet by shiv RS VALUE ROUND OFF
                //if (cmbAccType.SelectedIndex == 4)
                //{
                //    double GrossAmountFEROUND = Math.Round(Val.Val(txtGrossAmountFE.Text), 2);
                //    txtGrossAmountFE.Text = Val.Format(GrossAmountFEROUND, "########0.00");
                //}
                //else
                //{
                //    double GrossAmountFEROUND = Math.Round(Val.Val(txtGrossAmountFE.Text), 0);
                //    txtGrossAmountFE.Text = Val.Format(GrossAmountFEROUND, "########0.00");
                //}

                double GrossAmountFEROUND = Math.Round(Val.Val(txtGrossAmountFE.Text), 0);
                txtGrossAmountFE.Text = Val.Format(GrossAmountFEROUND, "########0.00");

                double DouDiscAmt = Math.Round(Val.Val(txtDiscAmount.Text), 4);
                double DouGSTAmt = Math.Round(Val.Val(txtGSTAmount.Text), 4);
                double DouInsAmt = Math.Round(Val.Val(txtInsuranceAmount.Text), 4);
                double DouShipAmt = Math.Round(Val.Val(txtShippingAmount.Text), 4);
                double DouIGSTAmt = Math.Round(Val.Val(txtIGSTAmount.Text), 0);
                double DouCGSTAmt = Math.Round(Val.Val(txtCGSTAmount.Text), 0);
                double DouSGSTAmt = Math.Round(Val.Val(txtSGSTAmount.Text), 0);
                double DouTCSAmt = Math.Round(Val.Val(txtTCSAmount.Text), 0);

                double ROUNDDouIGSTAmt = ToDouble50Round(DouIGSTAmt);
                double ROUNDDouCGSTAmt = ToDouble50Round(DouCGSTAmt);
                double ROUNDDouSGSTAmt = ToDouble50Round(DouSGSTAmt);
                double ROUNDDouTCSAmt = ToDouble50Round(DouTCSAmt);

                txtDiscAmount.Text = Val.Format(DouDiscAmt, "########0.00");
                txtInsuranceAmount.Text = Val.Format(DouInsAmt, "########0.00");
                txtShippingAmount.Text = Val.Format(DouShipAmt, "########0.00");
                txtGSTAmount.Text = Val.Format(DouGSTAmt, "########0.00");

                txtIGSTAmount.Text = Val.Format(ROUNDDouIGSTAmt, "########0.00");
                txtCGSTAmount.Text = Val.Format(ROUNDDouCGSTAmt, "########0.00");
                txtSGSTAmount.Text = Val.Format(ROUNDDouSGSTAmt, "########0.00");
                txtTCSAmount.Text = Val.Format(ROUNDDouTCSAmt, "########0.00");

                if (cmbBillType.Text.ToUpper() == "DOLLARBILL" || cmbBillType.Text.ToUpper() == "EXPORT" || cmbBillType.Text.ToUpper() == "NET CONSIGNMENT")
                {
                    //double DouNetAmt = Math.Round((Val.Val(txtGrossAmount.Text) + DouIGSTAmt + DouCGSTAmt + DouSGSTAmt + DouInsAmt + DouShipAmt + DouGSTAmt + DouTCSAmt) - DouDiscAmt, 3);
                    //double DouNetAmt = Math.Round((Val.Val(txtGrossAmount.Text) + DouInsAmt + DouShipAmt + DouGSTAmt + DouTCSAmt) - DouDiscAmt, 3);  Commnet by shiv 29-06-2022
                    double DouNetAmt = Math.Round((Val.Val(txtGrossAmount.Text) + DouInsAmt + DouShipAmt + DouGSTAmt - DouTCSAmt) - DouDiscAmt, 3);
                    txtNetAmount.Text = Val.Format(DouNetAmt, "########0.00");
                }
                else
                {
                    //double DouNetAmt = Math.Round((Val.Val(txtGrossAmount.Text) + DouIGSTAmt + DouCGSTAmt + DouSGSTAmt + DouInsAmt + DouShipAmt + DouGSTAmt + DouTCSAmt) - DouDiscAmt, 0); Commnet by shiv 29-06-2022
                    double DouNetAmt = Math.Round((Val.Val(txtGrossAmount.Text) + DouIGSTAmt + DouCGSTAmt + DouSGSTAmt + DouInsAmt + DouShipAmt + DouGSTAmt - DouTCSAmt) - DouDiscAmt, 0);
                    txtNetAmount.Text = Val.Format(DouNetAmt, "########0.00");
                }

                double DouDiscAmtFE = Math.Round(Val.Val(txtDiscAmountFE.Text), 2);
                double DouGSTAmtFE = Math.Round(Val.Val(txtGSTAmountFE.Text), 2);

                double DouInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouInsAmt, 2);
                double DouShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouShipAmt, 2);

                double DouIGSTAmtFE = Math.Round(Val.Val(txtIGSTAmountFE.Text), 0);
                double DouCGSTAmtFE = Math.Round(Val.Val(txtCGSTAmountFE.Text), 0);
                double DouSGSTAmtFE = Math.Round(Val.Val(txtSGSTAmountFE.Text), 0);
                double DouTCSAmtFE = Math.Round(Val.Val(txtTCSAmountFE.Text), 0);

                double ROUNDDouIGSTAmtFE = ToDouble50Round(DouIGSTAmtFE);
                double ROUNDDouCGSTAmtFE = ToDouble50Round(DouCGSTAmtFE);
                double ROUNDDouSGSTAmtFE = ToDouble50Round(DouSGSTAmtFE);
                double ROUNDDouTCSAmtFE = ToDouble50Round(DouTCSAmtFE);

                txtDiscAmountFE.Text = Val.Format(DouDiscAmtFE, "########0.00");
                txtInsuranceAmountFE.Text = Val.Format(DouInsAmtFE, "########0.00");
                txtShippingAmountFE.Text = Val.Format(DouShipAmtFE, "########0.00");
                txtGSTAmountFE.Text = Val.Format(DouGSTAmtFE, "########0.00");

                txtIGSTAmountFE.Text = Val.Format(ROUNDDouIGSTAmtFE, "########0.00");
                txtCGSTAmountFE.Text = Val.Format(ROUNDDouCGSTAmtFE, "########0.00");
                txtSGSTAmountFE.Text = Val.Format(ROUNDDouSGSTAmtFE, "########0.00");
                txtTCSAmountFE.Text = Val.Format(ROUNDDouTCSAmtFE, "########0.00");

                //Commnet by shiv 29-06-2022
                //double DouNetAmtFEWithRoundOff = (Val.Val(txtGrossAmountFE.Text) + DouIGSTAmtFE + DouCGSTAmtFE + DouSGSTAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE + DouTCSAmtFE) - DouDiscAmtFE;
                double DouNetAmtFEWithRoundOff = (Val.Val(txtGrossAmountFE.Text) + DouIGSTAmtFE + DouCGSTAmtFE + DouSGSTAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE - DouTCSAmtFE) - DouDiscAmtFE;
                double DouNetAmtFE = 0;

                if (cmbAccType.SelectedIndex == 2 || cmbAccType.SelectedIndex == 4 || cmbAccType.SelectedIndex == 3) //add shiv 22-06-2022
                {
                    //commnet by shiv 29-06-2022
                    //DouNetAmtFE = Math.Round((Val.Val(txtGrossAmountFE.Text) + DouIGSTAmtFE + DouCGSTAmtFE + DouSGSTAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE + DouTCSAmtFE) - DouDiscAmtFE, 2);
                    DouNetAmtFE = Math.Round((Val.Val(txtGrossAmountFE.Text) + DouIGSTAmtFE + DouCGSTAmtFE + DouSGSTAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE - DouTCSAmtFE) - DouDiscAmtFE, 2);
                    txtNetAmountFE.Text = Val.Format(DouNetAmtFE, "########0.00");
                }
                else
                {
                    //commnet by shiv 29-06-2022
                    //DouNetAmtFE = Math.Round((Val.Val(txtGrossAmountFE.Text) + DouIGSTAmtFE + DouCGSTAmtFE + DouSGSTAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE + DouTCSAmtFE) - DouDiscAmtFE, 0, MidpointRounding.AwayFromZero);
                    DouNetAmtFE = Math.Round((Val.Val(txtGrossAmountFE.Text) + DouIGSTAmtFE + DouCGSTAmtFE + DouSGSTAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE - DouTCSAmtFE) - DouDiscAmtFE, 0, MidpointRounding.AwayFromZero);
                    txtNetAmountFE.Text = Val.Format(DouNetAmtFE, "########0.00");
                }

                if (DouNetAmtFEWithRoundOff > DouNetAmtFE)
                {
                    txtRoundOffAmountFE.Text = Val.ToString(DouNetAmtFEWithRoundOff - DouNetAmtFE);
                    CmbRoundOff.SelectedIndex = 1;
                }
                else
                {
                    txtRoundOffAmountFE.Text = Val.ToString(DouNetAmtFE - DouNetAmtFEWithRoundOff);
                    CmbRoundOff.SelectedIndex = 0;
                }


                if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE)
                {
                    txtBrokerAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 0));
                }
                else
                {
                    txtBrokerAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtNetAmountFE.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 0));
                }

                txtAdatAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtNetAmountFE.Text) * Val.Val(txtAdatPer.Text)) / 100, 0));
                txtAdatAmt.Text = Val.ToString(Math.Round((Val.Val(txtNetAmount.Text) * Val.Val(txtAdatPer.Text)) / 100, 3));
                txtBrokerAmt.Text = Val.ToString(Math.Round((Val.Val(txtNetAmount.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 3));
                txtStkAmtFE.Text = Val.Format(Val.Val(txtNetAmountFE.Text) - (Val.Val(txtBrokerAmtFE.Text) + Val.Val(txtAdatAmtFE.Text)), "########0.00");

                if (cmbAccType.SelectedIndex == 2 || cmbAccType.SelectedIndex == 4 || cmbAccType.SelectedIndex == 3)
                {
                    if (txtBrokerExcRate.Text != string.Empty)
                    {
                        //txtBrokerAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtBrokerAmt.Text) * Val.Val(txtBrokerExcRate.Text)), 0)); Commnet by shiv 14-09-2022
                        double NetAmtFE = Val.Val(Math.Round((Val.Val(txtNetAmount.Text) * Val.Val(txtBrokerExcRate.Text)), 0));
                        txtBrokerAmtFE.Text = Val.ToString(Math.Round((Val.Val(NetAmtFE) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 0));
                    }
                }

                if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE)
                {
                    if (rdBroGST.SelectedIndex == 0 || rdoBroTDS.SelectedIndex == 0)
                    {
                        txtBroTAmt.Text = Val.Format(Math.Round(Val.Val(txtBrokerAmtFE.Text) - Val.Val(txtBroTdsRs.Text) + Val.Val(txtBroCgstAmt.Text) + Val.Val(txtBroSgstAmt.Text) + Val.Val(txtBroIgstAmt.Text), 2), "########0.00");
                    }
                }
                if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
                {
                    BulkPriceCalculation();
                }
            }
            catch (Exception ex)

            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void repTxtFMemoPricePerCarat_Validating(object sender, CancelEventArgs e)
        {
            if (GrdSummuryMNL.FocusedRowHandle < 0)
                return;

            try
            {
                GrdSummuryMNL.PostEditor();

                double DouCarat = 0;
                double DouCostDiscount = 0;
                double DouCostPricePerCarat = 0;
                double DouCostRapaport = 0;
                double DouCostAmount = 0;
                double DouCostFEAmount = 0;

                DataRow DR = GrdSummuryMNL.GetFocusedDataRow();
                DouCarat = Val.Val(GrdSummuryMNL.GetFocusedRowCellValue("CARAT"));
                DouCostPricePerCarat = Val.Val(GrdSummuryMNL.EditingValue);

                DouCostRapaport = Val.Val(GrdSummuryMNL.GetFocusedRowCellValue("MEMORAPAPORT"));

                if (DouCostRapaport != 0)
                    DouCostDiscount = Math.Round((DouCostPricePerCarat - DouCostRapaport) / DouCostRapaport * 100, 2);
                else
                    DouCostDiscount = 0;
                if (Val.ToString(cmbBillType.SelectedItem).ToUpper() == "DOLLARBILL")
                {
                    DouCostAmount = Math.Round(DouCostPricePerCarat * DouCarat, 2);
                    DouCostFEAmount = Math.Round(DouCostAmount * Val.Val(txtExcRate.Text), 2);
                }
                else if (Val.ToString(cmbBillType.SelectedItem).ToUpper() == "RUPEESBILL")
                {
                    DouCostFEAmount = Math.Round(DouCostPricePerCarat * DouCarat, 2);
                    if (Val.Val(txtExcRate.Text) != 0)
                    {
                        DouCostAmount = Math.Round(DouCostFEAmount / Val.Val(txtExcRate.Text), 2);
                    }
                    else
                    {
                        DouCostAmount = 0;
                    }

                    //DR["MEMOPRICEPERCARAT"] = Math.Round(DouCostAmount / DouCarat, 2);
                    DR["FMEMOPRICEPERCARAT"] = Math.Round(DouCostFEAmount / DouCarat, 2);
                }

                DR["MEMODISCOUNT"] = DouCostDiscount;
                //DR["MEMOAMOUNT"] = DouCostAmount;
                DR["FMEMOAMOUNT"] = DouCostFEAmount;

                //Calculation();
                //CalculationNew();
                CalculationNewInvoiceEntry();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public void GetSummuryDetailForGridSaleInvoiceEntry()
        {
            try
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
                    double DouTotalAmountFE = 0.000;
                    double DouTotalAvgDiscFE = 0.00;
                    double DouTotalRateFE = 0.00;

                    for (int IntI = 0; IntI < GrdSummuryMNL.RowCount; IntI++)
                    {
                        DataRow DRow = GrdSummuryMNL.GetDataRow(IntI);

                        DouTotalPCS = DouTotalPCS + Val.ToInt(DRow["PCS"]);
                        DouTotalCarat = Math.Round((DouTotalCarat + Val.Val(DRow["CARAT"])), 2);

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

                    DouTotalRateFE = Math.Round((DouTotalAmountFE / DouTotalCarat), 3);
                    DouTotalAvgDiscFE = Math.Round(Val.Val(((DouTotalAmountFE - DouTotalRapaport) / DouTotalRapaport) * 100), 3);
                    DouTotalMemoRateFE = Math.Round((DouTotalMemoAmountFE / DouTotalCarat), 3);
                    DouTotalMemoAvgDiscFE = Math.Round(Val.Val(((DouTotalMemoAmountFE - DouTotalRapaport) / DouTotalRapaport) * 100), 3);

                    if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE)
                    {
                        if (cmbAccType.SelectedIndex == 2 || cmbAccType.SelectedIndex == 3) //add shiv 22-06-2022
                        {
                            DouTotalMemoAmountFE = Math.Round(DouTotalMemoAmountFE, 3);
                            DouTotalMemoRateFE = Math.Round((DouTotalMemoAmountFE / DouTotalCarat), 3);

                            DouTotalMemoAmount = Math.Round(DouTotalMemoRate, 2) * DouTotalCarat;
                            DouTotalMemoRate = Math.Round(DouTotalMemoRate, 2);
                        }
                        else if (cmbAccType.SelectedIndex == 4)
                        {
                            DouTotalMemoAmount = Math.Round(DouTotalMemoRate, 2) * DouTotalCarat;
                            DouTotalMemoRate = Math.Round(DouTotalMemoRate, 2);
                        }
                        else
                        {
                            DouTotalMemoAmountFE = Math.Round(DouTotalMemoRateFE, 0) * DouTotalCarat;
                            DouTotalMemoAmountFE = Math.Round(DouTotalMemoAmountFE, 0);
                            DouTotalMemoRateFE = Math.Round((DouTotalMemoAmountFE / DouTotalCarat), 0);
                            DouTotalMemoRateFE = Math.Round(DouTotalMemoRateFE, 0);
                        }
                    }
                    else
                    {
                        DouTotalMemoAmountFE = Math.Round(DouTotalMemoAmountFE, 3);
                        DouTotalMemoRateFE = Math.Round((DouTotalMemoAmountFE / DouTotalCarat), 3);
                    }


                    StrDescription = "Cut & Polished Diamond";

                    if (DTabMemoSummury.Rows.Count > 0)
                    {
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
                        DTabMemoSummury.Rows[0]["MEMOTOTALAMOUNTFE"] = Val.Format(DouTotalMemoAmountFE, "########0.000");
                        DTabMemoSummury.Rows[0]["MEMOAVGDISCFE"] = DouTotalMemoAvgDiscFE;
                    }
                    else
                    {
                        if (mFormType == FORMTYPE.SALEINVOICE)
                        {
                            CalculationNew();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void AutoGstCalculationNew()
        {
            if (PnlGSTDetail.Visible == true && txtBCountry.Text == "INDIA")
            {
                string CGSTVALUE = "", SGSTVALUE = "", IGSTVALUE = "";
                //DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "CGST");
                //CGSTVALUE = Val.ToString(DtLedger.Rows[0]["GSTPER"]);
                //DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "SGST");
                //SGSTVALUE = Val.ToString(DtLedger.Rows[0]["GSTPER"]);
                //DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "IGST");
                //IGSTVALUE = Val.ToString(DtLedger.Rows[0]["GSTPER"]);

                string StrTxtBState = txtBState.Text.ToUpper();
                if (StrTxtBState == "MAHARASHTRA")
                {
                    txtCGSTPer.Text = "0.00";
                    txtSGSTPer.Text = "0.00";
                    txtIGSTPer.Text = "0.00";
                    txtIGSTAmount.Text = "0.00";
                    txtIGSTAmountFE.Text = "0.00";
                    txtBroCgstPer.Text = "0.00";
                    txtBroCgstAmt.Text = "0.00";
                    txtBroSgstPer.Text = "0.00";
                    txtBroSgstAmt.Text = "0.00";
                    txtBroIgstPer.Text = "0.00";
                    txtBroIgstAmt.Text = "0.00";

                    if (mFormType == FORMTYPE.SALEINVOICE)
                    {
                        txtCGSTPer.Text = CGSTVALUE; //cmnt Cz Not Consider in order  bill
                        txtSGSTPer.Text = SGSTVALUE; //cmnt Cz Not Consider in order  bill
                        txtBroCgstPer.Text = "9";
                        txtBroSgstPer.Text = "9";
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
                    txtIGSTPer.Text = "0.00";
                    txtBroCgstPer.Text = "0.00";
                    txtBroCgstAmt.Text = "0.00";
                    txtBroSgstPer.Text = "0.00";
                    txtBroSgstAmt.Text = "0.00";
                    txtBroIgstPer.Text = "0.00";
                    txtBroIgstAmt.Text = "0.00";

                    if (mFormType == FORMTYPE.SALEINVOICE)
                    {
                        txtIGSTPer.Text = IGSTVALUE; //cmnt Cz Not Consider in order  bill
                        txtBroIgstPer.Text = "18";
                    }
                }
            }
        }

        private void btnBrokerUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection cn_T;

                cn_T = new SqlConnection(BOConfiguration.ConnectionString);
                if (cn_T.State == ConnectionState.Open) { cn_T.Close(); }
                cn_T.Open();

                if (Global.Confirm("Are You Sure For Update Entry") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                MemoEntryProperty Property = new MemoEntryProperty();

                if (lblMode.Text == "Edit Mode")
                {
                    if (cmbAccType.SelectedIndex == 1)
                    {
                        if (Val.Val(txtTCSAmountFE.Text) == 0.00)
                        {

                            DataTable DTMAX = new BOMST_Ledger().GetLedgerDataForTDSCreditLimit(Val.ToString(txtBillingParty.Tag));
                            double DouMemoAmount = Val.Val(txtGrossAmountFE.Text);
                            double DouLedgerTotAmt = Val.Val(lblCrdlimit.Text);
                            if (DouLedgerTotAmt > dblTCSAmount)
                            {
                                if (DTMAX.Rows.Count > 0)
                                {
                                    IS_MAXLIMIT = Val.ToBoolean(DTMAX.Rows[0]["IS_Maxlimit"]);
                                }
                                if (IS_MAXLIMIT == true)
                                {
                                    if (DouLedgerTotAmt > dblTCSAmount)
                                    {
                                        double dblDiff = dblTCSAmount - (DouLedgerTotAmt);
                                        if (dblDiff > 0)
                                        {
                                            txtTCSCalcAmt.Text = "0.00";
                                        }
                                        else
                                        {
                                            Global.Message("TDS Limit Exists Fifty lakh Above Please Add Tds.......");
                                            txtTCSCalcAmt.Text = Val.ToString(Math.Abs(dblDiff));
                                        }
                                    }
                                }
                                else
                                {
                                    Global.Message("TDS Limit Exists Fifty lakh Above Please Add Tds.......");
                                    txtTCSCalcAmt.Text = Val.ToString(DouMemoAmount);
                                }
                            }
                            else
                            {
                                txtTCSCalcAmt.Text = "0.00";
                            }
                        }
                    }
                    Property.MEMO_ID = Val.ToString(lblMemoNo.Tag);
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

                    Property.BASEBROKERAGEEXCRATE = Val.ToDouble(txtBrokerExcRate.Text);
                    Property.ADATEXCRATE = Val.ToDouble(txtAdatExcRate.Text);
                    Property.GRFREIGHT = Val.ToDouble(txtGRFreight.Text);

                    Property.BROTDSPER = Val.ToDouble(txtBroTdsPer.Text);
                    Property.BROTDSRS = Val.ToDouble(txtBroTdsRs.Text);
                    Property.BROTOTALAMT = Val.ToDouble(txtBroTAmt.Text);
                    Property.IS_BROTDS = Val.ToBoolean(rdoBroTDS.Text);
                    Property.BROCGSTPER = Val.ToDouble(txtBroCgstPer.Text);
                    Property.BROCGSTRS = Val.ToDouble(txtBroCgstAmt.Text);
                    Property.BROSGSTPER = Val.ToDouble(txtBroSgstPer.Text);
                    Property.BROSGSTRS = Val.ToDouble(txtBroSgstAmt.Text);
                    Property.BROIGSTPER = Val.ToDouble(txtBroIgstPer.Text);
                    Property.BROIGSTRS = Val.ToDouble(txtBroIgstAmt.Text);
                    Property.IS_BROGST = Val.ToBoolean(rdBroGST.Text);

                    Property.TCSPER = Val.Val(txtTCSPer.Text);
                    Property.TCSAMOUNT = Val.Val(txtTCSAmount.Text);
                    Property.FTCSAMOUNT = Val.Val(txtTCSAmountFE.Text);
                    Property.FNETAMOUNT = Val.Val(txtNetAmountFE.Text);

                    Property.TERMS_ID = Val.ToInt32(txtTerms.Tag);
                    Property.TERMSDAYS = Val.ToInt32(txtTermsDays.Text);
                    Property.TERMSPER = 0;
                    Property.TERMSDATE = Val.SqlDate(DTPTermsDate.Text);

                    Property = ObjMemo.UpdateBroker(cn_T, Property);
                    Global.Message(Property.ReturnMessageDesc);
                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        cn_T.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtShippingAmount_Validated(object sender, EventArgs e)
        {
            try
            {
                if (txtShippingAmount.Focused)
                {
                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        txtShippingPer.Text = Math.Round((Val.Val(txtShippingAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                        txtShippingAmountFE.Text = Val.Format(Math.Round(Val.Val(txtShippingAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                    else
                    {
                        txtShippingPer.Text = "0.000";
                        txtShippingAmount.Text = "0.000";
                        txtShippingAmountFE.Text = "0.000";
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                    {
                        CalculationNewInvoiceEntry();
                        GetSummuryDetailForGridSaleInvoiceEntry();
                    }
                    else
                    {
                        CalculationNew();
                        GetSummuryDetailForGrid();
                    }


                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtShippingAmountFE_Validated(object sender, EventArgs e)
        {
            try
            {
                if (txtShippingAmount.Focused)
                {
                    if (Val.Val(txtGrossAmountFE.Text) != 0)
                    {
                        double DouShippingAmount = 0;
                        DouShippingAmount = Math.Round(Val.Val(txtShippingAmountFE.Text) / Val.Val(txtExcRate.Text), 3);

                        txtShippingAmount.Text = Val.Format(DouShippingAmount, "########0.000");
                        txtShippingPer.Text = Math.Round((Val.Val(DouShippingAmount) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                    }
                    else
                    {
                        txtShippingPer.Text = "0.000";
                        txtShippingAmount.Text = "0.000";
                        txtShippingAmountFE.Text = "0.000";
                    }
                    if (MenuTagName == "SALEINVOICEENTRY")
                    {
                        CalculationNewInvoiceEntry();
                        GetSummuryDetailForGridSaleInvoiceEntry();
                    }
                    else
                    {
                        CalculationNew();
                        GetSummuryDetailForGrid();
                    }

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void btnConsRetrun_Click(object sender, EventArgs e)
        {
            try
            {

                //if (txtBillingParty.Text.Length == 0)
                //{
                //    Global.Message("Billing Party Is Required");
                //    txtBillingParty.Focus();
                //    return;
                //}
                //if (txtBCountry.Text.Length == 0)
                //{
                //    Global.Message("Billing Country Is Required");
                //    txtBCountry.Focus();
                //    return;
                //}
                //if (txtSellerName.Text.Length == 0)
                //{
                //    Global.Message("Seller Name Is Required");
                //    txtSellerName.Focus();
                //    return;
                //}
                //if (txtTerms.Text.Length == 0)
                //{
                //    Global.Message("Terms Is Required");
                //    txtTerms.Focus();
                //    return;
                //}

                this.Cursor = Cursors.WaitCursor;
                DataTable DtabInvoiceDetail = new DataTable();
                // DtabInvoiceDetail = DsLiveStock.Tables[0];
                // DtabInvoiceDetail = DtReturnStoneList;


                mFormType = FORMTYPE.CONSIGNMENTISSUE;

                this.Cursor = Cursors.Default;
                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);

                if (mFormType == FORMTYPE.PURCHASEISSUE)
                    FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.PURCHASERETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                else if (mFormType == FORMTYPE.MEMOISSUE)
                    FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.MEMORETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                else if (mFormType == FORMTYPE.ORDERCONFIRM)
                    FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.ORDERCONFIRMRETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                else if (mFormType == FORMTYPE.SALEINVOICE)
                    FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALESDELIVERYRETURN, DtabInvoiceDetail, DTabMemo, mStrStockType, Val.ToString(lblMemoNo.Tag));
                else if (mFormType == FORMTYPE.LABISSUE)
                    FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.LABRETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                else if (mFormType == FORMTYPE.CONSIGNMENTISSUE)
                    //FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.CONSIGNMENTRETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                    FrmMemoEntry.ShowForm(Val.ToString(lblMemoNo.Tag), "SALEINVOICEENTRY", 1);
                else if (mFormType == FORMTYPE.HOLD)
                    FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.RELEASE, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));

                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }

        private void btnAccClose_Click(object sender, EventArgs e)
        {
            try
            {
                PnlAccPrint.Visible = false;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void FrmMemoEntry_Load(object sender, EventArgs e)
        {
            try
            {
                if (MenuTagName == "SALEINVOICEENTRY" || mFormType == FORMTYPE.SALEINVOICE)
                {
                    AutoGstCalculationNew();
                    loadApiSetting();
                    loadApiLoginDetail();
                }
                //Added by Daksha on 17/01/2023
                if (Val.ToString(BOConfiguration.gEmployeeProperty.LEDGER_ID).ToUpper() == "B0703EEC-A579-EC11-A8B7-ACB57D1F87CF")
                {
                    btnViewOrderByStockiest.Visible = true;
                    GrdDetail.Columns["BoxName"].Visible = true;
                }
                else
                {
                    btnViewOrderByStockiest.Visible = false;
                    GrdDetail.Columns["BoxName"].Visible = false;
                }
                //End as Daksha

                string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDetail.Name);

                if (Str != "")
                {
                    byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                    MemoryStream stream = new MemoryStream(byteArray);
                    GrdDetail.RestoreLayoutFromStream(stream);
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        #region ::"Thrid Party"::

        public void loadApiSetting()
        {
            GspName = eInvSession.eInvApiSetting.GSPName;
            AspUserId = eInvSession.eInvApiSetting.AspUserId;
            AspPassword = eInvSession.eInvApiSetting.AspPassword;
            ClientId = eInvSession.eInvApiSetting.client_id;
            ClientSecret = eInvSession.eInvApiSetting.client_secret;
            AuthUrl = eInvSession.eInvApiSetting.AuthUrl;
            BaseUrl = eInvSession.eInvApiSetting.BaseUrl;
            EwbBaseUrl = eInvSession.eInvApiSetting.EwbByIRN;
            CancelEWB = eInvSession.eInvApiSetting.CancelEwbUrl;
        }

        private void chkIsTdsAmt_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkIsTdsAmt.Checked == true)
                {
                    txtTCSAmountFE.ReadOnly = false;
                }
                else
                {
                    txtTCSAmountFE.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        //Added by Daksha on 17/01/2023
        private void btnViewOrderByStockiest_Click(object sender, EventArgs e)
        {
            try
            {
                int RetVal = 0;
                if (Val.ToString(txtJangedNo.Tag) != "")
                {
                    RetVal = new BOMST_FormPermission().Update_IsStockiest_View_Notification(Val.ToString(txtJangedNo.Tag));
                }
                if (RetVal == -1)
                {
                    Global.Message("This Order is Already Viewed.");
                }
                else if (RetVal == 1)
                {
                    if (Global.Confirm("This Order is Marked as Viewed, Are You Sure to Close?") == DialogResult.Yes)
                    {
                        this.Close();
                    }
                }
                else
                {
                    Global.MessageError("Error While Update Flag");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        //End as Daksha

        public void loadApiLoginDetail()
        {
            UserName = eInvSession.eInvApiLoginDetails.UserName;
            Password = eInvSession.eInvApiLoginDetails.Password;
            Gstin = eInvSession.eInvApiLoginDetails.GSTIN;
            AppKey = eInvSession.eInvApiLoginDetails.AppKey;
            AuthToken = eInvSession.eInvApiLoginDetails.AuthToken;
            Sek = eInvSession.eInvApiLoginDetails.Sek;
            TokenExp = eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp.ToString();
        }

        #endregion
        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            try
            {
                int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDetail.Name);
                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Deleted");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtBackAddLess_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Comment by Gunjan:19/11/2024 As Per discussion with brijeshbhai text change par change thase discount 
                //if (e.KeyCode == Keys.Enter)
                //{
                //    BtnModifyPrice_Click(null, null);
                //}
                //end as gunjan
            }
            catch (Exception Ex)
            {

            }
        }

        private void ChkApprovedOrder_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BtnPrintDrop_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void IsConsiderBillingParty_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtShippingParty.Tag = Val.ToString(txtBillingParty.Tag);
                txtShippingParty.Text = Val.ToString(txtBillingParty.Text);
                txtSAddress1.Text = Val.ToString(txtBAddress1.Text);
                txtSAddress2.Text = Val.ToString(txtBAddress2.Text);
                txtSAddress3.Text = Val.ToString(txtBAddress3.Text);
                txtSCity.Text = Val.ToString(txtBCity.Text);
                txtSCountry.Text = Val.ToString(txtBCountry.Text);
                txtSZipCode.Text = Val.ToString(txtBZipCode.Text);
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }



        private void btnPacktingListPrintRs_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.Confirm("Are You Sure For Print Entry") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                else
                {
                    if (Val.ToString(lblMemoNo.Tag) == "")
                    {
                        Global.Message("No Memo Found");
                        return;
                    }
                    this.Cursor = Cursors.WaitCursor;
                    DataTable DTab = new DataTable();
                    if (MenuTagName == "SALEINVOICEENTRY")
                    {
                        string Option = "";
                        if (rdbAll.Checked == true) Option = "ALL"; else if (rdbSingle.Checked == true) Option = "DETAIL"; else if (rdbParcel.Checked == true) Option = "PACKING";
                        if (cmbAccType.SelectedIndex == 4 && cmbBillType.Text == "Export")
                        {
                            DTab = ObjMemo.PrintPacktingListRsSaleEntryFileUploadOP(Val.ToString(lblMemoNo.Tag), "USD", Option);
                        }
                        else if (cmbAccType.SelectedIndex == 2 && cmbBillType.Text == "DollarBill")
                        {
                            DTab = ObjMemo.PrintPacktingListRsSaleEntry(Val.ToString(lblMemoNo.Tag), "USD");
                        }
                        else
                        {
                            DTab = ObjMemo.PrintPacktingListRsSaleEntryFileUploadOP(Val.ToString(lblMemoNo.Tag), "INR", Option);
                        }
                        //if (DTabMemoDetailParcelFile.Rows.Count > 0)
                        //{
                        //    if (cmbAccType.SelectedIndex == 4 && cmbBillType.Text == "Export")
                        //    {
                        //        DTab = ObjMemo.PrintPacktingListRsSaleEntryFileUpload(Val.ToString(lblMemoNo.Tag), "USD");
                        //    }
                        //    else
                        //    {
                        //        DTab = ObjMemo.PrintPacktingListRsSaleEntryFileUpload(Val.ToString(lblMemoNo.Tag), "INR");
                        //    }
                        //}
                        //else
                        //{
                        //    if (cmbAccType.SelectedIndex == 4 && cmbBillType.Text == "Export")
                        //    {
                        //        DTab = ObjMemo.PrintPacktingListRsSaleEntry(Val.ToString(lblMemoNo.Tag), "USD");
                        //    }
                        //    else
                        //    {
                        //        DTab = ObjMemo.PrintPacktingListRsSaleEntry(Val.ToString(lblMemoNo.Tag), "INR");
                        //    }
                        //}
                    }
                    else
                        DTab = ObjMemo.PrintPacktingListRs(Val.ToString(lblMemoNo.Tag), "INR");


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
                    DTabDuplicate.AcceptChanges();
                    DS.Tables.Add(DTabDuplicate);

                    if (cmbAccType.SelectedIndex == 4 && cmbBillType.Text == "Export")
                    {
                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("RptSalesExport_PackingListDollar_FileUpload_OP", DTabDuplicate);
                        //if (DTabMemoDetailParcelFile.Rows.Count > 0)
                        //    FrmReportViewer.ShowFormInvoicePrint("RptSalesExport_PackingListDollar_FileUpload", DTabDuplicate);
                        //else
                        //    FrmReportViewer.ShowFormInvoicePrint("RptSalesExport_PackingListDollar", DTabDuplicate);
                        this.Cursor = Cursors.Default;
                    }
                    else if (cmbAccType.SelectedIndex == 2 && cmbBillType.Text == "DollarBill")
                    {
                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("RptSalesExport_PackingListDollar", DTabDuplicate);
                        this.Cursor = Cursors.Default;
                    }
                    else
                    {
                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("RptSalesExport_PackingListLocal", DTabDuplicate);
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void txtLabServiceCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblLabServiceCode_Click(object sender, EventArgs e)
        {

        }

        private void lblSeller_Click(object sender, EventArgs e)
        {

        }

        private void txtSellerName_TextChanged(object sender, EventArgs e)
        {

        }


        private void cmbAccType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BOConfiguration.gStrLoginSection != "B" && mFormType == FORMTYPE.SALEINVOICE) //A : Part and SaleInv module hase
            {
                if (MenuTagName == "SALEINVOICEENTRY" || mFormType == FORMTYPE.SALEINVOICE)
                {
                    if (cmbAccType.SelectedIndex == 1) // Local Sale
                    {
                        //ChkApprovedOrder.Visible = false;
                        simpleButton1.Visible = false;


                        cLabel23.Visible = false;
                        CmbDeliveryType.Visible = false;


                        //lblBuyer.Visible = false;
                        //txtBuyer.Visible = false;
                        chkIsConsingee.Visible = false;
                        lblStkAmt.Visible = false;
                        txtStkAmtFE.Visible = false;
                        txtAdatAmt.Visible = false;
                        txtBrokerAmt.Visible = false;
                        cLabel6.Visible = false;
                        txtTransport.Visible = false;



                        lblState.Visible = true;
                        txtBState.Visible = true;
                        cLabel7.Visible = true;
                        txtBCity.Visible = true;

                        //Charges
                        cLabel41.Visible = false;
                        cLabel48.Visible = false;
                        cLabel49.Visible = false;
                        lblGSTAmountFESymbol.Visible = false;
                        cLabel50.Visible = false;
                        cLabel42.Visible = false;
                        cLabel43.Visible = false;
                        cLabel39.Visible = false;
                        cLabel76.Visible = false;
                        CmbRoundOff.Visible = false;
                        txtGSTPer.Visible = false;
                        txtGSTAmount.Visible = false;
                        txtGSTAmountFE.Visible = false;
                        txtInsurancePer.Visible = false;
                        txtInsuranceAmount.Visible = false;
                        txtInsuranceAmountFE.Visible = false;
                        txtShippingPer.Visible = false;
                        txtShippingAmount.Visible = false;
                        txtShippingAmountFE.Visible = false;
                        txtDiscPer.Visible = false;
                        txtDiscAmount.Visible = false;
                        txtDiscAmountFE.Visible = false;
                        txtRoundOffPer.Visible = false;
                        txtRoundOffAmount.Visible = false;
                        txtRoundOffAmountFE.Visible = false;

                        //cLabel69.Visible = true;
                        //txtInvoiceNo.Visible = true;

                        //Add shiv 
                        //Comment By Gunjan:10/09/2024
                        //cLabel62.Visible = true;
                        //cLabel66.Visible = true;
                        //cLabel58.Visible = true;
                        //cLabel72.Visible = true;
                        //cLabel12.Visible = true;
                        //cLabel19.Visible = false;
                        //cLabel18.Visible = true;
                        //txtCGSTPer.Visible = true;
                        //txtSGSTPer.Visible = true;
                        //txtIGSTPer.Visible = true;
                        //txtTCSPer.Visible = true;
                        //txtCGSTAmount.Visible = false;
                        //txtSGSTAmount.Visible = false;
                        //txtIGSTAmount.Visible = false;
                        //txtTCSAmount.Visible = false;
                        //txtCGSTAmountFE.Visible = true;
                        //txtSGSTAmountFE.Visible = true;
                        //txtIGSTAmountFE.Visible = true;
                        //txtTCSAmountFE.Visible = true;

                        //cLabel12.Location = new Point(465, 4);
                        //cLabel62.Location = new Point(415, 26);
                        //txtCGSTPer.Location = new Point(455, 24);
                        //cLabel66.Location = new Point(415, 49);
                        //txtSGSTPer.Location = new Point(455, 45);
                        //cLabel58.Location = new Point(415, 72);
                        //txtIGSTPer.Location = new Point(455, 68);
                        //cLabel72.Location = new Point(415, 95);
                        //txtTCSPer.Location = new Point(455, 91);
                        //Edn as Gunjan

                        txtGrossAmount.Enabled = true;
                        txtGrossAmountFE.Enabled = true;
                        txtNetAmount.Enabled = true;
                        txtNetAmountFE.Enabled = true;
                        txtBrokerExcRate.Visible = false;
                        txtAdatExcRate.Visible = false;
                        cLabel34.Visible = false;
                        CmbPaymentMode.Visible = false;
                        BtnContinue.Visible = false;

                        cLabel35.Visible = false;
                        txtCurrency.Visible = false;

                        GrdSummuryMNL.Columns["PSTATE"].Visible = false;
                        lblGRFreight.Visible = false;
                        txtGRFreight.Visible = false;


                        if (cmbBillType.Text.ToUpper() == "RUPEESBILL")
                        {
                            txtGrossAmount.Visible = false;
                            LblDollar.Visible = false;
                            txtGrossAmountFE.Visible = true;
                            lblGrossAmountFESymbol.Visible = true;
                            //txtGrossAmountFE.Location = new Point(785, 34);
                            //lblGrossAmountFESymbol.Location = new Point(248, 1);
                            //lblNetAmt.Location = new Point(450, 25);

                            txtNetAmount.Visible = false;
                            txtNetAmountFE.Visible = true;
                            //txtNetAmountFE.Location = new Point(785, 62);
                            // lblGrossAmt.Location = new Point(120, 25);
                        }
                        else if (cmbBillType.Text.ToUpper() == "DOLLARBILL")
                        {
                            txtGrossAmount.Visible = true;
                            LblDollar.Visible = true;
                            txtGrossAmountFE.Visible = false;
                            lblGrossAmountFESymbol.Visible = false;
                            //txtGrossAmountFE.Location = new Point(85, 5);
                            //lblGrossAmountFESymbol.Location = new Point(193, 11);

                            txtNetAmount.Visible = true;
                            txtNetAmountFE.Visible = false;
                            //txtNetAmountFE.Location = new Point(454, 5);
                            //lblNetAmountFESymbol.Location = new Point(561, 11);

                            txtNetAmountFE.Visible = true;
                        }

                        txtAutoInvoiceNo.Visible = true;
                        lblZipCode.Visible = true;
                        txtBZipCode.Visible = true;
                        btnConsRetrun.Visible = false;
                        cLabel24.Visible = true;
                        txtBroker.Visible = true;
                        txtBaseBrokeragePer.Visible = true;
                        txtBrokerAmtFE.Visible = true;
                        cLabel26.Visible = true;
                        txtAdat.Visible = true;
                        txtAdatPer.Visible = true;
                        txtAdatAmtFE.Visible = true;
                        cLabel92.Visible = true;
                        txtTCSCalcAmt.Visible = true;

                        lblConsignmentRefNo.Visible = false;
                        txtConsignmentRefNo.Visible = false;

                        if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME == "administrator")
                        {
                            chkIsTdsAmt.Visible = true;
                        }
                    }
                    else if (cmbAccType.SelectedIndex == 2) // Diamond Dollar With Demand
                    {
                        txtBrokerExcRate.Visible = true;
                        txtAdatExcRate.Visible = true;
                        //cLabel69.Visible = true;
                        //txtInvoiceNo.Visible = true;

                        //ChkApprovedOrder.Visible = false;
                        simpleButton1.Visible = false;


                        cLabel23.Visible = false;
                        CmbDeliveryType.Visible = false;


                        //lblBuyer.Visible = false;
                        //txtBuyer.Visible = false;
                        chkIsConsingee.Visible = false;
                        lblStkAmt.Visible = false;
                        txtStkAmtFE.Visible = false;
                        txtAdatAmt.Visible = false;
                        txtBrokerAmt.Visible = false;
                        cLabel6.Visible = false;
                        txtTransport.Visible = false;



                        lblState.Visible = true;
                        txtBState.Visible = true;
                        cLabel7.Visible = true;
                        txtBCity.Visible = true;

                        cLabel41.Visible = false;
                        cLabel48.Visible = false;
                        cLabel49.Visible = false;
                        lblGSTAmountFESymbol.Visible = false;
                        cLabel50.Visible = false;
                        cLabel42.Visible = false;
                        cLabel43.Visible = false;
                        cLabel39.Visible = false;
                        cLabel76.Visible = false;
                        CmbRoundOff.Visible = false;
                        txtGSTPer.Visible = false;
                        txtGSTAmount.Visible = false;
                        txtGSTAmountFE.Visible = false;
                        txtInsurancePer.Visible = false;
                        txtInsuranceAmount.Visible = false;
                        txtInsuranceAmountFE.Visible = false;
                        txtShippingPer.Visible = false;
                        txtShippingAmount.Visible = false;
                        txtShippingAmountFE.Visible = false;
                        txtDiscPer.Visible = false;
                        txtDiscAmount.Visible = false;
                        txtDiscAmountFE.Visible = false;
                        txtRoundOffPer.Visible = false;
                        txtRoundOffAmount.Visible = false;
                        txtRoundOffAmountFE.Visible = false;

                        //Add shiv 
                        //Comment By Gunjan:10/09/2024
                        //cLabel62.Visible = true;
                        //cLabel66.Visible = true;
                        //cLabel58.Visible = true;
                        //cLabel72.Visible = true;
                        //cLabel12.Visible = true;
                        //cLabel19.Visible = false;
                        //cLabel18.Visible = true;
                        //txtCGSTPer.Visible = true;
                        //txtSGSTPer.Visible = true;
                        //txtIGSTPer.Visible = true;
                        //txtTCSPer.Visible = true;
                        //txtCGSTAmount.Visible = false;
                        //txtSGSTAmount.Visible = false;
                        //txtIGSTAmount.Visible = false;
                        //txtTCSAmount.Visible = false;
                        //txtCGSTAmountFE.Visible = true;
                        //txtSGSTAmountFE.Visible = true;
                        //txtIGSTAmountFE.Visible = true;
                        //txtTCSAmountFE.Visible = true;


                        //cLabel12.Location = new Point(465, 4);
                        //cLabel62.Location = new Point(415, 26);
                        //txtCGSTPer.Location = new Point(455, 24);
                        //cLabel66.Location = new Point(415, 49);
                        //txtSGSTPer.Location = new Point(455, 45);
                        //cLabel58.Location = new Point(415, 72);
                        //txtIGSTPer.Location = new Point(455, 68);
                        //cLabel72.Location = new Point(415, 95);
                        //txtTCSPer.Location = new Point(455, 91);
                        //Edn aS Gunjan
                        txtGrossAmount.Enabled = true;
                        txtGrossAmountFE.Enabled = true;
                        txtNetAmount.Enabled = true;
                        txtNetAmount.Visible = false;
                        txtNetAmountFE.Enabled = true;
                        cLabel34.Visible = false;
                        CmbPaymentMode.Visible = false;
                        BtnContinue.Visible = false;

                        cLabel35.Visible = true;
                        txtCurrency.Visible = true;
                        //txtExcRate.Visible = false;

                        GrdSummuryMNL.Columns["PSTATE"].Visible = false;
                        lblGRFreight.Visible = false;
                        txtGRFreight.Visible = false;

                        //txtNetAmountFE.Location = new Point(454, 5);
                        //lblNetAmountFESymbol.Location = new Point(575, 11);

                        //txtNetAmount.Location = new Point(785, 34);
                        //lblNetAmt.Location = new Point(450, 25);

                        txtGrossAmountFE.Visible = true; //Add shiv 01-06-2022
                        lblGrossAmountFESymbol.Visible = true;

                        txtAutoInvoiceNo.Visible = true;
                        lblZipCode.Visible = true;
                        txtBZipCode.Visible = true;
                        btnConsRetrun.Visible = false;

                        DTPTermsDate.Visible = true;
                        cLabel24.Visible = true;
                        txtBroker.Visible = true;
                        txtBaseBrokeragePer.Visible = true;
                        txtBrokerAmtFE.Visible = true;
                        cLabel26.Visible = true;
                        txtAdat.Visible = true;
                        txtAdatPer.Visible = true;
                        txtAdatAmtFE.Visible = true;
                        cLabel92.Visible = true;
                        txtTCSCalcAmt.Visible = true;

                        lblConsignmentRefNo.Visible = false;
                        txtConsignmentRefNo.Visible = false;

                        if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME == "administrator")
                        {
                            chkIsTdsAmt.Visible = true;
                        }
                    }
                    else if (cmbAccType.SelectedIndex == 3)
                    {
                        txtBrokerExcRate.Visible = true;
                        txtAdatExcRate.Visible = true;
                        //cLabel69.Visible = true;
                        //txtInvoiceNo.Visible = true;

                        //ChkApprovedOrder.Visible = false;
                        simpleButton1.Visible = false;

                        cLabel23.Visible = false;
                        CmbDeliveryType.Visible = false;


                        //lblBuyer.Visible = false;
                        //txtBuyer.Visible = false;
                        chkIsConsingee.Visible = false;
                        lblStkAmt.Visible = false;
                        txtStkAmtFE.Visible = false;
                        txtAdatAmt.Visible = false;
                        txtBrokerAmt.Visible = false;
                        cLabel6.Visible = false;
                        txtTransport.Visible = false;



                        lblState.Visible = true;
                        txtBState.Visible = true;
                        cLabel7.Visible = true;
                        txtBCity.Visible = true;

                        cLabel41.Visible = false;
                        cLabel48.Visible = false;
                        cLabel49.Visible = false;
                        lblGSTAmountFESymbol.Visible = false;
                        cLabel50.Visible = false;
                        cLabel42.Visible = false;
                        cLabel43.Visible = false;
                        cLabel39.Visible = false;
                        cLabel76.Visible = false;
                        CmbRoundOff.Visible = false;
                        txtGSTPer.Visible = false;
                        txtGSTAmount.Visible = false;
                        txtGSTAmountFE.Visible = false;
                        txtInsurancePer.Visible = false;
                        txtInsuranceAmount.Visible = false;
                        txtInsuranceAmountFE.Visible = false;
                        txtShippingPer.Visible = false;
                        txtShippingAmount.Visible = false;
                        txtShippingAmountFE.Visible = false;
                        txtDiscPer.Visible = false;
                        txtDiscAmount.Visible = false;
                        txtDiscAmountFE.Visible = false;
                        txtRoundOffPer.Visible = false;
                        txtRoundOffAmount.Visible = false;
                        txtRoundOffAmountFE.Visible = false;

                        //Add shiv 
                        //Comment By Gunjan:10/09/2024
                        //cLabel62.Visible = true;
                        //cLabel66.Visible = true;
                        //cLabel58.Visible = true;
                        //cLabel72.Visible = true;
                        //cLabel12.Visible = true;
                        //cLabel19.Visible = false;
                        //cLabel18.Visible = true;
                        //txtCGSTPer.Visible = true;
                        //txtSGSTPer.Visible = true;
                        //txtIGSTPer.Visible = true;
                        //txtTCSPer.Visible = true;
                        //txtCGSTAmount.Visible = false;
                        //txtSGSTAmount.Visible = false;
                        //txtIGSTAmount.Visible = false;
                        //txtTCSAmount.Visible = false;
                        //txtCGSTAmountFE.Visible = true;
                        //txtSGSTAmountFE.Visible = true;
                        //txtIGSTAmountFE.Visible = true;
                        //txtTCSAmountFE.Visible = true;

                        //txtCGSTAmountFE.Text = "0.000";
                        //txtSGSTAmountFE.Text = "0.000";
                        //txtIGSTAmountFE.Text = "0.000";
                        //txtTCSAmountFE.Text = "0.000";
                        //txtCGSTPer.Text = "0.00";
                        //txtSGSTPer.Text = "0.00";
                        //txtIGSTPer.Text = "0.00";
                        //txtTCSPer.Text = "0.00";
                        //txtNetAmountFE.Visible = true;

                        //cLabel12.Location = new Point(465, 4);
                        //cLabel62.Location = new Point(415, 26);
                        //txtCGSTPer.Location = new Point(455, 24);
                        //cLabel66.Location = new Point(415, 49);
                        //txtSGSTPer.Location = new Point(455, 45);
                        //cLabel58.Location = new Point(415, 72);
                        //txtIGSTPer.Location = new Point(455, 68);
                        //cLabel72.Location = new Point(415, 95);
                        //txtTCSPer.Location = new Point(455, 91);
                        //End As Gunjan
                        txtGrossAmount.Enabled = true;
                        txtGrossAmountFE.Enabled = true;
                        txtNetAmount.Enabled = true;
                        txtNetAmount.Visible = false;
                        txtNetAmountFE.Enabled = true;
                        cLabel34.Visible = false;
                        CmbPaymentMode.Visible = false;
                        BtnContinue.Visible = false;

                        cLabel35.Visible = true;
                        txtCurrency.Visible = true;
                        //txtExcRate.Visible = false;

                        GrdSummuryMNL.Columns["PSTATE"].Visible = false;
                        lblGRFreight.Visible = false;
                        txtGRFreight.Visible = false;

                        //txtNetAmountFE.Location = new Point(454, 5);
                        //lblNetAmountFESymbol.Location = new Point(575, 11);

                        //txtNetAmount.Location = new Point(785, 34);
                        //lblNetAmt.Location = new Point(450, 25);

                        txtGrossAmountFE.Visible = true; //Add shiv 01-06-2022
                        lblGrossAmountFESymbol.Visible = true;

                        txtAutoInvoiceNo.Visible = true;
                        lblZipCode.Visible = true;
                        txtBZipCode.Visible = true;
                        btnConsRetrun.Visible = false;

                        DTPTermsDate.Visible = true;
                        cLabel24.Visible = true;
                        txtBroker.Visible = true;
                        txtBaseBrokeragePer.Visible = true;
                        txtBrokerAmtFE.Visible = true;
                        cLabel26.Visible = true;
                        txtAdat.Visible = true;
                        txtAdatPer.Visible = true;
                        txtAdatAmtFE.Visible = true;
                        cLabel92.Visible = true;
                        txtTCSCalcAmt.Visible = true;

                        lblConsignmentRefNo.Visible = false;
                        txtConsignmentRefNo.Visible = false;

                        if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME == "administrator")
                        {
                            chkIsTdsAmt.Visible = true;
                        }
                    }
                    else if (cmbAccType.SelectedIndex == 4) // Export
                    {
                        //ChkApprovedOrder.Visible = false;
                        simpleButton1.Visible = true;


                        cLabel23.Visible = false;
                        CmbDeliveryType.Visible = false;


                        //lblBuyer.Visible = false;
                        //txtBuyer.Visible = false;
                        chkIsConsingee.Visible = true;
                        //cLabel69.Visible = true;
                        //txtInvoiceNo.Visible = true;
                        lblStkAmt.Visible = false;
                        txtStkAmtFE.Visible = false;
                        txtAdatAmt.Visible = false;
                        txtBrokerAmt.Visible = false;
                        cLabel6.Visible = false;
                        txtTransport.Visible = false;


                        lblState.Visible = false;
                        txtBState.Visible = false;
                        cLabel7.Visible = false;
                        txtBCity.Visible = false;

                        cLabel41.Visible = true;
                        cLabel48.Visible = true;
                        cLabel49.Visible = true;
                        lblGSTAmountFESymbol.Visible = true;
                        cLabel50.Visible = true;
                        cLabel42.Visible = true;
                        cLabel43.Visible = true;
                        cLabel39.Visible = true;
                        cLabel76.Visible = true;
                        CmbRoundOff.Visible = true;
                        txtGSTPer.Visible = true;
                        txtGSTAmount.Visible = true;
                        txtGSTAmountFE.Visible = true;
                        txtInsurancePer.Visible = true;
                        txtInsuranceAmount.Visible = true;
                        txtInsuranceAmountFE.Visible = true;
                        txtShippingPer.Visible = true;
                        txtShippingAmount.Visible = true;
                        txtShippingAmountFE.Visible = true;
                        txtDiscPer.Visible = true;
                        txtDiscAmount.Visible = true;
                        txtDiscAmountFE.Visible = true;
                        txtRoundOffPer.Visible = true;
                        txtRoundOffAmount.Visible = true;
                        txtRoundOffAmountFE.Visible = true;

                        //Add shiv
                        // //Comment By Gunjan:10/09/2024
                        //cLabel62.Visible = false;
                        //cLabel66.Visible = false;
                        //cLabel58.Visible = false;
                        //cLabel72.Visible = false;
                        //cLabel12.Visible = false;
                        //cLabel19.Visible = false;
                        //cLabel18.Visible = false;
                        //txtCGSTPer.Visible = false;
                        //txtSGSTPer.Visible = false;
                        //txtIGSTPer.Visible = false;
                        //txtTCSPer.Visible = false;
                        //txtCGSTAmount.Visible = false;
                        //txtSGSTAmount.Visible = false;
                        //txtIGSTAmount.Visible = false;
                        //txtTCSAmount.Visible = false;
                        //txtCGSTAmountFE.Visible = false;
                        //txtSGSTAmountFE.Visible = false;
                        //txtIGSTAmountFE.Visible = false;
                        //txtTCSAmountFE.Visible = false;
                        //Edn As Gunjan

                        //cLabel12.Location = new Point(425, 4);
                        //cLabel62.Location = new Point(370, 26);
                        //txtCGSTPer.Location = new Point(413, 22);
                        //txtCGSTPer.Location = new Point(450, 24);
                        //cLabel66.Location = new Point(370, 49);
                        //txtSGSTPer.Location = new Point(413, 45);
                        //cLabel58.Location = new Point(370, 72);
                        //txtIGSTPer.Location = new Point(413, 68);
                        //cLabel72.Location = new Point(370, 95);
                        //txtTCSPer.Location = new Point(413, 91);

                        //Comment By Gunjan:10/09/2024
                        //cLabel12.Location = new Point(387, 6);
                        //cLabel62.Location = new Point(331, 28);
                        //txtCGSTPer.Location = new Point(373, 24);
                        //cLabel66.Location = new Point(331, 51);
                        //txtSGSTPer.Location = new Point(373, 47);
                        //cLabel58.Location = new Point(331, 74);
                        //txtIGSTPer.Location = new Point(373, 70);
                        //cLabel72.Location = new Point(331, 97);
                        //txtTCSPer.Location = new Point(373, 93);
                        //End As Gunjan

                        txtGrossAmount.Enabled = true;
                        txtGrossAmountFE.Enabled = true;
                        txtNetAmount.Enabled = true;
                        txtNetAmountFE.Enabled = false;
                        txtBrokerExcRate.Visible = true;
                        txtAdatExcRate.Visible = true;
                        cLabel34.Visible = false;
                        CmbPaymentMode.Visible = false;
                        BtnContinue.Visible = false;

                        cLabel35.Visible = true;
                        txtCurrency.Visible = true;
                        //txtExcRate.Visible = false;

                        GrdSummuryMNL.Columns["PSTATE"].Visible = true;

                        GrdSummuryMNL.Columns["FMEMOAMOUNT"].Visible = false;
                        GrdSummuryMNL.Columns["FMEMOPRICEPERCARAT"].Visible = false;

                        lblGRFreight.Visible = true;
                        txtGRFreight.Visible = true;

                        txtGrossAmountFE.Visible = true;
                        lblGrossAmountFESymbol.Visible = true;
                        txtNetAmountFE.Visible = false;

                        txtAutoInvoiceNo.Visible = false;
                        lblZipCode.Visible = false;
                        txtBZipCode.Visible = false;
                        lblGRFreight.Location = new Point(379, 94);
                        txtGRFreight.Location = new Point(458, 90);
                        btnConsRetrun.Visible = false;

                        DTPTermsDate.Visible = true;
                        cLabel24.Visible = true;
                        txtBroker.Visible = true;
                        txtBaseBrokeragePer.Visible = true;
                        txtBrokerAmtFE.Visible = true;
                        cLabel26.Visible = true;
                        txtAdat.Visible = true;
                        txtAdatPer.Visible = true;
                        txtAdatAmtFE.Visible = true;
                        cLabel92.Visible = false;
                        txtTCSCalcAmt.Visible = false;
                    }

                    if (cmbAccType.SelectedIndex == 5)
                    {
                        //ChkApprovedOrder.Visible = false;
                        simpleButton1.Visible = true;
                        cLabel23.Visible = false;
                        CmbDeliveryType.Visible = false;


                        //lblBuyer.Visible = false;
                        //txtBuyer.Visible = false;
                        chkIsConsingee.Visible = true;
                        //cLabel69.Visible = true;
                        //txtInvoiceNo.Visible = true;
                        lblStkAmt.Visible = false;
                        txtStkAmtFE.Visible = false;
                        txtAdatAmt.Visible = false;
                        txtBrokerAmt.Visible = false;
                        cLabel6.Visible = false;
                        txtTransport.Visible = false;


                        lblState.Visible = false;
                        txtBState.Visible = false;
                        cLabel7.Visible = false;
                        txtBCity.Visible = false;

                        cLabel41.Visible = true;
                        cLabel48.Visible = true;
                        cLabel49.Visible = true;
                        lblGSTAmountFESymbol.Visible = true;
                        cLabel50.Visible = true;
                        cLabel42.Visible = true;
                        cLabel43.Visible = true;
                        cLabel39.Visible = true;
                        cLabel76.Visible = true;
                        CmbRoundOff.Visible = true;
                        txtGSTPer.Visible = true;
                        txtGSTAmount.Visible = true;
                        txtGSTAmountFE.Visible = true;
                        txtInsurancePer.Visible = true;
                        txtInsuranceAmount.Visible = true;
                        txtInsuranceAmountFE.Visible = true;
                        txtShippingPer.Visible = true;
                        txtShippingAmount.Visible = true;
                        txtShippingAmountFE.Visible = true;
                        txtDiscPer.Visible = true;
                        txtDiscAmount.Visible = true;
                        txtDiscAmountFE.Visible = true;
                        txtRoundOffPer.Visible = true;
                        txtRoundOffAmount.Visible = true;
                        txtRoundOffAmountFE.Visible = true;

                        //Add shiv 
                        //Comment By Gunjan:10/09/2024
                        //cLabel62.Visible = false;
                        //cLabel66.Visible = false;
                        //cLabel58.Visible = false;
                        //cLabel72.Visible = false;
                        //cLabel12.Visible = false;
                        //cLabel19.Visible = false;
                        //cLabel18.Visible = false;
                        //txtCGSTPer.Visible = false;
                        //txtSGSTPer.Visible = false;
                        //txtIGSTPer.Visible = false;
                        //txtTCSPer.Visible = false;
                        //txtCGSTAmount.Visible = false;
                        //txtSGSTAmount.Visible = false;
                        //txtIGSTAmount.Visible = false;
                        //txtTCSAmount.Visible = false;
                        //txtCGSTAmountFE.Visible = false;
                        //txtSGSTAmountFE.Visible = false;
                        //txtIGSTAmountFE.Visible = false;
                        //txtTCSAmountFE.Visible = false;

                        //cLabel12.Location = new Point(387, 6);
                        //cLabel62.Location = new Point(331, 28);
                        //txtCGSTPer.Location = new Point(373, 24);
                        //cLabel66.Location = new Point(331, 51);
                        //txtSGSTPer.Location = new Point(373, 47);
                        //cLabel58.Location = new Point(331, 74);
                        //txtIGSTPer.Location = new Point(373, 70);
                        //cLabel72.Location = new Point(331, 97);
                        //txtTCSPer.Location = new Point(373, 93);
                        //End As Gunjan

                        txtGrossAmount.Enabled = true;
                        txtGrossAmountFE.Enabled = true;
                        txtNetAmount.Enabled = true;
                        txtNetAmountFE.Enabled = false;
                        txtBrokerExcRate.Visible = true;
                        txtAdatExcRate.Visible = true;
                        cLabel34.Visible = false;
                        CmbPaymentMode.Visible = false;
                        BtnContinue.Visible = false;

                        cLabel35.Visible = true;
                        txtCurrency.Visible = true;

                        lblGRFreight.Visible = true;
                        txtGRFreight.Visible = true;

                        txtGrossAmountFE.Visible = true;
                        lblGrossAmountFESymbol.Visible = true;
                        txtNetAmountFE.Visible = false;

                        txtAutoInvoiceNo.Visible = false;
                        lblZipCode.Visible = false;
                        txtBZipCode.Visible = false;
                        lblGRFreight.Location = new Point(379, 94);
                        txtGRFreight.Location = new Point(458, 90);
                        simpleButton1.Visible = false;
                        btnConsRetrun.Text = "Consignment Return";
                        btnConsRetrun.Visible = true;
                        DTPTermsDate.Visible = true;
                        cLabel24.Visible = true;
                        txtBroker.Visible = true;
                        txtBaseBrokeragePer.Visible = true;
                        txtBrokerAmtFE.Visible = true;
                        cLabel26.Visible = true;
                        txtAdat.Visible = true;
                        txtAdatPer.Visible = true;
                        txtAdatAmtFE.Visible = true;
                        cLabel92.Visible = true;
                        txtTCSCalcAmt.Visible = true;

                        lblConsignmentRefNo.Visible = false;
                        txtConsignmentRefNo.Visible = false;

                        chkIsTdsAmt.Visible = false;
                    }
                    else if (cmbAccType.SelectedIndex == 6)
                    {
                        // ChkApprovedOrder.Visible = false;
                        simpleButton1.Visible = true;
                        cLabel23.Visible = false;
                        CmbDeliveryType.Visible = false;
                        //lblBuyer.Visible = false;
                        //txtBuyer.Visible = false;
                        chkIsConsingee.Visible = true;
                        //cLabel69.Visible = true;
                        //txtInvoiceNo.Visible = true;
                        lblStkAmt.Visible = false;
                        txtStkAmtFE.Visible = false;
                        txtAdatAmt.Visible = false;
                        txtBrokerAmt.Visible = false;
                        cLabel6.Visible = false;
                        txtTransport.Visible = false;
                        lblState.Visible = false;
                        txtBState.Visible = false;
                        cLabel7.Visible = false;
                        txtBCity.Visible = false;

                        cLabel41.Visible = true;
                        cLabel48.Visible = true;
                        cLabel49.Visible = true;
                        lblGSTAmountFESymbol.Visible = true;
                        cLabel50.Visible = true;
                        cLabel42.Visible = true;
                        cLabel43.Visible = true;
                        cLabel39.Visible = true;
                        cLabel76.Visible = true;
                        CmbRoundOff.Visible = true;
                        txtGSTPer.Visible = true;
                        txtGSTAmount.Visible = true;
                        txtGSTAmountFE.Visible = true;
                        txtInsurancePer.Visible = true;
                        txtInsuranceAmount.Visible = true;
                        txtInsuranceAmountFE.Visible = true;
                        txtShippingPer.Visible = true;
                        txtShippingAmount.Visible = true;
                        txtShippingAmountFE.Visible = true;
                        txtDiscPer.Visible = true;
                        txtDiscAmount.Visible = true;
                        txtDiscAmountFE.Visible = true;
                        txtRoundOffPer.Visible = true;
                        txtRoundOffAmount.Visible = true;
                        txtRoundOffAmountFE.Visible = true;

                        //Add shiv 
                        //Comment By Gunjan:10/09/2024
                        //cLabel62.Visible = false;
                        //cLabel66.Visible = false;
                        //cLabel58.Visible = false;
                        //cLabel72.Visible = false;
                        //cLabel12.Visible = false;
                        //cLabel19.Visible = false;
                        //cLabel18.Visible = false;
                        //txtCGSTPer.Visible = false;
                        //txtSGSTPer.Visible = false;
                        //txtIGSTPer.Visible = false;
                        //txtTCSPer.Visible = false;
                        //txtCGSTAmount.Visible = false;
                        //txtSGSTAmount.Visible = false;
                        //txtIGSTAmount.Visible = false;
                        //txtTCSAmount.Visible = false;
                        //txtCGSTAmountFE.Visible = false;
                        //txtSGSTAmountFE.Visible = false;
                        //txtIGSTAmountFE.Visible = false;
                        //txtTCSAmountFE.Visible = false;

                        //cLabel12.Location = new Point(387, 6);
                        //cLabel62.Location = new Point(331, 28);
                        //txtCGSTPer.Location = new Point(373, 24);
                        //cLabel66.Location = new Point(331, 51);
                        //txtSGSTPer.Location = new Point(373, 47);
                        //cLabel58.Location = new Point(331, 74);
                        //txtIGSTPer.Location = new Point(373, 70);
                        //cLabel72.Location = new Point(331, 97);
                        //txtTCSPer.Location = new Point(373, 93);

                        //End As Gunjan

                        txtGrossAmount.Enabled = true;
                        txtGrossAmountFE.Enabled = true;
                        txtNetAmount.Enabled = true;
                        txtNetAmountFE.Enabled = false;
                        txtBrokerExcRate.Visible = false;
                        txtAdatExcRate.Visible = false;
                        cLabel34.Visible = false;
                        CmbPaymentMode.Visible = false;
                        BtnContinue.Visible = false;

                        cLabel35.Visible = true;
                        txtCurrency.Visible = true;

                        //txtExcRate.Visible = false;

                        GrdSummuryMNL.Columns["RETURNCARAT"].VisibleIndex = 7;

                        lblGRFreight.Visible = false;
                        txtGRFreight.Visible = false;

                        txtGrossAmountFE.Visible = true;
                        lblGrossAmountFESymbol.Visible = true;
                        txtNetAmountFE.Visible = false;

                        txtAutoInvoiceNo.Visible = false;
                        lblZipCode.Visible = false;
                        txtBZipCode.Visible = false;
                        lblGRFreight.Location = new Point(379, 94);
                        txtGRFreight.Location = new Point(458, 90);
                        simpleButton1.Visible = false;
                        btnConsRetrun.Visible = false;

                        DTPTermsDate.Visible = false;
                        cLabel24.Visible = false;
                        txtBroker.Visible = false;
                        txtBaseBrokeragePer.Visible = false;
                        txtBrokerAmtFE.Visible = false;
                        cLabel26.Visible = false;
                        txtAdat.Visible = false;
                        txtAdatPer.Visible = false;
                        txtAdatAmtFE.Visible = false;
                        cLabel92.Visible = true;
                        txtTCSCalcAmt.Visible = true;

                        lblConsignmentRefNo.Visible = false;
                        txtConsignmentRefNo.Visible = false;

                        chkIsTdsAmt.Visible = false;
                    }
                    else if (cmbAccType.SelectedIndex == 7)
                    {
                        //ChkApprovedOrder.Visible = false;
                        simpleButton1.Visible = true;


                        cLabel23.Visible = false;
                        CmbDeliveryType.Visible = false;


                        //lblBuyer.Visible = false;
                        //txtBuyer.Visible = false;
                        chkIsConsingee.Visible = true;
                        //cLabel69.Visible = true;
                        //txtInvoiceNo.Visible = true;
                        lblStkAmt.Visible = false;
                        txtStkAmtFE.Visible = false;
                        txtAdatAmt.Visible = false;
                        txtBrokerAmt.Visible = false;
                        cLabel6.Visible = false;
                        txtTransport.Visible = false;


                        lblState.Visible = false;
                        txtBState.Visible = false;
                        cLabel7.Visible = false;
                        txtBCity.Visible = false;

                        cLabel41.Visible = true;
                        cLabel48.Visible = true;
                        cLabel49.Visible = true;
                        lblGSTAmountFESymbol.Visible = true;
                        cLabel50.Visible = true;
                        cLabel42.Visible = true;
                        cLabel43.Visible = true;
                        cLabel39.Visible = true;
                        cLabel76.Visible = true;
                        CmbRoundOff.Visible = true;
                        txtGSTPer.Visible = true;
                        txtGSTAmount.Visible = true;
                        txtGSTAmountFE.Visible = true;
                        txtInsurancePer.Visible = true;
                        txtInsuranceAmount.Visible = true;
                        txtInsuranceAmountFE.Visible = true;
                        txtShippingPer.Visible = true;
                        txtShippingAmount.Visible = true;
                        txtShippingAmountFE.Visible = true;
                        txtDiscPer.Visible = true;
                        txtDiscAmount.Visible = true;
                        txtDiscAmountFE.Visible = true;
                        txtRoundOffPer.Visible = true;
                        txtRoundOffAmount.Visible = true;
                        txtRoundOffAmountFE.Visible = true;

                        //Add shiv 
                        //Comment By Gunjan:10/09/2024
                        //cLabel62.Visible = false;
                        //cLabel66.Visible = false;
                        //cLabel58.Visible = false;
                        //cLabel72.Visible = false;
                        //cLabel12.Visible = false;
                        //cLabel19.Visible = false;
                        //cLabel18.Visible = false;
                        //txtCGSTPer.Visible = false;
                        //txtSGSTPer.Visible = false;
                        //txtIGSTPer.Visible = false;
                        //txtTCSPer.Visible = false;
                        //txtCGSTAmount.Visible = false;
                        //txtSGSTAmount.Visible = false;
                        //txtIGSTAmount.Visible = false;
                        //txtTCSAmount.Visible = false;
                        //txtCGSTAmountFE.Visible = false;
                        //txtSGSTAmountFE.Visible = false;
                        //txtIGSTAmountFE.Visible = false;
                        //txtTCSAmountFE.Visible = false;

                        //cLabel12.Location = new Point(387, 6);
                        //cLabel62.Location = new Point(331, 28);
                        //txtCGSTPer.Location = new Point(373, 24);
                        //cLabel66.Location = new Point(331, 51);
                        //txtSGSTPer.Location = new Point(373, 47);
                        //cLabel58.Location = new Point(331, 74);
                        //txtIGSTPer.Location = new Point(373, 70);
                        //cLabel72.Location = new Point(331, 97);
                        //txtTCSPer.Location = new Point(373, 93);
                        //End As Gunjan

                        txtGrossAmount.Enabled = true;
                        txtGrossAmountFE.Enabled = true;
                        txtNetAmount.Enabled = true;
                        txtNetAmountFE.Enabled = false;
                        txtBrokerExcRate.Visible = true;
                        txtAdatExcRate.Visible = true;
                        cLabel34.Visible = false;
                        CmbPaymentMode.Visible = false;
                        BtnContinue.Visible = false;

                        cLabel35.Visible = true;
                        txtCurrency.Visible = true;

                        //txtExcRate.Visible = false;

                        GrdSummuryMNL.Columns["FMEMOAMOUNT"].Visible = false;
                        GrdSummuryMNL.Columns["FMEMOPRICEPERCARAT"].Visible = false;

                        lblGRFreight.Visible = true;
                        txtGRFreight.Visible = true;

                        txtGrossAmountFE.Visible = true;
                        lblGrossAmountFESymbol.Visible = true;
                        txtNetAmountFE.Visible = false;

                        txtAutoInvoiceNo.Visible = false;
                        lblZipCode.Visible = false;
                        txtBZipCode.Visible = false;
                        lblGRFreight.Location = new Point(379, 94);
                        txtGRFreight.Location = new Point(458, 90);
                        btnConsRetrun.Visible = false;

                        DTPTermsDate.Visible = true;
                        cLabel24.Visible = true;
                        txtBroker.Visible = true;
                        txtBaseBrokeragePer.Visible = true;
                        txtBrokerAmtFE.Visible = true;
                        cLabel26.Visible = true;
                        txtAdat.Visible = true;
                        txtAdatPer.Visible = true;
                        txtAdatAmtFE.Visible = true;
                        cLabel92.Visible = false;
                        txtTCSCalcAmt.Visible = false;

                        if (MenuTagName == "SALEINVOICEENTRY")
                        {
                            lblConsignmentRefNo.Visible = true;
                            txtConsignmentRefNo.Visible = true;
                            lblConsignmentRefNo.Location = new Point(604, 143);
                            txtConsignmentRefNo.Location = new Point(750, 140);
                        }
                        else
                        {
                            lblConsignmentRefNo.Visible = false;
                            txtConsignmentRefNo.Visible = false;
                        }

                        chkIsTdsAmt.Visible = false;
                    }
                    else
                    {
                        //ChkApprovedOrder.Visible = false;
                        simpleButton1.Visible = false;
                        //simpleButton1.Visible = true;


                        cLabel23.Visible = false;
                        CmbDeliveryType.Visible = false;


                        //lblBuyer.Visible = false;
                        //txtBuyer.Visible = false;
                        chkIsConsingee.Visible = false;
                        //cLabel69.Visible = false;
                        //txtInvoiceNo.Visible = false;
                        lblStkAmt.Visible = false;
                        txtStkAmtFE.Visible = false;
                        txtAdatAmt.Visible = false;
                        txtBrokerAmt.Visible = false;
                        cLabel6.Visible = false;
                        txtTransport.Visible = false;



                        cLabel41.Visible = true;
                        cLabel48.Visible = true;
                        cLabel49.Visible = true;
                        lblGSTAmountFESymbol.Visible = true;
                        cLabel50.Visible = true;
                        cLabel42.Visible = true;
                        cLabel43.Visible = true;
                        cLabel39.Visible = true;
                        cLabel76.Visible = true;
                        CmbRoundOff.Visible = true;
                        txtGSTPer.Visible = true;
                        txtGSTAmount.Visible = true;
                        txtGSTAmountFE.Visible = true;
                        txtInsurancePer.Visible = true;
                        txtInsuranceAmount.Visible = true;
                        txtInsuranceAmountFE.Visible = true;
                        txtShippingPer.Visible = true;
                        txtShippingAmount.Visible = true;
                        txtShippingAmountFE.Visible = true;
                        txtDiscPer.Visible = true;
                        txtDiscAmount.Visible = true;
                        txtDiscAmountFE.Visible = true;
                        txtRoundOffPer.Visible = true;
                        txtRoundOffAmount.Visible = true;
                        txtRoundOffAmountFE.Visible = true;

                        ////Add shiv 
                        /// //Comment By Gunjan:10/09/2024
                        //cLabel62.Visible = true;
                        //cLabel66.Visible = true;
                        //cLabel58.Visible = true;
                        //cLabel72.Visible = true;
                        //cLabel12.Visible = true;
                        //cLabel19.Visible = true;
                        //cLabel18.Visible = true;
                        //txtCGSTPer.Visible = true;
                        //txtSGSTPer.Visible = true;
                        //txtIGSTPer.Visible = true;
                        //txtTCSPer.Visible = true;
                        //txtCGSTAmount.Visible = true;
                        //txtSGSTAmount.Visible = true;
                        //txtIGSTAmount.Visible = true;
                        //txtTCSAmount.Visible = true;
                        //txtCGSTAmountFE.Visible = true;
                        //txtSGSTAmountFE.Visible = true;
                        //txtIGSTAmountFE.Visible = true;
                        //txtTCSAmountFE.Visible = true;

                        //cLabel12.Location = new Point(387, 6);
                        //cLabel62.Location = new Point(331, 28);
                        //txtCGSTPer.Location = new Point(373, 24);
                        //cLabel66.Location = new Point(331, 51);
                        //txtSGSTPer.Location = new Point(373, 47);
                        //cLabel58.Location = new Point(331, 74);
                        //txtIGSTPer.Location = new Point(373, 70);
                        //cLabel72.Location = new Point(331, 97);
                        //txtTCSPer.Location = new Point(373, 93);
                        //End As Gunjan

                        txtGrossAmount.Enabled = true;
                        txtGrossAmountFE.Enabled = true;
                        txtNetAmount.Enabled = true;
                        txtNetAmountFE.Enabled = true;
                        txtBrokerExcRate.Visible = false;
                        txtAdatExcRate.Visible = false;
                        cLabel34.Visible = true;
                        CmbPaymentMode.Visible = true;
                        BtnContinue.Visible = false;

                        cLabel35.Visible = true;
                        txtCurrency.Visible = true;

                        // txtExcRate.Visible = false;

                        GrdSummuryMNL.Columns["PSTATE"].Visible = false;
                        lblGRFreight.Visible = false;
                        txtGRFreight.Visible = false;

                        txtGrossAmountFE.Visible = true;
                        lblGrossAmountFESymbol.Visible = true;
                        txtNetAmountFE.Visible = true;

                        //txtGrossAmountFE.Location = new Point(785, 34);
                        //lblGrossAmountFESymbol.Location = new Point(830, 18);
                        txtNetAmountFE.Location = new Point(785, 62);

                        txtGrossAmount.Visible = true;
                        LblDollar.Visible = true;
                        txtNetAmount.Visible = true;

                        //txtGrossAmount.Location = new Point(676, 34);
                        //LblDollar.Location = new Point(721, 18);
                        //txtNetAmount.Location = new Point(676, 62);

                        txtAutoInvoiceNo.Visible = true;
                        lblZipCode.Visible = true;
                        txtBZipCode.Visible = true;
                        //cLabel67.Location = new Point(909, 134);
                        //cmbInsuranceType.Location = new Point(1019, 125);

                        //cLabel69.Visible = true;
                        //txtInvoiceNo.Visible = true;
                        btnConsRetrun.Visible = false;
                        //PnlOtherDetail.Visible = true; Commnet by shiv 14-11-22

                        DTPTermsDate.Visible = true;
                        cLabel24.Visible = true;
                        txtBroker.Visible = true;
                        txtBaseBrokeragePer.Visible = true;
                        txtBrokerAmtFE.Visible = true;
                        cLabel26.Visible = true;
                        txtAdat.Visible = true;
                        txtAdatPer.Visible = true;
                        txtAdatAmtFE.Visible = true;
                        cLabel92.Visible = true;
                        txtTCSCalcAmt.Visible = true;


                        lblConsignmentRefNo.Visible = false;
                        txtConsignmentRefNo.Visible = false;

                        chkIsTdsAmt.Visible = false;
                    }
                }
                else
                {
                    cLabel35.Visible = true;
                    txtCurrency.Visible = true;

                    txtExcRate.Visible = true;

                    GrdSummuryMNL.Columns["PSTATE"].Visible = false;
                    lblGRFreight.Visible = false;
                    txtGRFreight.Visible = false;

                    txtGrossAmountFE.Visible = true;
                    lblGrossAmountFESymbol.Visible = true;
                    txtNetAmountFE.Visible = true;
                    //txtGrossAmountFE.Location = new Point(211, 5);
                    //lblGrossAmountFESymbol.Location = new Point(329, 11);
                    //txtNetAmountFE.Location = new Point(582, 5);
                    txtGrossAmount.Visible = true;
                    LblDollar.Visible = true;
                    txtNetAmount.Visible = true;
                    //txtGrossAmount.Location = new Point(676, 34);
                    // LblDollar.Location = new Point(193, 11);
                    //txtNetAmount.Location = new Point(454, 5);

                    txtAutoInvoiceNo.Visible = true;
                    lblZipCode.Visible = true;
                    txtBZipCode.Visible = true;
                    //cLabel67.Location = new Point(909, 134);
                    //cmbInsuranceType.Location = new Point(1019, 125);
                }
            }
            else
            {
                //ChkApprovedOrder.Visible = true;
                simpleButton1.Visible = true;

                cLabel23.Visible = true;
                CmbDeliveryType.Visible = true;
                //lblBuyer.Visible = true;
                //txtBuyer.Visible = true;
                chkIsConsingee.Visible = true;
                //cLabel69.Visible = true;
                //txtInvoiceNo.Visible = true;
                lblStkAmt.Visible = true;
                txtStkAmtFE.Visible = true;
                txtAdatAmt.Visible = true;
                txtBrokerAmt.Visible = true;
                cLabel6.Visible = true;
                txtTransport.Visible = true;

                cLabel41.Visible = true;
                cLabel48.Visible = true;
                cLabel49.Visible = true;
                lblGSTAmountFESymbol.Visible = true;
                cLabel50.Visible = true;
                cLabel42.Visible = true;
                cLabel43.Visible = true;
                cLabel39.Visible = true;
                cLabel76.Visible = true;
                CmbRoundOff.Visible = true;
                txtGSTPer.Visible = true;
                txtGSTAmount.Visible = true;
                txtGSTAmountFE.Visible = true;
                txtInsurancePer.Visible = true;
                txtInsuranceAmount.Visible = true;
                txtInsuranceAmountFE.Visible = true;
                txtShippingPer.Visible = true;
                txtShippingAmount.Visible = true;
                txtShippingAmountFE.Visible = true;
                txtDiscPer.Visible = true;
                txtDiscAmount.Visible = true;
                txtDiscAmountFE.Visible = true;
                txtRoundOffPer.Visible = true;
                txtRoundOffAmount.Visible = true;
                txtRoundOffAmountFE.Visible = true;

                //Add shiv 
                //Comment By Gunjan:10/09/2024
                //cLabel62.Visible = true;
                //cLabel66.Visible = true;
                //cLabel58.Visible = true;
                //cLabel72.Visible = true;
                //cLabel12.Visible = true;
                //cLabel19.Visible = true;
                //cLabel18.Visible = true;
                //txtCGSTPer.Visible = true;
                //txtSGSTPer.Visible = true;
                //txtIGSTPer.Visible = true;
                //txtTCSPer.Visible = true;
                //txtCGSTAmount.Visible = true;
                //txtSGSTAmount.Visible = true;
                //txtIGSTAmount.Visible = true;
                //txtTCSAmount.Visible = true;
                //txtCGSTAmountFE.Visible = true;
                //txtSGSTAmountFE.Visible = true;
                //txtIGSTAmountFE.Visible = true;
                //txtTCSAmountFE.Visible = true;

                //cLabel12.Location = new Point(387, 6);
                //cLabel62.Location = new Point(331, 28);
                //txtCGSTPer.Location = new Point(373, 24);
                //cLabel66.Location = new Point(331, 51);
                //txtSGSTPer.Location = new Point(373, 47);
                //cLabel58.Location = new Point(331, 74);
                //txtIGSTPer.Location = new Point(373, 70);
                //cLabel72.Location = new Point(331, 97);
                //txtTCSPer.Location = new Point(373, 93);
                //End As Gunjan

                //txtGrossAmount.Location = new Point(676, 34);
                // LblDollar.Location = new Point(124, 1);
                //txtNetAmount.Location = new Point(400, 20);


                //txtGrossAmountFE.Location = new Point(785, 34);
                //lblGrossAmountFESymbol.Location = new Point(248, 1);
                //txtNetAmount.Location = new Point(785, 34);

                txtGrossAmount.Enabled = true;
                txtGrossAmountFE.Enabled = true;
                txtNetAmount.Enabled = true;
                txtNetAmountFE.Enabled = true;
                cLabel35.Visible = true;
                txtCurrency.Visible = true;
                txtExcRate.Visible = true;

                txtBrokerExcRate.Visible = false;
                txtAdatExcRate.Visible = false;
                cLabel34.Visible = true;
                CmbPaymentMode.Visible = true;
                BtnContinue.Visible = true;

                GrdSummuryMNL.Columns["PSTATE"].Visible = false;
                lblGRFreight.Visible = false;
                txtGRFreight.Visible = false;

                txtGrossAmountFE.Visible = true;
                lblGrossAmountFESymbol.Visible = true;
                txtNetAmountFE.Visible = true;
                //lblNetAmountFESymbol.Location = new Point(698, 11);
                txtGrossAmount.Visible = true;
                LblDollar.Visible = true;
                txtNetAmount.Visible = true;

                txtAutoInvoiceNo.Visible = true;
                lblZipCode.Visible = true;
                txtBZipCode.Visible = true;
                btnConsRetrun.Visible = false;
                DTPTermsDate.Visible = true;
                cLabel24.Visible = true;
                txtBroker.Visible = true;
                txtBaseBrokeragePer.Visible = true;
                txtBrokerAmtFE.Visible = true;
                cLabel26.Visible = true;
                txtAdat.Visible = true;
                txtAdatPer.Visible = true;
                txtAdatAmtFE.Visible = true;
                cLabel92.Visible = true;
                txtTCSCalcAmt.Visible = true;

                lblConsignmentRefNo.Visible = false;
                txtConsignmentRefNo.Visible = false;

                chkIsTdsAmt.Visible = false;
            }
        }


        private void btnPanelPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(lblMemoNo.Tag) == "")
                {
                    Global.Message("No Memo Found");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;


                DataTable DTab = ObjMemo.PrintRs(Val.ToString(lblMemoNo.Tag), "INR");
                DataSet DtabQR = ObjEInvoice.GetEInvoiceData(Val.ToString(lblMemoNo.Tag));
                string AccState = txtAccState.Text;

                if (DtabQR.Tables[0].Rows.Count > 0)
                {
                    SignedQRCode = DtabQR.Tables[0].Rows[0]["SignedQRCode"].ToString();
                }

                if (DTab.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("There Is No Data Found For Print");
                    return;
                }

                var Pic = new PictureBox();
                Image Img;
                var qr = new QRCodeGenerator();
                QRCodeData Qrdata = qr.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.L);
                var code = new QRCoder.QRCode(Qrdata);
                Pic.Image = code.GetGraphic(2);
                Img = Pic.Image;
                // Img.Save("d:\\a.png",ImageFormat.Png);
                var barcode = new Linear();
                barcode.Type = BarcodeType.CODE128;
                barcode.Data = AckNo;

                DataSet DS = new DataSet();
                DTab.TableName = "Table";
                DS.Tables.Add(DTab);
                DataTable DTabDuplicate = DTab.Copy();
                DTabDuplicate.TableName = "Table1";
                foreach (DataRow DRow in DTabDuplicate.Rows)
                {
                    DRow["SignedQRCode"] = ImageToByteArray(Img);
                    DRow["PrintPartyState"] = AccState;
                }
                DTabDuplicate.AcceptChanges();
                DS.Tables.Add(DTabDuplicate);

                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowFormInvoicePrint("AccSalesLocalSummaryPrint", DTabDuplicate);
                this.Cursor = Cursors.Default;

            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.Default;
                Global.Message(EX.Message);
            }
        }

        private void txtBackAddLess_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BtnModifyPrice_Click(null, null);
            }
            catch (Exception Ex)
            {

            }
        }

        private void BtnLedgerList_Click(object sender, EventArgs e)
        {
            FrmLedgerList frm = new FrmLedgerList();
            frm.ShowForm("BROKER");
        }

        private void BtnAddNewLedger_Click(object sender, EventArgs e)
        {
            FrmLedgerList Frm = new FrmLedgerList();
            Frm.ShowForm("SALE");
        }

        private void txtPricePerCaratDisc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BtnModifyPrice_Click(null, null);
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void btnPanlClose_Click(object sender, EventArgs e)
        {
            try
            {
                PnlPrintState.Visible = false;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtAccState_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (cmbAccType.SelectedIndex == 4)
                {
                    DataTable DtSignature = new DataTable();
                    DtSignature.Columns.Add("ID", typeof(int));
                    DtSignature.Columns.Add("Name", typeof(string));

                    DtSignature.Rows.Add(1, "ARVINDBHAI S. GAJERA");
                    DtSignature.Rows.Add(2, "KENIL JAYSUKH SELIYA");
                    DtSignature.Rows.Add(3, "PARESHBHAI J. KHOKHARIA ");
                    DtSignature.Rows.Add(4, "CHANDRAKANT J. KHOKHARIA");
                    DtSignature.Rows.Add(5, "JERAMBHAI B. KHOKHARIA ");
                    DtSignature.Rows.Add(6, "RAJNIKANT V. GOYANI");
                    DtSignature.Rows.Add(7, "JIGNESHBHAI RAVJIBHAI SELIYA");


                    if (Global.OnKeyPressEveToPopup(e))
                    {
                        FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                        FrmSearch.mStrSearchField = "ID,Name";
                        FrmSearch.mStrSearchText = e.KeyChar.ToString();
                        this.Cursor = Cursors.WaitCursor;
                        //FrmSearch.mDTab = new BusLib.BOComboFill().GetState(Val.ToInt(txtBCountry.Tag));
                        FrmSearch.mDTab = DtSignature;

                        //FrmSearch.mStrColumnsToHide = "ID";
                        this.Cursor = Cursors.Default;
                        FrmSearch.ShowDialog();
                        e.Handled = true;
                        if (FrmSearch.DRow != null)
                        {
                            txtAccState.Text = Val.ToString(FrmSearch.DRow["Name"]);
                            txtAccState.Tag = Val.ToString(FrmSearch.DRow["ID"]);
                        }
                        FrmSearch.Hide();
                        FrmSearch.Dispose();
                        FrmSearch = null;
                    }
                }
                else
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
                            txtAccState.Text = Val.ToString(FrmSearch.DRow["STATENAME"]);
                            txtAccState.Tag = Val.ToString(FrmSearch.DRow["STATE_ID"]);
                        }
                        FrmSearch.Hide();
                        FrmSearch.Dispose();
                        FrmSearch = null;
                    }
                }
                // AutoGstCalculation();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void btnPrintSummary_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Global.Confirm("Are You Sure For Print Entry") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                else
                {
                    if (Val.ToString(lblMemoNo.Tag) == "")
                    {
                        Global.Message("No Memo Found");
                        return;
                    }
                    this.Cursor = Cursors.WaitCursor;

                    if (cmbAccType.SelectedIndex == 1)
                    {
                        DataTable DTab = ObjMemo.PrintRs(Val.ToString(lblMemoNo.Tag), "SUMMARY");
                        DataSet DtabQR = ObjEInvoice.GetEInvoiceData(Val.ToString(lblMemoNo.Tag));
                        string AccState = txtAccState.Text;

                        if (DtabQR.Tables[0].Rows.Count > 0)
                        {
                            SignedQRCode = DtabQR.Tables[0].Rows[0]["SignedQRCode"].ToString();
                        }

                        if (DTab.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("There Is No Data Found For Print");
                            return;
                        }

                        var Pic = new PictureBox();
                        Image Img;
                        var qr = new QRCodeGenerator();
                        QRCodeData Qrdata = qr.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.L);
                        var code = new QRCoder.QRCode(Qrdata);
                        Pic.Image = code.GetGraphic(2);
                        Img = Pic.Image;
                        // Img.Save("d:\\a.png",ImageFormat.Png);
                        var barcode = new Linear();
                        barcode.Type = BarcodeType.CODE128;
                        barcode.Data = AckNo;

                        DataSet DS = new DataSet();
                        DTab.TableName = "Table";
                        DS.Tables.Add(DTab);
                        DataTable DTabDuplicate = DTab.Copy();
                        DTabDuplicate.TableName = "Table1";
                        foreach (DataRow DRow in DTabDuplicate.Rows)
                        {
                            DRow["SignedQRCode"] = ImageToByteArray(Img);
                            DRow["PrintPartyState"] = AccState;
                        }
                        DTabDuplicate.AcceptChanges();
                        DS.Tables.Add(DTabDuplicate);

                        if (DTabDuplicate.Rows.Count > 0)
                        {
                            PartyName = Val.ToString(DTabDuplicate.Rows[0]["PARTYNAME"]);
                            InvoiceNo = Val.ToString(DTabDuplicate.Rows[0]["InoVoiceno"]);
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("AccSalesLocalSummaryPrint", DTabDuplicate, PartyName, InvoiceNo);
                        this.Cursor = Cursors.Default;
                    }
                    else if (cmbAccType.SelectedIndex == 2) //add shiv 22-06-2022
                    {
                        DataTable DTab = ObjMemo.PrintRsDDA(Val.ToString(lblMemoNo.Tag), "SUMMARY");
                        DataSet DtabQR = ObjEInvoice.GetEInvoiceData(Val.ToString(lblMemoNo.Tag));
                        string AccState = txtAccState.Text;

                        if (DtabQR.Tables[0].Rows.Count > 0)
                        {
                            SignedQRCode = DtabQR.Tables[0].Rows[0]["SignedQRCode"].ToString();
                        }

                        if (DTab.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("There Is No Data Found For Print");
                            return;
                        }

                        var Pic = new PictureBox();
                        Image Img;
                        var qr = new QRCodeGenerator();
                        QRCodeData Qrdata = qr.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.L);
                        var code = new QRCoder.QRCode(Qrdata);
                        Pic.Image = code.GetGraphic(2);
                        Img = Pic.Image;
                        // Img.Save("d:\\a.png",ImageFormat.Png);
                        var barcode = new Linear();
                        barcode.Type = BarcodeType.CODE128;
                        barcode.Data = AckNo;

                        DataSet DS = new DataSet();
                        DTab.TableName = "Table";
                        DS.Tables.Add(DTab);
                        DataTable DTabDuplicate = DTab.Copy();
                        DTabDuplicate.TableName = "Table1";
                        foreach (DataRow DRow in DTabDuplicate.Rows)
                        {
                            DRow["SignedQRCode"] = ImageToByteArray(Img);
                            DRow["PrintPartyState"] = AccState;
                        }
                        DTabDuplicate.AcceptChanges();
                        DS.Tables.Add(DTabDuplicate);

                        if (DTabDuplicate.Rows.Count > 0)
                        {
                            PartyName = Val.ToString(DTabDuplicate.Rows[0]["PARTYNAME"]);
                            InvoiceNo = Val.ToString(DTabDuplicate.Rows[0]["InoVoiceno"]);
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("AccSalesDDASummaryPrint", DTabDuplicate, PartyName, InvoiceNo);
                        this.Cursor = Cursors.Default;
                    }
                    else if (cmbAccType.SelectedIndex == 4)
                    {
                        DataTable DTab = ObjMemo.PrintRsExport(Val.ToString(lblMemoNo.Tag), "SUMMARY");
                        DataSet DtabQR = ObjEInvoice.GetEInvoiceData(Val.ToString(lblMemoNo.Tag));
                        string AccState = txtAccState.Text;

                        if (DtabQR.Tables[0].Rows.Count > 0)
                        {
                            SignedQRCode = DtabQR.Tables[0].Rows[0]["SignedQRCode"].ToString();
                        }

                        if (DTab.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("There Is No Data Found For Print");
                            return;
                        }

                        var Pic = new PictureBox();
                        Image Img;
                        var qr = new QRCodeGenerator();
                        QRCodeData Qrdata = qr.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.L);
                        var code = new QRCoder.QRCode(Qrdata);
                        Pic.Image = code.GetGraphic(2);
                        Img = Pic.Image;
                        // Img.Save("d:\\a.png",ImageFormat.Png);
                        var barcode = new Linear();
                        barcode.Type = BarcodeType.CODE128;
                        barcode.Data = AckNo;

                        DataSet DS = new DataSet();
                        DTab.TableName = "Table";
                        DS.Tables.Add(DTab);
                        DataTable DTabDuplicate = DTab.Copy();
                        DTabDuplicate.TableName = "Table1";
                        foreach (DataRow DRow in DTabDuplicate.Rows)
                        {
                            DRow["SignedQRCode"] = ImageToByteArray(Img);
                            DRow["PrintPartyState"] = AccState;
                        }
                        DTabDuplicate.AcceptChanges();
                        DS.Tables.Add(DTabDuplicate);

                        if (DTabDuplicate.Rows.Count > 0)
                        {
                            PartyName = Val.ToString(DTabDuplicate.Rows[0]["PARTYNAME"]);
                            InvoiceNo = Val.ToString(DTabDuplicate.Rows[0]["InoVoiceno"]);
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("AccSalesExportSummaryPrint", DTabDuplicate, PartyName, InvoiceNo);
                        this.Cursor = Cursors.Default;
                    }
                    else if (cmbAccType.SelectedIndex == 3)
                    {
                        DataTable DTab = ObjMemo.PrintRsDDA(Val.ToString(lblMemoNo.Tag), "SUMMARY");
                        DataSet DtabQR = ObjEInvoice.GetEInvoiceData(Val.ToString(lblMemoNo.Tag));
                        string AccState = txtAccState.Text;

                        if (DtabQR.Tables[0].Rows.Count > 0)
                        {
                            SignedQRCode = DtabQR.Tables[0].Rows[0]["SignedQRCode"].ToString();
                        }

                        if (DTab.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("There Is No Data Found For Print");
                            return;
                        }

                        var Pic = new PictureBox();
                        Image Img;
                        var qr = new QRCodeGenerator();
                        QRCodeData Qrdata = qr.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.L);
                        var code = new QRCoder.QRCode(Qrdata);
                        Pic.Image = code.GetGraphic(2);
                        Img = Pic.Image;
                        // Img.Save("d:\\a.png",ImageFormat.Png);
                        var barcode = new Linear();
                        barcode.Type = BarcodeType.CODE128;
                        barcode.Data = AckNo;

                        DataSet DS = new DataSet();
                        DTab.TableName = "Table";
                        DS.Tables.Add(DTab);
                        DataTable DTabDuplicate = DTab.Copy();
                        DTabDuplicate.TableName = "Table1";
                        foreach (DataRow DRow in DTabDuplicate.Rows)
                        {
                            DRow["SignedQRCode"] = ImageToByteArray(Img);
                            DRow["PrintPartyState"] = AccState;
                        }
                        DTabDuplicate.AcceptChanges();
                        DS.Tables.Add(DTabDuplicate);

                        if (DTabDuplicate.Rows.Count > 0)
                        {
                            PartyName = Val.ToString(DTabDuplicate.Rows[0]["PARTYNAME"]);
                            InvoiceNo = Val.ToString(DTabDuplicate.Rows[0]["InoVoiceno"]);
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("AccSalesDemSummaryPrint", DTabDuplicate, PartyName, InvoiceNo);
                        this.Cursor = Cursors.Default;
                    }
                    else if (cmbAccType.SelectedIndex == 5)
                    {
                        DataTable DTab = ObjMemo.PrintRsExport(Val.ToString(lblMemoNo.Tag), "SUMMARY");
                        DataSet DtabQR = ObjEInvoice.GetEInvoiceData(Val.ToString(lblMemoNo.Tag));
                        string AccState = txtAccState.Text;

                        if (DtabQR.Tables[0].Rows.Count > 0)
                        {
                            SignedQRCode = DtabQR.Tables[0].Rows[0]["SignedQRCode"].ToString();
                        }

                        if (DTab.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("There Is No Data Found For Print");
                            return;
                        }

                        var Pic = new PictureBox();
                        Image Img;
                        var qr = new QRCodeGenerator();
                        QRCodeData Qrdata = qr.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.L);
                        var code = new QRCoder.QRCode(Qrdata);
                        Pic.Image = code.GetGraphic(2);
                        Img = Pic.Image;
                        // Img.Save("d:\\a.png",ImageFormat.Png);
                        var barcode = new Linear();
                        barcode.Type = BarcodeType.CODE128;
                        barcode.Data = AckNo;

                        DataSet DS = new DataSet();
                        DTab.TableName = "Table";
                        DS.Tables.Add(DTab);
                        DataTable DTabDuplicate = DTab.Copy();
                        DTabDuplicate.TableName = "Table1";
                        foreach (DataRow DRow in DTabDuplicate.Rows)
                        {
                            DRow["SignedQRCode"] = ImageToByteArray(Img);
                            DRow["PrintPartyState"] = AccState;
                        }
                        DTabDuplicate.AcceptChanges();
                        DS.Tables.Add(DTabDuplicate);

                        if (DTabDuplicate.Rows.Count > 0)
                        {
                            PartyName = Val.ToString(DTabDuplicate.Rows[0]["PARTYNAME"]);
                            InvoiceNo = Val.ToString(DTabDuplicate.Rows[0]["InoVoiceno"]);
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("Acc_DeliveryChallan", DTabDuplicate, PartyName, InvoiceNo);
                        this.Cursor = Cursors.Default;
                    }
                    else if (cmbAccType.SelectedIndex == 7)
                    {
                        DataTable DTab = ObjMemo.PrintRsExport(Val.ToString(lblMemoNo.Tag), "SUMMARY");
                        DataSet DtabQR = ObjEInvoice.GetEInvoiceData(Val.ToString(lblMemoNo.Tag));
                        string AccState = txtAccState.Text;

                        if (DtabQR.Tables[0].Rows.Count > 0)
                        {
                            SignedQRCode = DtabQR.Tables[0].Rows[0]["SignedQRCode"].ToString();
                        }

                        if (DTab.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("There Is No Data Found For Print");
                            return;
                        }

                        var Pic = new PictureBox();
                        Image Img;
                        var qr = new QRCodeGenerator();
                        QRCodeData Qrdata = qr.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.L);
                        var code = new QRCoder.QRCode(Qrdata);
                        Pic.Image = code.GetGraphic(2);
                        Img = Pic.Image;
                        // Img.Save("d:\\a.png",ImageFormat.Png);
                        var barcode = new Linear();
                        barcode.Type = BarcodeType.CODE128;
                        barcode.Data = AckNo;

                        DataSet DS = new DataSet();
                        DTab.TableName = "Table";
                        DS.Tables.Add(DTab);
                        DataTable DTabDuplicate = DTab.Copy();
                        DTabDuplicate.TableName = "Table1";
                        foreach (DataRow DRow in DTabDuplicate.Rows)
                        {
                            DRow["SignedQRCode"] = ImageToByteArray(Img);
                            DRow["PrintPartyState"] = AccState;
                        }
                        DTabDuplicate.AcceptChanges();
                        DS.Tables.Add(DTabDuplicate);

                        if (DTabDuplicate.Rows.Count > 0)
                        {
                            PartyName = Val.ToString(DTabDuplicate.Rows[0]["PARTYNAME"]);
                            InvoiceNo = Val.ToString(DTabDuplicate.Rows[0]["InoVoiceno"]);
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("AccSalesNetConsiSummaryPrint", DTabDuplicate, PartyName, InvoiceNo);
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void btnPrintDetail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Global.Confirm("Are You Sure For Print Entry") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                else
                {
                    if (Val.ToString(lblMemoNo.Tag) == "")
                    {
                        Global.Message("No Memo Found");
                        return;
                    }
                    this.Cursor = Cursors.WaitCursor;

                    if (cmbAccType.SelectedIndex == 1)
                    {
                        DataTable DTab = ObjMemo.PrintRs(Val.ToString(lblMemoNo.Tag), "DETAIL");
                        DataSet DtabQR = ObjEInvoice.GetEInvoiceData(Val.ToString(lblMemoNo.Tag));
                        string AccState = txtAccState.Text;

                        if (DtabQR.Tables[0].Rows.Count > 0)
                        {
                            SignedQRCode = DtabQR.Tables[0].Rows[0]["SignedQRCode"].ToString();
                        }

                        if (DTab.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("There Is No Data Found For Print");
                            return;
                        }

                        var Pic = new PictureBox();
                        Image Img;
                        var qr = new QRCodeGenerator();
                        QRCodeData Qrdata = qr.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.L);
                        var code = new QRCoder.QRCode(Qrdata);
                        Pic.Image = code.GetGraphic(2);
                        Img = Pic.Image;
                        // Img.Save("d:\\a.png",ImageFormat.Png);
                        var barcode = new Linear();
                        barcode.Type = BarcodeType.CODE128;
                        barcode.Data = AckNo;

                        DataSet DS = new DataSet();
                        DTab.TableName = "Table";
                        DS.Tables.Add(DTab);
                        DataTable DTabDuplicate = DTab.Copy();
                        DTabDuplicate.TableName = "Table1";
                        foreach (DataRow DRow in DTabDuplicate.Rows)
                        {
                            DRow["SignedQRCode"] = ImageToByteArray(Img);
                            DRow["PrintPartyState"] = AccState;
                        }
                        DTabDuplicate.AcceptChanges();
                        DS.Tables.Add(DTabDuplicate);

                        if (DTabDuplicate.Rows.Count > 0)
                        {
                            PartyName = Val.ToString(DTabDuplicate.Rows[0]["PARTYNAME"]);
                            InvoiceNo = Val.ToString(DTabDuplicate.Rows[0]["InoVoiceno"]);
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("AccSalesLocalSummaryPrint", DTabDuplicate, PartyName, InvoiceNo);
                        this.Cursor = Cursors.Default;
                    }
                    else if (cmbAccType.SelectedIndex == 2) //add shiv 22-06-2022
                    {
                        DataTable DTab = ObjMemo.PrintRsDDA(Val.ToString(lblMemoNo.Tag), "DETAIL");
                        DataSet DtabQR = ObjEInvoice.GetEInvoiceData(Val.ToString(lblMemoNo.Tag));
                        string AccState = txtAccState.Text;

                        if (DtabQR.Tables[0].Rows.Count > 0)
                        {
                            SignedQRCode = DtabQR.Tables[0].Rows[0]["SignedQRCode"].ToString();
                        }

                        if (DTab.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("There Is No Data Found For Print");
                            return;
                        }

                        var Pic = new PictureBox();
                        Image Img;
                        var qr = new QRCodeGenerator();
                        QRCodeData Qrdata = qr.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.L);
                        var code = new QRCoder.QRCode(Qrdata);
                        Pic.Image = code.GetGraphic(2);
                        Img = Pic.Image;
                        // Img.Save("d:\\a.png",ImageFormat.Png);
                        var barcode = new Linear();
                        barcode.Type = BarcodeType.CODE128;
                        barcode.Data = AckNo;

                        DataSet DS = new DataSet();
                        DTab.TableName = "Table";
                        DS.Tables.Add(DTab);
                        DataTable DTabDuplicate = DTab.Copy();
                        DTabDuplicate.TableName = "Table1";
                        foreach (DataRow DRow in DTabDuplicate.Rows)
                        {
                            DRow["SignedQRCode"] = ImageToByteArray(Img);
                            DRow["PrintPartyState"] = AccState;
                        }
                        DTabDuplicate.AcceptChanges();
                        DS.Tables.Add(DTabDuplicate);

                        if (DTabDuplicate.Rows.Count > 0)
                        {
                            PartyName = Val.ToString(DTabDuplicate.Rows[0]["PARTYNAME"]);
                            InvoiceNo = Val.ToString(DTabDuplicate.Rows[0]["InoVoiceno"]);
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("AccSalesDDASummaryPrint", DTabDuplicate, PartyName, InvoiceNo);
                        this.Cursor = Cursors.Default;
                    }
                    else if (cmbAccType.SelectedIndex == 4)
                    {
                        string Option = "";
                        if (rdbAll.Checked == true) Option = "ALL"; else if (rdbSingle.Checked == true) Option = "DETAIL"; else if (rdbParcel.Checked == true) Option = "PACKING";
                        DataTable DTab = new DataTable();
                        //if (DTabMemoDetailParcelFile.Rows.Count > 0)
                        //{
                        //    DTab = ObjMemo.PrintRsExportFileUpload(Val.ToString(lblMemoNo.Tag), "DETAIL");
                        //}
                        //else
                        //{
                        DTab = ObjMemo.PrintRsExportFileUploadOP(Val.ToString(lblMemoNo.Tag), "DETAIL", Option);
                        //}
                        DataSet DtabQR = ObjEInvoice.GetEInvoiceData(Val.ToString(lblMemoNo.Tag));
                        string AccState = txtAccState.Text;

                        if (DtabQR.Tables[0].Rows.Count > 0)
                        {
                            SignedQRCode = DtabQR.Tables[0].Rows[0]["SignedQRCode"].ToString();
                        }

                        if (DTab.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("There Is No Data Found For Print");
                            return;
                        }

                        var Pic = new PictureBox();
                        Image Img;
                        var qr = new QRCodeGenerator();
                        QRCodeData Qrdata = qr.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.L);
                        var code = new QRCoder.QRCode(Qrdata);
                        Pic.Image = code.GetGraphic(2);
                        Img = Pic.Image;
                        // Img.Save("d:\\a.png",ImageFormat.Png);
                        var barcode = new Linear();
                        barcode.Type = BarcodeType.CODE128;
                        barcode.Data = AckNo;

                        DataSet DS = new DataSet();
                        DTab.TableName = "Table";
                        DS.Tables.Add(DTab);
                        DataTable DTabDuplicate = DTab.Copy();
                        DTabDuplicate.TableName = "Table1";
                        foreach (DataRow DRow in DTabDuplicate.Rows)
                        {
                            DRow["SignedQRCode"] = ImageToByteArray(Img);
                            DRow["PrintPartyState"] = AccState;
                        }
                        DTabDuplicate.AcceptChanges();
                        DS.Tables.Add(DTabDuplicate);

                        if (DTabDuplicate.Rows.Count > 0)
                        {
                            PartyName = Val.ToString(DTabDuplicate.Rows[0]["PARTYNAME"]);
                            InvoiceNo = Val.ToString(DTabDuplicate.Rows[0]["InoVoiceno"]);
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        //if (DTabMemoDetailParcelFile.Rows.Count > 0)
                        //    FrmReportViewer.ShowFormInvoicePrint("AccSalesExportSummaryPrint_FileUpload", DTabDuplicate, PartyName, InvoiceNo);
                        //else
                        FrmReportViewer.ShowFormInvoicePrint("AccSalesExportSummaryPrint", DTabDuplicate, PartyName, InvoiceNo);
                        this.Cursor = Cursors.Default;
                    }
                    else if (cmbAccType.SelectedIndex == 3)
                    {
                        DataTable DTab = ObjMemo.PrintRsDDA(Val.ToString(lblMemoNo.Tag), "DETAIL");
                        DataSet DtabQR = ObjEInvoice.GetEInvoiceData(Val.ToString(lblMemoNo.Tag));
                        string AccState = txtAccState.Text;

                        if (DtabQR.Tables[0].Rows.Count > 0)
                        {
                            SignedQRCode = DtabQR.Tables[0].Rows[0]["SignedQRCode"].ToString();
                        }

                        if (DTab.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("There Is No Data Found For Print");
                            return;
                        }

                        var Pic = new PictureBox();
                        Image Img;
                        var qr = new QRCodeGenerator();
                        QRCodeData Qrdata = qr.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.L);
                        var code = new QRCoder.QRCode(Qrdata);
                        Pic.Image = code.GetGraphic(2);
                        Img = Pic.Image;
                        // Img.Save("d:\\a.png",ImageFormat.Png);
                        var barcode = new Linear();
                        barcode.Type = BarcodeType.CODE128;
                        barcode.Data = AckNo;

                        DataSet DS = new DataSet();
                        DTab.TableName = "Table";
                        DS.Tables.Add(DTab);
                        DataTable DTabDuplicate = DTab.Copy();
                        DTabDuplicate.TableName = "Table1";
                        foreach (DataRow DRow in DTabDuplicate.Rows)
                        {
                            DRow["SignedQRCode"] = ImageToByteArray(Img);
                            DRow["PrintPartyState"] = AccState;
                        }
                        DTabDuplicate.AcceptChanges();
                        DS.Tables.Add(DTabDuplicate);

                        if (DTabDuplicate.Rows.Count > 0)
                        {
                            PartyName = Val.ToString(DTabDuplicate.Rows[0]["PARTYNAME"]);
                            InvoiceNo = Val.ToString(DTabDuplicate.Rows[0]["InoVoiceno"]);
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormInvoicePrint("AccSalesDemSummaryPrint", DTabDuplicate, PartyName, InvoiceNo);
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public void GridCalulationMNL()
        {
            for (int i = 0; i < DTabMemoDetail.Rows.Count; i++)
            {
                var DRows = DTabMemoDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["MEMODETAIL_ID"]) == "").ToArray();
                foreach (DataRow dr in DRows)
                    DTabMemoDetail.Rows.Remove(dr);

                DTabMemoDetail.AcceptChanges();
                if (DTabMemoDetail.Rows.Count > 0)
                {

                    DataRow DR = DTabMemoDetail.Rows[i];
                    if (DTabMemoDetail.Rows[i]["MEMO_ID"].ToString() == "")
                    {
                        if (cmbBillType.Text.ToUpper() == "RUPEESBILL")
                        {
                            if (DTabMemoDetail.Rows[i]["JANGEDNOSTR"].ToString() != "")
                            {
                                FindRapSaleEntry(DR, i);
                            }
                        }
                        else if (cmbBillType.Text.ToUpper() == "EXPORT")
                        {
                            if (DTabMemoDetail.Rows[i]["MEMODETAIL_ID"].ToString() != "")
                            {
                                FindRapSaleEntry(DR, i);
                            }
                        }
                        else if (cmbBillType.Text.ToUpper() == "DOLLARBILL")
                        {
                            if (DTabMemoDetail.Rows[i]["JANGEDNOSTR"].ToString() != "")
                            {
                                FindRapSaleEntry(DR, i);
                            }
                        }
                        else
                        {
                            if (DTabMemoDetail.Rows[i]["MEMODETAIL_ID"].ToString() != "")
                            {
                                FindRapSaleEntry(DR, i);
                            }
                        }
                    }
                    else
                    {
                        if (DTabMemoDetail.Rows[i]["JANGEDNOSTR"].ToString() != "")
                        {
                            FindRapSaleEntry(DR, i);
                        }
                    }
                }
            }
        }

        public int FindID(DataTable pDTab, string pStrValue, string pStrStoneNo, string pStrColumnName, ref int IntCheck, ref string StrMessage)
        {
            try
            {
                pStrValue = pStrValue.Trim().ToUpper();

                if (pStrValue != "")
                {
                    var dr = (from DrPara in pDTab.AsEnumerable()
                              where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(pStrValue)
                              select DrPara);
                    IntCheck = dr.Count() > 0 ? Val.ToInt(dr.FirstOrDefault()["PARA_ID"]) : 0;
                    if (IntCheck == 0)
                    {
                        StrMessage = "Stone No : " + pStrStoneNo + " -> Column : [ " + pStrColumnName + " ] Has Invalid Values [ " + pStrValue + " ]";
                        IntCheck = -1;
                        return IntCheck;
                    }
                    else
                    {
                        return IntCheck;
                    }
                }
                else
                {
                    IntCheck = 0;
                    return IntCheck;
                }
            }
            catch (Exception ex)
            {
                IntCheck = -1;
                StrMessage = pStrStoneNo + " -> " + ex.Message;
                return IntCheck;
            }
        }


        float heightRatio = new float();
        float widthRatio = new float();
        int standardHeight, standardWidth;
        public void ResizeForm(Form objForm, int DesignerHeight, int DesignerWidth)
        {
            standardHeight = DesignerHeight;
            standardWidth = DesignerWidth;
            int presentHeight = Screen.PrimaryScreen.WorkingArea.Height;//.Bounds.Height;
                                                                        //int presentHeight = Screen.PrimaryScreen.Bounds.Height;
            int presentWidth = Screen.PrimaryScreen.Bounds.Width;
            heightRatio = (float)((float)presentHeight / (float)standardHeight);
            widthRatio = (float)((float)presentWidth / (float)standardWidth);
            objForm.AutoScaleMode = AutoScaleMode.None;
            objForm.Scale(new SizeF(widthRatio, heightRatio));
            foreach (Control c in objForm.Controls)
            {
                if (c.HasChildren)
                {
                    ResizeControlStore(c);
                }
                else
                {
                    //c.Font = new Font(c.Font.FontFamily, c.Font.Size * heightRatio, c.Font.Style, c.Font.Unit, ((byte)(0)));
                }
            }
            //objForm.Font = new Font(objForm.Font.FontFamily, objForm.Font.Size * heightRatio, objForm.Font.Style, objForm.Font.Unit, ((byte)(0)));
        }

        private void ResizeControlStore(Control objCtl)
        {
            if (objCtl.HasChildren)
            {
                foreach (Control cChildren in objCtl.Controls)
                {
                    if (cChildren.HasChildren)
                    {
                        ResizeControlStore(cChildren);
                    }
                    else
                    {
                        //cChildren.Font = new Font(cChildren.Font.FontFamily, cChildren.Font.Size * heightRatio, cChildren.Font.Style, cChildren.Font.Unit, ((byte)(0)));
                    }
                }
                //objCtl.Font = new Font(objCtl.Font.FontFamily, objCtl.Font.Size * heightRatio, objCtl.Font.Style, objCtl.Font.Unit, ((byte)(0)));
            }
            else
            {
                // objCtl.Font = new Font(objCtl.Font.FontFamily, objCtl.Font.Size * heightRatio, objCtl.Font.Style, objCtl.Font.Unit, ((byte)(0)));
            }
        }

        public void FindRapSaleRetEntry(DataRow Dr, int pIntRowIndex)
        {
            double Rapaport = 0;
            double CostDiscount = 0;
            double CostPricePerCarat = 0;
            double CostAmount = 0;
            double Carat = 0;

            double SaleDiscount = 0;
            double SalePricePerCarat = 0;
            double SaleAmount = 0;

            CostDiscount = Val.Val(Dr["SALEDISCOUNT"]);
            SaleDiscount = Val.Val(Dr["MEMODISCOUNT"]);
            Carat = Val.Val(Dr["RETURNCARAT"]);

            var drShape = (from DrPara in DtabPara.AsEnumerable()
                           where Val.ToString(DrPara["PARANAME"]).ToUpper() == Val.ToString(Dr["SHAPENAME"]).ToUpper()
                           select DrPara);

            string StrShape = drShape.Count() > 0 ? Val.ToString(drShape.FirstOrDefault()["RAPAVALUE"]) : "";

            var VarQry = (from DrRapaport in DtabRapaport.AsEnumerable()
                          where Val.ToString(DrRapaport["SHAPE"]).ToUpper() == StrShape.ToUpper()
                          && Val.ToString(DrRapaport["COLOR"]).ToUpper() == Val.ToString(Dr["COLORNAME"]).ToUpper()
                          && Val.ToString(DrRapaport["CLARITY"]).ToUpper() == Val.ToString(Dr["CLARITYNAME"]).ToUpper()
                          && Val.Val(Dr["RETURNCARAT"]) >= Val.Val(DrRapaport["FROMCARAT"])
                          && Val.Val(Dr["RETURNCARAT"]) <= Val.Val(DrRapaport["TOCARAT"])
                          select DrRapaport).ToList();

            if (VarQry.Any())
            {
                Rapaport = Val.Val(VarQry.FirstOrDefault()["NVALUE"]);
                CostPricePerCarat = Rapaport + (Rapaport * CostDiscount / 100);
                CostAmount = CostPricePerCarat * Carat;

                Rapaport = Val.Val(VarQry.FirstOrDefault()["NVALUE"]);
                SalePricePerCarat = Rapaport + (Rapaport * SaleDiscount / 100);
                SaleAmount = SalePricePerCarat * Carat;

                DTabMemoDetail.Rows[pIntRowIndex]["SALERAPAPORT"] = Rapaport;
                DTabMemoDetail.Rows[pIntRowIndex]["SALEPRICEPERCARAT"] = CostPricePerCarat;
                DTabMemoDetail.Rows[pIntRowIndex]["SALEAMOUNT"] = CostAmount;

                DTabMemoDetail.Rows[pIntRowIndex]["MEMORAPAPORT"] = Rapaport;
                DTabMemoDetail.Rows[pIntRowIndex]["MEMOPRICEPERCARAT"] = SalePricePerCarat;
                DTabMemoDetail.Rows[pIntRowIndex]["MEMOAMOUNT"] = SaleAmount;
                DTabMemoDetail.Rows[pIntRowIndex]["FMEMOPRICEPERCARAT"] = Math.Round(SalePricePerCarat * Val.Val(txtExcRate.Text), 2);
                DTabMemoDetail.Rows[pIntRowIndex]["FMEMOAMOUNT"] = Math.Round(SaleAmount * Val.Val(txtExcRate.Text), 2);

                DTabMemoDetail.AcceptChanges();
            }
            else //#p : 13-01-2020 : Coz When RapValue Not Found Its Not Calculate properly
            {
                double DouCarat = Val.Val(Dr["RETURNCARAT"]);
                double DouRapaport = Val.Val(Dr["SALERAPAPORT"]);
                double DouDiscount = Val.Val(Dr["SALEDISCOUNT"]);
                double DouPricePerCarat = Math.Round(DouRapaport + ((DouRapaport * DouDiscount) / 100), 2);
                double DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                CostPricePerCarat = Val.Val(Dr["SALEPRICEPERCARAT"]);
                CostAmount = CostPricePerCarat * Carat;

                Rapaport = Val.Val(Dr["MEMORAPAPORT"]);
                SalePricePerCarat = Val.Val(Dr["MEMOPRICEPERCARAT"]);
                SaleAmount = SalePricePerCarat * Carat;

                //if (cmbAccType.Text.ToUpper() == "EXPORT")
                //{
                //    SaleAmount = Math.Round(SaleAmount, 2);
                //}
                //else
                //{
                //    SaleAmount = Math.Round(SaleAmount, 3);
                //}

                if (cmbAccType.Text.ToUpper() == "LOCAL SALES")
                {
                    Rapaport = Val.Val(Dr["MEMORAPAPORT"]);
                    SalePricePerCarat = Val.Val(Dr["FMEMOPRICEPERCARAT"]);
                    SaleAmount = SalePricePerCarat * Carat;
                    SaleAmount = Math.Round(SaleAmount, 0);
                }
                else
                {
                    SaleAmount = Math.Round(SaleAmount, 2);
                    Rapaport = Val.Val(Dr["MEMORAPAPORT"]);
                    SalePricePerCarat = Val.Val(Dr["MEMOPRICEPERCARAT"]);
                    //SaleAmount = SalePricePerCarat * Carat;
                }

                DTabMemoDetail.Rows[pIntRowIndex]["SALERAPAPORT"] = Rapaport;
                DTabMemoDetail.Rows[pIntRowIndex]["SALEPRICEPERCARAT"] = CostPricePerCarat;
                DTabMemoDetail.Rows[pIntRowIndex]["SALEAMOUNT"] = CostAmount;

                DTabMemoDetail.Rows[pIntRowIndex]["MEMORAPAPORT"] = Rapaport;
                DTabMemoDetail.Rows[pIntRowIndex]["MEMOPRICEPERCARAT"] = SalePricePerCarat;
                DTabMemoDetail.Rows[pIntRowIndex]["MEMOAMOUNT"] = SaleAmount;
                DTabMemoDetail.Rows[pIntRowIndex]["FMEMOPRICEPERCARAT"] = Math.Round(SalePricePerCarat * Val.Val(txtExcRate.Text), 2);
                DTabMemoDetail.Rows[pIntRowIndex]["FMEMOAMOUNT"] = Math.Round(SaleAmount * Val.Val(txtExcRate.Text), 2);

                DTabMemoDetail.AcceptChanges();
            }

            //Calculation();
            CalculationNew();
            CalculationNewInvoiceEntry();
        }

        public double ToDouble50Round(double pDouValue)
        {
            string Str = Val.Format(pDouValue.ToString(), "#####0.00");
            string[] StrSplit = Str.ToString().Split('.');

            if (StrSplit.Length > 1)
            {
                double DouScal = Val.ToDouble(StrSplit[0]);
                double DouPrecision = Val.ToDouble(StrSplit[1]);

                if (Math.Round(DouPrecision, 2) >= 50)
                {
                    return DouScal = DouScal + 1;
                }
                else
                {
                    return DouScal;
                }
            }
            else
            {
                return pDouValue;
            }
        }

        private void ExportGridColHide()
        {
            try
            {
                if (cmbAccType.Text == "Export")
                {
                    GrdDetail.Columns["SALERAPAPORT"].Visible = false;
                    GrdDetail.Columns["SALEDISCOUNT"].Visible = false;
                    GrdDetail.Columns["SALEPRICEPERCARAT"].Visible = false;
                    GrdDetail.Columns["SALEAMOUNT"].Visible = false;
                    GrdDetail.Columns["FSALEPRICEPERCARAT"].Visible = false;
                    GrdDetail.Columns["FSALEAMOUNT"].Visible = false;
                    GrdDetail.Columns["MEMORAPAPORT"].Visible = false;
                    GrdDetail.Columns["MEMODISCOUNT"].Visible = false;
                    GrdDetail.Columns["JANGEDRAPAPORT"].Visible = false;
                    GrdDetail.Columns["JANGEDDISCOUNT"].Visible = false;
                    GrdDetail.Columns["JANGEDPRICEPERCARAT"].Visible = false;
                    GrdDetail.Columns["JANGEDAMOUNT"].Visible = false;
                    GrdDetail.Columns["FINALBUYERNAME"].Visible = false;
                    GrdDetail.Columns["NARRATIONNAME"].Visible = false;
                    GrdDetail.Columns["REMARK"].Visible = false;
                    GrdDetail.Bands["BANDJANGEDDETAIL"].Visible = false;
                }
                else
                {
                    GrdDetail.Columns["SALERAPAPORT"].Visible = true;
                    GrdDetail.Columns["SALEDISCOUNT"].Visible = true;
                    GrdDetail.Columns["SALEPRICEPERCARAT"].Visible = true;
                    GrdDetail.Columns["SALEAMOUNT"].Visible = true;
                    GrdDetail.Columns["FSALEPRICEPERCARAT"].Visible = true;
                    GrdDetail.Columns["FSALEAMOUNT"].Visible = true;
                    GrdDetail.Columns["MEMORAPAPORT"].Visible = true;
                    GrdDetail.Columns["MEMODISCOUNT"].Visible = true;
                    GrdDetail.Columns["JANGEDRAPAPORT"].Visible = true;
                    GrdDetail.Columns["JANGEDDISCOUNT"].Visible = true;
                    GrdDetail.Columns["JANGEDPRICEPERCARAT"].Visible = true;
                    GrdDetail.Columns["JANGEDAMOUNT"].Visible = true;
                    GrdDetail.Columns["FINALBUYERNAME"].Visible = true;
                    GrdDetail.Columns["NARRATIONNAME"].Visible = true;
                    GrdDetail.Columns["REMARK"].Visible = true;
                    GrdDetail.Bands["BANDJANGEDDETAIL"].Visible = true;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        private void BtnExport_Click_1(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    DataTable dtData = (DataTable)MainGrdDetail.DataSource;

                    if (dtData.Rows.Count <= 0)
                    {
                        Global.Message("Please Select Records to Export");
                        return;
                    }
                    var list = dtData.AsEnumerable().Select(r => r["PARTYSTOCKNO"].ToString());
                    string StrJangedNo = string.Join(",", list);
                    //var list = dtData.AsEnumerable().Select(r => r["MEMODETAIL_ID"].ToString());
                    // string StrJangedNo = Val.ToString(txtJangedNo.Text);
                    string StrFilePath = ReportListExportExcel(StrJangedNo);

                    if (StrFilePath != "")
                    {
                        if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(StrFilePath, "CMD");
                        }
                    }
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
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        public string ReportListExportExcel(string StrJangedNo, string StrFilePath = "")
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;


                //DataTable DTabDetail = ObjMemo.GetDataForSaleReport(strMemoDetailIds);
                DataTable DTabDetail = ObjMemo.GetDataForSaleReportNew(StrJangedNo);

                DataTable dtData = (DataTable)MainGrdDetail.DataSource;


                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }


                for (int i = 0; i < DTabDetail.Rows.Count; i++)
                {
                    string StrQuery = "PARTYSTOCKNO = '" + Val.ToString(DTabDetail.Rows[i]["ID"]) + "'";
                    DataRow[] UDROW = dtData.Select(StrQuery);

                    if (Val.ToDouble(DTabDetail.Rows[i]["RAP"]) == 0)
                    {
                        if (UDROW.Length > 0)
                        {
                            DTabDetail.Rows[i]["RAP"] = UDROW[0]["MEMORAPAPORT"];
                        }
                    }
                    if (Val.ToDouble(DTabDetail.Rows[i]["FDPC"]) == 0)
                    {
                        if (UDROW.Length > 0)
                        {
                            DTabDetail.Rows[i]["FDPC"] = UDROW[0]["MEMOPRICEPERCARAT"];
                        }
                    }
                    if (Val.ToDouble(DTabDetail.Rows[i]["FBACK"]) == 0)
                    {
                        if (UDROW.Length > 0)
                        {
                            DTabDetail.Rows[i]["FBACK"] = Convert.ToDecimal(UDROW[0]["MEMODISCOUNT"]);
                        }
                    }
                    if (Val.ToDouble(DTabDetail.Rows[i]["FTOT"]) == 0)
                    {
                        if (UDROW.Length > 0)
                        {
                            DTabDetail.Rows[i]["FTOT"] = UDROW[0]["MEMOAMOUNT"];
                        }
                    }
                }
                DTabDetail.AcceptChanges();
                this.Cursor = Cursors.WaitCursor;
                if (StrFilePath == "")
                {
                    SaveFileDialog svDialog = new SaveFileDialog();
                    svDialog.DefaultExt = ".xlsx";
                    svDialog.Title = "Export to Excel";
                    svDialog.FileName = mFormType.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
                    svDialog.Filter = "Excel File (*.xlsx)|*.xlsx ";

                    if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                    {
                        StrFilePath = svDialog.FileName;
                    }
                }

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color FontColor = Color.Red;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;
                this.Cursor = Cursors.WaitCursor;
                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add(mFormType.ToString());

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
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(Color.Transparent);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(Color.Black);

                    int CaratColumn = DTabDetail.Columns["CTS"].Ordinal + 1;
                    int ShapeColumn = DTabDetail.Columns["ID"].Ordinal + 1;

                    int MemoAmtColumn = DTabDetail.Columns["TOT"].Ordinal + 1;
                    int LiveAmtColumn = DTabDetail.Columns["FTOT"].Ordinal + 1;

                    int MemoPerCtsColumn = DTabDetail.Columns["DPC"].Ordinal + 1;
                    int LivePerCtsColumn = DTabDetail.Columns["FDPC"].Ordinal + 1;

                    int MemoDiscColumn = DTabDetail.Columns["BACK"].Ordinal + 1;
                    int LiveDiscColumn = DTabDetail.Columns["FBACK"].Ordinal + 1;

                    int RAPColumn = DTabDetail.Columns["RAP"].Ordinal + 1;
                    int DateColumn = DTabDetail.Columns["REP_DATE"].Ordinal + 1;

                    EndRow = EndRow + 2;
                    worksheet.Cells[EndRow, 1, EndRow, 1].Value = "TOTAL";



                    string CaratCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["CTS"].Ordinal + 1);
                    string MemoAmtCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["TOT"].Ordinal + 1);
                    string LiveAmtCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["FTOT"].Ordinal + 1);
                    string RAPCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RAP"].Ordinal + 1);

                    int IntTotRow = DTabDetail.Rows.Count + 1;

                    StartRow = StartRow + 1;

                    worksheet.Cells[EndRow, ShapeColumn, EndRow, ShapeColumn].Formula = "SUBTOTAL(2," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, CaratColumn, EndRow, CaratColumn].Formula = "SUBTOTAL(9," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, MemoAmtColumn, EndRow, MemoAmtColumn].Formula = "SUBTOTAL(9," + MemoAmtCol + StartRow + ":" + MemoAmtCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, LiveAmtColumn, EndRow, LiveAmtColumn].Formula = "SUBTOTAL(9," + LiveAmtCol + StartRow + ":" + LiveAmtCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, MemoPerCtsColumn, EndRow, MemoPerCtsColumn].Formula = "ROUND(" + MemoAmtCol + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, LivePerCtsColumn, EndRow, LivePerCtsColumn].Formula = "ROUND(" + LiveAmtCol + EndRow + "/" + CaratCol + EndRow + ",0)";

                    worksheet.Cells[EndRow, MemoDiscColumn, EndRow, MemoDiscColumn].Formula =
                                         "=ROUND(-100 + (" + MemoAmtCol + EndRow + "/SUMPRODUCT(" + RAPCol + StartRow + ":" + RAPCol + IntTotRow + ", "
                                         + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")) * 100, 2)";

                    worksheet.Cells[EndRow, LiveDiscColumn, EndRow, LiveDiscColumn].Formula =
                   "=ROUND(-100 + (" + LiveAmtCol + EndRow + "/SUMPRODUCT(" + RAPCol + StartRow + ":" + RAPCol + IntTotRow + ", " + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")) * 100, 2)";

                    //worksheet.Cells[EndRow, MemoDiscColumn, EndRow, MemoDiscColumn].Formula ="100 - (" + MemoAmtCol + EndRow + "/ (SUBTOTAL(9, " + RAPColumn + StartRow + ":" + CaratCol + IntTotRow + ")) * 100)";
                    // worksheet.Cells[EndRow, LiveDiscColumn, EndRow, LiveDiscColumn].Formula = "100 - (" + LiveAmtCol + EndRow + "/ (SUBTOTAL(9, " + RAPColumn + StartRow + ":" + CaratCol + IntTotRow + ")) * 100)";
                    worksheet.Cells[EndRow + 1, 12, EndRow + 1, 12].Value = "LESS";
                    worksheet.Cells[EndRow + 1, 13, EndRow + 1, 13].Value = Val.ToString(txtAdatPer.Text);
                    worksheet.Cells[EndRow + 1, 14, EndRow + 1, 14].Formula = "ROUND(" + LiveAmtCol + EndRow + "-" + LiveAmtCol + EndRow + "*" + txtAdatPer.Text + "/100 ,2)";

                    int EndRow1 = EndRow + 1;
                    worksheet.Cells[EndRow + 2, 12, EndRow + 2, 12].Value = "RATE";
                    worksheet.Cells[EndRow + 2, 13, EndRow + 2, 13].Value = Val.ToString(txtExcRate.Text);
                    worksheet.Cells[EndRow + 2, 14, EndRow + 2, 14].Formula = "ROUND(" + LiveAmtCol + EndRow1 + "*" + Val.ToDouble(txtExcRate.Text) + ",2)";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    worksheet.Cells[StartRow, MemoDiscColumn, EndRow, MemoDiscColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, LiveDiscColumn, EndRow, LiveDiscColumn].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;


                    #endregion

                    worksheet.Column(17).Hidden = true; //Cmnt : Coz due to AutoFitcolumns column can not hide
                    worksheet.Column(24).Hidden = true;

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

        private void deleteStoneFromDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.SALEINVOICE && lblMode.Text.ToUpper() == "ADD MODE")
                {
                    DTabMemoDetail.Rows.RemoveAt(GrdDetail.FocusedRowHandle);
                    DTabMemoDetail.AcceptChanges();
                    CalculationNew();
                    BulkPriceCalculation();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        private void BtnClientPreview_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtTerms.Text.Length == 0)
                {
                    Global.Message("Terms Is Required");
                    txtTerms.Focus();
                    return;
                }

                if (Val.ToString(cmbBillType.SelectedItem) == "None")
                {
                    Global.Message("Please Select BillType");
                    cmbBillType.Focus();
                    return;
                }

                //string StrFilePath = Application.StartupPath + "\\ClientPreview" + "\\ClientPreview_" + Val.ToString(txtJangedNo.Text) + ".txt ";
                DataTable DTabMessage = (DataTable)MainGrdDetail.DataSource;
                string directoryPath = Path.Combine(Application.StartupPath, "ClientPreview");
                string fileName = Val.ToString(txtJangedNo.Text) + ".txt";
                fileName = fileName.Replace("/", "-");
                string StrFilePath = Path.Combine(directoryPath, fileName);


                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (StrFilePath != "")
                {
                    using (StreamWriter sw = File.CreateText(StrFilePath))
                    {

                        DataRow DR = ObjMast.GetPartyDetails(Val.ToGuid(txtBillingParty.Tag));
                        sw.WriteLine("MAHANT EXPORTS" + "  " + "Date : " + DTPMemoDate.Value.ToShortDateString());
                        sw.WriteLine("Bill Type : " + Val.ToString(cmbBillType.SelectedItem));
                        sw.WriteLine();
                        sw.WriteLine(txtTerms.Text);
                        sw.WriteLine("Name : " + Val.ToString(txtBillingParty.Text).ToUpper());
                        sw.WriteLine("Broker : " + Val.ToString(txtBroker.Text));
                        sw.WriteLine("Phone No : " + Val.ToString(DR["MobileNo1"]));
                        sw.WriteLine("Client ID : " + Val.ToString(DR["LedgerCode"]) + "   Order No : " + Val.ToString(txtJangedNo.Text));
                        sw.WriteLine("Bank Rate : " + Val.ToString(txtExcRate.Text) +
                                     "   TOTAL PCS : " + Val.ToDecimal(txtTotalPcs.Text).ToString("0") +
                                     "   T.WT : " + Val.ToDecimal(txtTotalCarat.Text).ToString("0.00"));

                        for (int i = 0; i < DTabMessage.Rows.Count; i++)
                        {
                            sw.WriteLine("-----------------------------------------------------");
                            sw.WriteLine("Stone ID: " + DTabMessage.Rows[i]["PARTYSTOCKNO"].ToString() +
                      "  Certi No: " + DTabMessage.Rows[i]["LABREPORTNO"]);
                            sw.WriteLine("Carat: " + Val.ToDecimal(DTabMessage.Rows[i]["CARAT"]).ToString("0.00") + "  " +
                                         DTabMessage.Rows[i]["SHAPENAME"] + "  " +
                                         DTabMessage.Rows[i]["COLORNAME"] + "  " +
                                         DTabMessage.Rows[i]["CLARITYNAME"]);
                            sw.WriteLine("Disc%: " + Val.ToDecimal(DTabMessage.Rows[i]["MEMODISCOUNT"]).ToString("N3") +
                                         "      $/Cts: " + Val.ToDecimal(DTabMessage.Rows[i]["MEMOPRICEPERCARAT"]).ToString("N3") +
                                         "      P/CTS: " + Val.ToDecimal(DTabMessage.Rows[i]["FMEMOPRICEPERCARAT"]).ToString("N3"));
                            sw.WriteLine("Tot $: " + Val.ToDecimal(DTabMessage.Rows[i]["MEMOAMOUNT"]).ToString("N2") +
                                         "      T/AT : " + Val.ToDecimal(DTabMessage.Rows[i]["FMEMOAMOUNT"]).ToString("N2"));
                        }
                        sw.WriteLine("-----------------------------------------------------");
                        sw.WriteLine("Total $ : " + Val.Val(txtNetAmount.Text).ToString("N0") + "    " + "T.AMOUNT-" + Val.Val(txtNetAmountFE.Text).ToString("N0"));
                        sw.WriteLine("-----------------------------------------------------");
                        //sw.WriteLine("THANK YOU");
                        //sw.WriteLine("WWW.AxoneDiasales.COM");
                        //sw.WriteLine("As per our new rules and regulation");
                        //sw.WriteLine("our company have updated terms & conditions");
                        //sw.WriteLine("1---From 31st March,2023  0.50 cash charge will be taken");
                        //sw.WriteLine("2---You will receive notification from RIJIYA GEMS as goods reached at Hongkong.");
                        //sw.WriteLine("3---Need to collect goods within 15 days after the goods arrives in Hongkong ,Else order will get cancel and return back to India without any intimation and 2% charge will be taken.");
                        //sw.WriteLine("4---Once goods shipped , Order cannot be cancelled.");
                        //sw.WriteLine("5---Please notify us, As you get the goods");
                        //sw.WriteLine("so please cooperate");

                    }

                    System.Diagnostics.Process.Start(StrFilePath, "CMD");
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }
    }
}
