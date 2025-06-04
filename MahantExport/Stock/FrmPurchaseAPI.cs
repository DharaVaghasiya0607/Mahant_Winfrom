using MahantExport.Class;
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows.Forms;
using System.Xml;
using MahantExport.Utility;

namespace MahantExport.Stock
{
    public partial class FrmPurchaseAPI : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();

        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();

        DataTable DtabInvoice = new DataTable();

        DataTable DTabMemoDetail = new DataTable();
        DataTable DTabMemo = new DataTable();
        DataTable DtabPara = new DataTable();
        DataTable DtabRapaport = new DataTable();

        DataTable DtabProcess = new DataTable();

        BODevGridSelection ObjGridSelection;

        //HINA - START
        string mStrStockType = "";
        //HINA - END

        #region Property Settings

        public FrmPurchaseAPI()
        {
            InitializeComponent();
        }

        public FORMTYPE mFormType = FORMTYPE.PURCHASEISSUE;
        public enum FORMTYPE
        {
            PURCHASEISSUE = 1,
            PURCHASERETURN = 2
        }

        //HINA - START
        //public void ShowForm(FORMTYPE pFormType, DataTable pDtInvoice)  //Call When Double Click On Current Stock Color Clarity Wise Report Data
        public void ShowForm(FORMTYPE pFormType, DataTable pDtInvoice, string pStockType = "ALL", string StrMainMemo_ID = "")  //Call When Double Click On Current Stock Color Clarity Wise Report Data
        //HINA - END
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            BtnReturn.Enabled = false;
            BtnOtherActivity.Enabled = false;

            mFormType = pFormType;
            //HINA - START
            //DataSet DS = ObjMemo.GetMemoListData(-1, null, null, "", "", "", 0, "", 0, "", "", "");
            mStrStockType = pStockType;
            DataSet DS = ObjMemo.GetMemoListData(-1, null, null, "", "", "", 0, "", 0, "", "", "", mStrStockType, false, -1);
            //HINA - END

            StrMainMemo_ID = Val.ToString(StrMainMemo_ID).Trim().Equals(string.Empty) ? Val.ToString(Guid.Empty) : StrMainMemo_ID;

            DTabMemo = DS.Tables[0];
            DTabMemoDetail = DS.Tables[1];

            MainGrdDetail.DataSource = DTabMemoDetail;
            MainGrdDetail.Refresh();

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
                GrdDetail.Columns["SALEDISCOUNT"].Visible = true;
                GrdDetail.Columns["MEMODISCOUNT"].Visible = true;

                GrdDetail.Columns["RETURNPCS"].Visible = false;
                GrdDetail.Columns["RETURNCARAT"].Visible = false;


            }
            //End : #P : 13-01-2020

            FillControlName();

            DTabMemoDetail.Rows.Clear();

            if (pDtInvoice != null)
            {
                foreach (DataRow DRow in pDtInvoice.Rows)
                {
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

                    if (Val.Val(DRow["MEMORAPAPORT"]) == 0)
                    {
                        DRNew["MEMORAPAPORT"] = DRow["SALERAPAPORT"];
                        DRNew["MEMODISCOUNT"] = DRow["SALEDISCOUNT"];
                        DRNew["MEMOPRICEPERCARAT"] = DRow["SALEPRICEPERCARAT"];
                        DRNew["MEMOAMOUNT"] = DRow["SALEAMOUNT"];
                        DRNew["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["SALEPRICEPERCARAT"]), 2);
                        DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["SALEAMOUNT"]), 2);
                    }
                    else
                    {
                        DRNew["MEMORAPAPORT"] = DRow["MEMORAPAPORT"];
                        DRNew["MEMODISCOUNT"] = DRow["MEMODISCOUNT"];
                        DRNew["MEMOPRICEPERCARAT"] = DRow["MEMOPRICEPERCARAT"];
                        DRNew["MEMOAMOUNT"] = DRow["MEMOAMOUNT"];
                        DRNew["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOPRICEPERCARAT"]), 2);
                        DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"]), 2);
                    }


                    DRNew["STATUS"] = "PENDING";
                    DRNew["REMARK"] = "";
                    DRNew["LOCATION_ID"] = DRow["LOCATION_ID"];
                    DRNew["LOCATIONNAME"] = DRow["LOCATIONNAME"];
                    DRNew["SIZE_ID"] = DRow["SIZE_ID"];
                    DRNew["SIZENAME"] = DRow["SIZENAME"];
                    DRNew["LAB_ID"] = DRow["LAB_ID"];
                    DRNew["LABNAME"] = DRow["LABNAME"];
                    DRNew["LABREPORTNO"] = DRow["LABREPORTNO"];
                    //DRNew["MAINMEMODETAIL_ID"] = DRow["MAINMEMODETAIL_ID"];

                    DRNew["MAINMEMO_ID"] = StrMainMemo_ID;


                    DTabMemoDetail.Rows.Add(DRNew);
                }

                string StrBillParty = Val.ToString(pDtInvoice.Rows[0]["BILLPARTY_ID"]);
                string StrShipParty = Val.ToString(pDtInvoice.Rows[0]["SHIPPARTY_ID"]);

                if (StrBillParty != "" && StrBillParty != "00000000-0000-0000-0000-000000000000" && Val.ToString(pDtInvoice.Rows[0]["STATUS"]) != "AVAILABLE") //Changes : Pinali : 07-11-2019 : Add Only status Condtn
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
            }

            DataTable DTabCurr = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);

            foreach (DataRow DRow in DTabCurr.Rows)
            {
                if (Val.ToString(DRow["CURRENCY_ID"]) == "2")
                {
                    txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                    txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);

                    lblGrossAmountFESymbol.Text = Val.ToString(DRow["SYMBOL"]);
                    lblInsuranceAmountFESymbol.Text = Val.ToString(DRow["SYMBOL"]);
                    lblShippingAmountFESymbol.Text = Val.ToString(DRow["SYMBOL"]);
                    lblDiscAmountFESymbol.Text = Val.ToString(DRow["SYMBOL"]);
                    lblGSTAmountFESymbol.Text = Val.ToString(DRow["SYMBOL"]);
                    lblNetAmountFESymbol.Text = Val.ToString(DRow["SYMBOL"]);
                    GrdDetail.Columns["FMEMOPRICEPERCARAT"].Caption = "  " + Val.ToString(DRow["SYMBOL"]) + "/Cts   借货单价￥";
                    GrdDetail.Columns["FMEMOAMOUNT"].Caption = "  Amt (" + Val.ToString(DRow["SYMBOL"]) + ")   借货金额￥";
                }
            }
            DTabCurr.Dispose();
            DTabCurr = null;

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




            lblSource.Text = "SOFTWARE";
            CmbPaymentMode.SelectedIndex = 0;
            CmbDeliveryType.SelectedIndex = 0;


            lblMode.Text = "Add Mode";
            lblMemoNo.Text = string.Empty;
            DTPMemoDate.Value = DateTime.Now;
            txtSellerName.Text = BOConfiguration.gEmployeeProperty.LEDGERNAME;
            txtSellerName.Tag = BOConfiguration.gEmployeeProperty.LEDGER_ID;

            txtCurrency_Validated(null, null);
            //Calculation();
            CalculationNew();
            DTPMemoDate.Focus();
        }

        //HINA - START
        // public void ShowForm(string pStrMemoID)  //Call When Double Click On Current Stock Color Clarity Wise Report Data
        public void ShowForm(string pStrMemoID, string pStrStockType = "ALL")  //Call When Double Click On Current Stock Color Clarity Wise Report Data
        //HINA - END
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

                //HINA - START
                //DataSet DS = ObjMemo.GetMemoListData(0, null, null, "", "", "", 0, "", 0, "", "ALL", pStrMemoID);
                mStrStockType = pStrStockType;
                DataSet DS = ObjMemo.GetMemoListData(0, null, null, "", "", "", 0, "", 0, "", "ALL", pStrMemoID, mStrStockType, false, -1);
                //HINA - END

                lblStockType.Text = mStrStockType + " STOCK";

                DTabMemo = DS.Tables[0];
                DTabMemoDetail = DS.Tables[1];

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

                DTPTermsDate.Text = DateTime.Parse(DRow["TERMSDATE"].ToString()).ToShortDateString();

                txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);
                txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                txtExcRate.Text = Val.ToString(DRow["EXCRATE"]);
                lblGrossAmountFESymbol.Text = Val.ToString(DRow["SYMBOL"]);
                lblInsuranceAmountFESymbol.Text = Val.ToString(DRow["SYMBOL"]);
                lblShippingAmountFESymbol.Text = Val.ToString(DRow["SYMBOL"]);
                lblDiscAmountFESymbol.Text = Val.ToString(DRow["SYMBOL"]);
                lblGSTAmountFESymbol.Text = Val.ToString(DRow["SYMBOL"]);
                lblNetAmountFESymbol.Text = Val.ToString(DRow["SYMBOL"]);
                GrdDetail.Columns["FMEMOPRICEPERCARAT"].Caption = "  " + Val.ToString(DRow["SYMBOL"]) + "/Cts    借货单价￥";
                GrdDetail.Columns["FMEMOAMOUNT"].Caption = "  Amt (" + Val.ToString(DRow["SYMBOL"]) + ")   借货金额￥";


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

                txtNetAmount.Text = Val.ToString(DRow["NETAMOUNT"]);
                txtNetAmountFE.Text = Val.ToString(DRow["FNETAMOUNT"]);

                txtRemark.Text = Val.ToString(DRow["REMARK"]);

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
                    GrdDetail.Columns["SALEDISCOUNT"].Visible = true;
                    GrdDetail.Columns["MEMODISCOUNT"].Visible = true;

                    GrdDetail.Columns["RETURNPCS"].Visible = false;
                    GrdDetail.Columns["RETURNCARAT"].Visible = false;

                }
                //End : #P : 13-01-2020


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



                DTPMemoDate.Focus();
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
            //ObjFormEvent.FormKeyDown = true;
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

                lblInvoiceNo.Text = Val.ToString(UDRow[0]["BILLNOPREFIX"]);
                lblInvoiceDate.Text = Val.ToString(UDRow[0]["BILLDATEPREFIX"]);
                BtnSave.Enabled = true;
            }
            else
            {
                BtnSave.Enabled = false;
                Global.Message("NOT VALID ISSUE PROCESS FOUND");
                this.Text = string.Empty;
                lblTitle.Text = string.Empty;

                lblTitle.Tag = 0;

                lblInvoiceNo.Text = string.Empty; ;
                lblInvoiceDate.Text = string.Empty; ;
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
            DtabProcess = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PROCESS);

            CmbMemoType.Items.Clear();

            GrdDetail.Bands["BANDMEMOPRICE"].Fixed = FixedStyle.Right;

            if (mFormType == FORMTYPE.PURCHASEISSUE)
            {
                DtabRapaport = new BOTRN_PriceRevised().GetOriginalRapData("GETCURRENTRAPAPORT", "", "", 0, 0);
                DtabPara = new BOMST_Parameter().GetParameterData();

                //BtnNewRow.Visible = true;
                PnlNewRow.Visible = true;

                GetSelectedProcessIssue("PURCHASE");

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = mStrStockType == "SINGLE" ? true : false;

                GrdDetail.Columns["SHAPENAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["COLORNAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["CLARITYNAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["CUTNAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["POLNAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["SYMNAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["FLNAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = true;
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

                GrdDetail.Columns["BALANCEPCS"].Visible = true;
                GrdDetail.Columns["BALANCECARAT"].Visible = true;

                GrdDetail.Columns["PCS"].Visible = false;
                GrdDetail.Columns["CARAT"].Visible = false;

                GrdDetail.Columns["STATUS"].Visible = false;

                GrdDetail.Columns["SALERAPAPORT"].Caption = "Cost Rapa($)   成本报价$";
                GrdDetail.Columns["SALEDISCOUNT"].Caption = "Cost Disc($)   成本折扣$";
                GrdDetail.Columns["SALEPRICEPERCARAT"].Caption = "Cost $/Cts($)   成本单价$";
                GrdDetail.Columns["SALEAMOUNT"].Caption = "Cost Amt($)   成本金额$";

                GrdDetail.Columns["MEMORAPAPORT"].Caption = "Sale Rapa($)   销售报价$";
                GrdDetail.Columns["MEMODISCOUNT"].Caption = "Sale Disc($)   销售折扣$";
                GrdDetail.Columns["MEMOPRICEPERCARAT"].Caption = "Sale $/Cts($)   销售价$";
                GrdDetail.Columns["MEMOAMOUNT"].Caption = "Sale Amt($)   销售金额$";

                GrdDetail.Columns["BALANCEPCS"].Caption = "Pcs   粒数";
                GrdDetail.Columns["BALANCECARAT"].Caption = "Carat   重量";

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
                PnlAvgDetail.Visible = false; //Add : Pinali : 10-10-2019

            }
            else if (mFormType == FORMTYPE.PURCHASERETURN)
            {
                GetSelectedProcessIssue("PURCHASE RETURN");

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = true;

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

                PnlAvgDetail.Visible = false; //Add : Pinali : 10-10-2019
            }
            CmbMemoType.SelectedIndex = 0;
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

                DTPMemoDate.Focus();
                DTabMemoDetail.Rows.Clear();
                MainGrdDetail.Refresh();
                //Calculation();
                CalculationNew();

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
                    else
                        FrmSearch.mDTab = DtabParty;
                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID,PARTYTYPE";
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

                        txtShippingParty.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtShippingParty.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                        txtSAddress1.Text = Val.ToString(FrmSearch.DRow["SHIPPINGADDRESS1"]);
                        txtSAddress2.Text = Val.ToString(FrmSearch.DRow["SHIPPINGADDRESS2"]);
                        txtSAddress3.Text = Val.ToString(FrmSearch.DRow["SHIPPINGADDRESS3"]);
                        txtSCity.Text = Val.ToString(FrmSearch.DRow["SHIPPINGCITY"]);
                        txtSCountry.Tag = Val.ToString(FrmSearch.DRow["SHIPPINGCOUNTRY_ID"]);
                        txtSCountry.Text = Val.ToString(FrmSearch.DRow["SHIPPINGCOUNTRYNAME"]);
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
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);

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

                if (Val.Val(txtNetAmount.Text) <= 0)
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
                //if (txtShippingParty.Text.Length == 0)
                //{
                //    Global.Message("Shipping Party Is Required");
                //    txtShippingParty.Focus();
                //    return;
                //}
                if (txtBCountry.Text.Length == 0)
                {
                    Global.Message("Billing Country Is Required");
                    txtBCountry.Focus();
                    return;
                }
                //if (txtSCountry.Text.Length == 0)
                //{
                //    Global.Message("Shipping Country Is Required");
                //    txtSCountry.Focus();
                //    return;
                //}
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
                    DataTable DTab = ObjMemo.ValDelete(Property);
                    if (DTab.Rows.Count != 0)
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
                }


                Property.JANGEDNOSTR = txtJangedNo.Text;
                Property.MEMONO = Val.ToInt64(lblMemoNo.Text);
                Property.MEMOTYPE = Val.ToString(CmbMemoType.SelectedItem);
                Property.MEMODATE = Val.SqlDate(DTPMemoDate.Text);

                Property.BILLINGPARTY_ID = Val.ToString(txtBillingParty.Tag);
                Property.SHIPPINGPARTY_ID = Val.ToString(txtShippingParty.Tag);

                if (txtBroker.Text.Trim().Length != 0)
                {
                    Property.BROKER_ID = Val.ToString(txtBroker.Tag);
                    Property.BROKERBASEPER = Val.Val(txtBaseBrokeragePer.Text);
                    Property.BROKERPROFITPER = Val.Val(txtProfitBrokeragePer.Text);
                }
                else
                {
                    Property.BROKER_ID = null;
                    Property.BROKERBASEPER = 0.00;
                    Property.BROKERPROFITPER = 0.00;
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

                Property.SHIPPINGADDRESS1 = Val.ToString(txtBAddress1.Text);
                Property.SHIPPINGADDRESS2 = Val.ToString(txtBAddress2.Text);
                Property.SHIPPINGADDRESS3 = Val.ToString(txtBAddress3.Text);
                Property.SHIPPINGCOUNTRY_ID = Val.ToInt32(txtBCountry.Tag);
                Property.SHIPPINGSTATE = Val.ToString(txtBState.Text);
                Property.SHIPPINGCITY = Val.ToString(txtBCity.Text);
                Property.SHIPPINGZIPCODE = Val.ToString(txtBZipCode.Text);

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
                Property.PROCESS_ID = Val.ToInt32(lblTitle.Tag);
                Property.PROCESSNAME = Val.ToString(lblTitle.Text);

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

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        SqlConnection cn_T = new SqlConnection(BOConfiguration.ConnectionString);
                        if (cn_T.State == ConnectionState.Open) { cn_T.Close(); } cn_T.Open();


                        Property = ObjMemo.SaveMemoEntry(cn_T, Property, MemoEntryDetailForXML, lblMode.Text, "", "");

                        txtJangedNo.Text = Property.ReturnValueJanged;
                        lblMemoNo.Text = Property.ReturnValue;

                        ReturnMessageDesc = Property.ReturnMessageDesc;
                        ReturnMessageType = Property.ReturnMessageType;

                        Property = null;
                    }
                    catch(Exception Ex)
                    {
                        Global.Message(Ex.Message.ToString());
                    }
                }
                //Global.Message(ReturnMessageDesc); /Comment : Pinali : 05-11-2019
                if (ReturnMessageType == "SUCCESS" && (mFormType == FORMTYPE.PURCHASEISSUE))
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
                        GrdDetail.SetFocusedRowCellValue("SHAPENAME", Val.ToString(FrmSearch.DRow["SHAPENAME"]));
                        GrdDetail.SetFocusedRowCellValue("SHAPE_ID", Val.ToString(FrmSearch.DRow["SHAPE_ID"]));
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
                        GrdDetail.SetFocusedRowCellValue("CUTNAME", Val.ToString(FrmSearch.DRow["CUTNAME"]));
                        GrdDetail.SetFocusedRowCellValue("CUT_ID", Val.ToString(FrmSearch.DRow["CUT_ID"]));
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
                        GrdDetail.SetFocusedRowCellValue("POLNAME", Val.ToString(FrmSearch.DRow["POLNAME"]));
                        GrdDetail.SetFocusedRowCellValue("POL_ID", Val.ToString(FrmSearch.DRow["POL_ID"]));
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
                        GrdDetail.SetFocusedRowCellValue("SYMNAME", Val.ToString(FrmSearch.DRow["SYMNAME"]));
                        GrdDetail.SetFocusedRowCellValue("SYM_ID", Val.ToString(FrmSearch.DRow["SYM_ID"]));
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
                    dr["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DrDetail["SALEAMOUNT"]), 2);
                }
                else
                {
                    dr["MEMORAPAPORT"] = DrDetail["MEMORAPAPORT"];
                    dr["MEMODISCOUNT"] = DrDetail["MEMODISCOUNT"];
                    dr["MEMOPRICEPERCARAT"] = DrDetail["MEMOPRICEPERCARAT"];
                    dr["MEMOAMOUNT"] = DrDetail["MEMOAMOUNT"];
                    dr["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DrDetail["MEMOPRICEPERCARAT"]), 2);
                    dr["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DrDetail["MEMOAMOUNT"]), 2);
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

        public void CalculationNew() // Add : Pinali : 29-08-2019
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

                if ((mFormType != FORMTYPE.PURCHASEISSUE) && (DouSaleRapAmt != 0)) // Cndtn Changed & add Else if : Pinali : 29-08-2019
                {
                    txtMemoAvgDisc.Text = Val.Format(((DouMemoTotalAmt - DouSaleRapAmt) / DouSaleRapAmt) * 100, "########0.00");
                }
                else if ((mFormType == FORMTYPE.PURCHASEISSUE) && (DouSaleRapAmt != 0)) // For Purchase Invoice
                {
                    txtMemoAvgDisc.Text = Val.Format(((DouSaleAmt - DouSaleRapAmt) / DouSaleRapAmt) * 100, "########0.00");
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

                double DouInsAmt = Math.Round(Val.Val(txtGrossAmount.Text) * Val.Val(txtInsurancePer.Text) / 100, 4);
                double DouShipAmt = Math.Round(Val.Val(txtGrossAmount.Text) * Val.Val(txtShippingPer.Text) / 100, 4);



                txtDiscAmount.Text = Val.Format(DouDiscAmt, "########0.00");
                txtInsuranceAmount.Text = Val.Format(DouInsAmt, "########0.00");
                txtShippingAmount.Text = Val.Format(DouShipAmt, "########0.00");
                txtGSTAmount.Text = Val.Format(DouGSTAmt, "########0.00");
                double DouNetAmt = Val.Val(txtGrossAmount.Text) + DouDiscAmt + DouInsAmt + DouShipAmt + DouGSTAmt;
                txtNetAmount.Text = Val.Format(DouNetAmt, "########0.00");

                //double DouDiscAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouDiscAmt, 2); ---#P : 25-12-2019
                double DouDiscAmtFE = Math.Round(Val.Val(txtDiscAmountFE.Text), 2);

                //double DouGSTAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouGSTAmt, 2); ---#P : 25-12-2019
                double DouGSTAmtFE = Math.Round(Val.Val(txtGSTAmountFE.Text), 2);

                double DouInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouInsAmt, 2);
                double DouShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouShipAmt, 2);


                txtDiscAmountFE.Text = Val.Format(DouDiscAmtFE, "########0.00");
                txtInsuranceAmountFE.Text = Val.Format(DouInsAmtFE, "########0.00");
                txtShippingAmountFE.Text = Val.Format(DouShipAmtFE, "########0.00");
                txtGSTAmountFE.Text = Val.Format(DouGSTAmtFE, "########0.00");
                double DouNetAmtFE = Math.Round(Val.Val(txtGrossAmountFE.Text) + DouDiscAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE, 0);
                txtNetAmountFE.Text = Val.Format(DouNetAmtFE, "########0.00");

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
                txtDiscAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtDiscPer.Text)) / 100, 2).ToString();
                txtDiscAmountFE.Text = Val.Format(Math.Round(Val.Val(txtDiscAmount.Text) / Val.Val(txtExcRate.Text), 2), "########0.00");
            }
            else
            {
                txtDiscPer.Text = "0.00";
                txtDiscAmount.Text = "0.00";
                txtDiscAmountFE.Text = "0.00";
            }
            //End : #P : 25-12-2019

            //Calculation();
            CalculationNew();
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

                        lblGrossAmountFESymbol.Text = Val.ToString(FrmSearch.DRow["SYMBOL"]);
                        lblInsuranceAmountFESymbol.Text = Val.ToString(FrmSearch.DRow["SYMBOL"]);
                        lblShippingAmountFESymbol.Text = Val.ToString(FrmSearch.DRow["SYMBOL"]);
                        lblDiscAmountFESymbol.Text = Val.ToString(FrmSearch.DRow["SYMBOL"]);
                        lblGSTAmountFESymbol.Text = Val.ToString(FrmSearch.DRow["SYMBOL"]);
                        lblNetAmountFESymbol.Text = Val.ToString(FrmSearch.DRow["SYMBOL"]);


                        GrdDetail.Columns["FMEMOPRICEPERCARAT"].Caption = "  " + Val.ToString(FrmSearch.DRow["SYMBOL"]) + "/Cts   借货单价￥";
                        GrdDetail.Columns["FMEMOAMOUNT"].Caption = "  Amt (" + Val.ToString(FrmSearch.DRow["SYMBOL"]) + ")   借货金额￥";

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
            this.Cursor = Cursors.WaitCursor;
            txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString()), mFormType.ToString()).ToString();
            txtExcRate_Validated(null, null);
            this.Cursor = Cursors.Default;
        }

        private void lblLatestRate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Text), mFormType.ToString()).ToString();
            this.Cursor = Cursors.Default;
        }

        private void txtExcRate_Validated(object sender, EventArgs e)
        {
            for (int IntI = 0; IntI < GrdDetail.RowCount; IntI++)
            {
                DataRow DRow = GrdDetail.GetDataRow(IntI);
                DRow["FMEMOPRICEPERCARAT"] = Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOPRICEPERCARAT"]);
                DRow["FMEMOAMOUNT"] = Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"]);
            }
            DTabMemoDetail.AcceptChanges();
            //Calculation();
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
                    MemoEntryProperty Property = new MemoEntryProperty();
                    Property.MEMO_ID = Val.ToString(lblMemoNo.Tag);

                    FrmPassword FrmPassword = new FrmPassword();
                    FrmPassword.ShowForm(ObjPer.PASSWORD);

                    if (Property.MEMO_ID == "")
                    {
                        Property = null;
                        Global.Message("No Memo Found For Delete");
                        return;
                    }

                    DataTable DTab = ObjMemo.ValDelete(Property);
                    if (DTab.Rows.Count != 0)
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
                    Property = ObjMemo.Delete(Property);
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

            if (ChkNoBackPrint.Checked)
            {
                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowForm("InvoicePrintWithoutBack", DTab);
                this.Cursor = Cursors.Default;
            }
            else
            {
                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowForm("InvoicePrintWithBack", DTab);
                this.Cursor = Cursors.Default;
            }
        }

        private void txtGSTAmount_Validated(object sender, EventArgs e)
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

            //double DouDiscAmt = Val.Val(txtDiscAmount.Text);
            //double DouInsAmt = Val.Val(txtInsuranceAmount.Text);
            //double DouShipAmt = Val.Val(txtShippingAmount.Text);
            //double DouGSTAmt = Val.Val(txtGSTAmount.Text);

            //double DouNetAmt = Val.Val(txtGrossAmount.Text) + DouDiscAmt + DouInsAmt + DouShipAmt + DouGSTAmt;
            //txtNetAmount.Text = Val.Format(DouNetAmt, "########0.00");

            //double DouDiscAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouDiscAmt, 2);
            //double DouInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouInsAmt, 2);
            //double DouShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouShipAmt, 2);
            //double DouGSTAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouGSTAmt, 2);

            //txtDiscAmountFE.Text = Val.Format(DouDiscAmtFE, "########0.00");
            //txtInsuranceAmountFE.Text = Val.Format(DouInsAmtFE, "########0.00");
            //txtShippingAmountFE.Text = Val.Format(DouShipAmtFE, "########0.00");
            //txtGSTAmountFE.Text = Val.Format(DouGSTAmtFE, "########0.00");
            //double DouNetAmtFE = Val.Val(txtGrossAmountFE.Text) + DouDiscAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE;
            //txtNetAmountFE.Text = Val.Format(DouNetAmtFE, "########0.00");
            CalculationNew();

            BtnSave.Focus();
        }

        private void txtShippingAmount_Validated(object sender, EventArgs e)
        {
            if (Val.Val(txtGrossAmount.Text) != 0)
            {
                txtShippingPer.Text = Math.Round((Val.Val(txtShippingAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
            }
            else
            {
                txtShippingPer.Text = "0.00";
            }
            //Calculation();
            CalculationNew();
        }

        private void txtInsuranceAmount_Validated(object sender, EventArgs e)
        {
            if (Val.Val(txtGrossAmount.Text) != 0)
            {
                txtInsurancePer.Text = Math.Round((Val.Val(txtInsuranceAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
            }
            else
            {
                txtInsurancePer.Text = "0.00";
            }
            //Calculation();
            CalculationNew();
        }

        private void txtDiscAmount_Validated(object sender, EventArgs e)
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

            //Calculation();
            CalculationNew();
            //double DouDiscAmt = Val.Val(txtDiscAmount.Text);
            //double DouInsAmt = Val.Val(txtInsuranceAmount.Text);
            //double DouShipAmt = Val.Val(txtShippingAmount.Text);
            //double DouGSTAmt = Val.Val(txtGSTAmount.Text);

            //double DouNetAmt = Val.Val(txtGrossAmount.Text) + DouDiscAmt + DouInsAmt + DouShipAmt + DouGSTAmt;
            //txtNetAmount.Text = Val.Format(DouNetAmt, "########0.00");

            //double DouDiscAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouDiscAmt, 2);
            //double DouInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouInsAmt, 2);
            //double DouShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouShipAmt, 2);
            //double DouGSTAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouGSTAmt, 2);

            //txtDiscAmountFE.Text = Val.Format(DouDiscAmtFE, "########0.00");
            //txtInsuranceAmountFE.Text = Val.Format(DouInsAmtFE, "########0.00");
            //txtShippingAmountFE.Text = Val.Format(DouShipAmtFE, "########0.00");
            //txtGSTAmountFE.Text = Val.Format(DouGSTAmtFE, "########0.00");
            //double DouNetAmtFE = Val.Val(txtGrossAmountFE.Text) + DouDiscAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE;
            //txtNetAmountFE.Text = Val.Format(DouNetAmtFE, "########0.00"); 

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
                case "LABREPORTNO":  //Add : #P : 30-01-2020

                    if (mFormType == FORMTYPE.PURCHASEISSUE && Val.ToString(lblMode.Text).ToUpper() == "ADD MODE" && !Val.ToString(DRow["LABREPORTNO"]).Trim().Equals(string.Empty) && !Val.ToString(DRow["LABNAME"]).Trim().Equals(string.Empty) && Val.Val(DRow["CARAT"]) > 0)
                    {
                        this.Cursor = Cursors.WaitCursor;

                        BindGIAParameterInRow(Val.ToString(DRow["LABREPORTNO"]), Val.ToString(DRow["LABNAME"]), Val.ToString(DRow["CARAT"]), DRow);
                        FindRap(DRow, e.RowHandle);
                        this.Cursor = Cursors.Default;
                    }
                    break;

                case "LABNAME":
                    if (mFormType == FORMTYPE.PURCHASEISSUE && Val.ToString(lblMode.Text).ToUpper() == "ADD MODE" && !Val.ToString(DRow["LABREPORTNO"]).Trim().Equals(string.Empty) && !Val.ToString(DRow["LABNAME"]).Trim().Equals(string.Empty) && Val.Val(DRow["CARAT"]) > 0)
                    {
                        this.Cursor = Cursors.WaitCursor;

                        BindGIAParameterInRow(Val.ToString(DRow["LABREPORTNO"]), Val.ToString(DRow["LABNAME"]), Val.ToString(DRow["CARAT"]), DRow);
                        FindRap(DRow, e.RowHandle);
                        this.Cursor = Cursors.Default;
                    }
                    break;

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

                    //Add : #P : 30-01-2020
                    if (mFormType == FORMTYPE.PURCHASEISSUE && Val.ToString(lblMode.Text).ToUpper() == "ADD MODE" && !Val.ToString(DRow["LABREPORTNO"]).Trim().Equals(string.Empty) && !Val.ToString(DRow["LABNAME"]).Trim().Equals(string.Empty) && Val.Val(DRow["CARAT"]) > 0)
                    {
                        this.Cursor = Cursors.WaitCursor;

                        BindGIAParameterInRow(Val.ToString(DRow["LABREPORTNO"]), Val.ToString(DRow["LABNAME"]), Val.ToString(DRow["CARAT"]), DRow);
                        FindRap(DRow, e.RowHandle);
                        this.Cursor = Cursors.Default;
                    }
                    //End : #P : 30-01-2020

                    FindRap(DRow, e.RowHandle);
                    break;

                case "SHAPENAME":
                case "COLORNAME":
                case "CLARITYNAME":
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
                        DouPricePerCarat = Math.Round(DouRapaport + ((DouRapaport * DouDiscount) / 100), 2);
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
                        DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                    else
                        DouDiscount = 0;

                    DTabMemoDetail.Rows[e.RowHandle]["SALEDISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["SALEAMOUNT"] = DouAmount;
                    DTabMemoDetail.AcceptChanges();
                    GrdDetail.PostEditor();
                    break;

                case "MEMORAPAPORT":
                case "MEMODISCOUNT":
                    DouCarat = Val.Val(DRow["CARAT"]);
                    DouRapaport = Val.Val(DRow["SALERAPAPORT"]);
                    DouDiscount = Val.Val(DRow["MEMODISCOUNT"]);

                    if (DouRapaport != 0)
                        DouPricePerCarat = Math.Round(DouRapaport + ((DouRapaport * DouDiscount) / 100), 2);
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
                        DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                    else
                        DouDiscount = 0;

                    DTabMemoDetail.Rows[e.RowHandle]["MEMODISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["MEMOAMOUNT"] = DouAmount;
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(txtExcRate.Text), 2);
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 2);
                    DTabMemoDetail.AcceptChanges();
                    GrdDetail.PostEditor();
                    break;
                default:
                    break;
            }
            //Calculation();
            CalculationNew();
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
                    DRow["MEMODISCOUNT"] = Math.Round(((Val.Val(DRow["MEMOPRICEPERCARAT"]) - Val.Val(DRow["SALERAPAPORT"])) / Val.Val(DRow["SALERAPAPORT"])) * 100, 2);
                else
                    DRow["MEMODISCOUNT"] = 0;

                DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 2);
                DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 2);

            }
            DTabMemoDetail.AcceptChanges();
            //Calculation();
            CalculationNew();

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
                        DRow["SALEDISCOUNT"] = Math.Round(((Val.Val(DRow["SALEPRICEPERCARAT"]) - Val.Val(DRow["SALERAPAPORT"])) / Val.Val(DRow["SALERAPAPORT"])) * 100, 2);
                    else
                        DRow["SALEDISCOUNT"] = 0;

                    DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 2);
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
                        DRow["MEMODISCOUNT"] = Math.Round(((Val.Val(DRow["MEMOPRICEPERCARAT"]) - Val.Val(DRow["SALERAPAPORT"])) / Val.Val(DRow["SALERAPAPORT"])) * 100, 2);
                    else
                        DRow["MEMODISCOUNT"] = 0;


                    DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 2);
                    DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 2);

                }
            }



            DTabMemoDetail.AcceptChanges();
            //Calculation();
            CalculationNew();

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
                    DRow["SALEPRICEPERCARAT"] = Math.Round(Val.Val(DRow["SALERAPAPORT"]) + ((Val.Val(DRow["SALERAPAPORT"]) * Val.Val(DRow["SALEDISCOUNT"])) / 100), 2);
                    DRow["SALEAMOUNT"] = Math.Round(Val.Val(DRow["SALEPRICEPERCARAT"]) * Val.Val(DRow["CARAT"]), 2);

                    //DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["SALEAMOUNT"]) * Val.Val(txtExcRate.Text), 2);
                    //DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["SALEPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 2);

                    DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 2);
                    DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 2); DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 2);
                }
            }
            else
            {
                foreach (DataRow DRow in DTabMemoDetail.Rows)
                {
                    DRow["MEMODISCOUNT"] = Math.Round(Val.Val(DRow["SALEDISCOUNT"]) + Val.Val(DRow["PER"]), 2);
                    DRow["MEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["SALERAPAPORT"]) + ((Val.Val(DRow["SALERAPAPORT"]) * Val.Val(DRow["MEMODISCOUNT"])) / 100), 2);
                    DRow["MEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(DRow["CARAT"]), 2);

                    DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 2);
                    DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 2);
                }
            }
            DTabMemoDetail.AcceptChanges();
            //Calculation();
            CalculationNew();
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
                    Global.Message("Please Select Records That You Want To Return..");
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                var SelectedPartyStoneNo = DtReturnStoneList.AsEnumerable().Select(s => s.Field<string>("PARTYSTOCKNO")).ToArray();
                string StrPartyStoneNoList = string.Join(",", SelectedPartyStoneNo);

                LiveStockProperty LStockProperty = new LiveStockProperty();
                LStockProperty.STOCKNO = Val.ToString(StrPartyStoneNoList);
                LStockProperty.STOCKTYPE = Val.ToString(mStrStockType);
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

        private void txtDiscAmountFE_Validated(object sender, EventArgs e)  //Add : Pinali : 09-09-2019
        {
            try
            {
                if (Val.Val(txtGrossAmountFE.Text) != 0)
                {

                    double DouDiscountAmount = 0;

                    DouDiscountAmount = Math.Round(Val.Val(txtDiscAmountFE.Text) / Val.Val(txtExcRate.Text), 2);
                    txtDiscAmount.Text = Val.Format(DouDiscountAmount, "########0.00");
                    txtDiscPer.Text = Math.Round((Val.Val(DouDiscountAmount) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
                    //txtDiscPer.Text = Math.Round((Val.Val(txtDiscAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
                }
                else
                {
                    txtDiscPer.Text = "0.00";
                }

                //Calculation();
                CalculationNew();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtGSTAmountFE_Validated(object sender, EventArgs e)
        {
            try
            {
                if (Val.Val(txtGrossAmountFE.Text) != 0)
                {
                    double DouGSTAmount = 0;
                    DouGSTAmount = Math.Round(Val.Val(txtGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 2);

                    txtGSTAmount.Text = Val.Format(DouGSTAmount, "########0.00");
                    txtGSTPer.Text = Math.Round((Val.Val(DouGSTAmount) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
                }
                else
                {
                    txtGSTPer.Text = "0.00";
                    txtGSTAmount.Text = "0.00";
                    txtGSTAmountFE.Text = "0.00";
                }

                //double DouDiscAmt = Val.Val(txtDiscAmount.Text);
                //double DouInsAmt = Val.Val(txtInsuranceAmount.Text);
                //double DouShipAmt = Val.Val(txtShippingAmount.Text);
                //double DouGSTAmt = Val.Val(txtGSTAmount.Text);

                //double DouNetAmt = Val.Val(txtGrossAmount.Text) + DouDiscAmt + DouInsAmt + DouShipAmt + DouGSTAmt;
                //txtNetAmount.Text = Val.Format(DouNetAmt, "########0.00");

                //double DouDiscAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouDiscAmt, 2);
                //double DouInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouInsAmt, 2);
                //double DouShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouShipAmt, 2);
                //double DouGSTAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouGSTAmt, 2);

                //txtDiscAmountFE.Text = Val.Format(DouDiscAmtFE, "########0.00");
                //txtInsuranceAmountFE.Text = Val.Format(DouInsAmtFE, "########0.00");
                //txtShippingAmountFE.Text = Val.Format(DouShipAmtFE, "########0.00");
                //txtGSTAmountFE.Text = Val.Format(DouGSTAmtFE, "########0.00");
                //double DouNetAmtFE = Val.Val(txtGrossAmountFE.Text) + DouDiscAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE;
                //txtNetAmountFE.Text = Val.Format(DouNetAmtFE, "########0.00");
                CalculationNew();
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
                    if (mFormType == FORMTYPE.PURCHASEISSUE)
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

        private void txtGSTPer_Validated(object sender, EventArgs e)
        {
            //#P : 25-12-2019
            if (Val.Val(txtGrossAmount.Text) != 0)
            {
                txtGSTAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtGSTPer.Text)) / 100, 2).ToString();
                txtGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtGSTAmount.Text) / Val.Val(txtExcRate.Text), 2), "########0.00");
            }
            else
            {
                txtGSTPer.Text = "0.00";
                txtGSTAmount.Text = "0.00";
                txtGSTAmountFE.Text = "0.00";
            }
            //End : #P : 25-12-2019

            //Calculation();
            CalculationNew();
        }

        public bool CheckParameterValue(string ColumnValue, string StrParaType)
        {
            if (ColumnValue.Length != 0)
            {
                if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = '" + StrParaType + "'").CopyToDataTable(), "REMARK", Val.ToString(ColumnValue).ToUpper(), "PARA_ID", true)) == 0)
                {
                    return true;
                }
            }
            return false;

        }

        public void BindGIAParameterInRow(string StrLabReportNo, string StrLab, string StrCarat, DataRow DrRow)
        {
            if (StrLabReportNo.Trim().Equals(string.Empty) || StrLab.Trim().Equals(string.Empty))
                return;


            DataTable DT = new CertiStoneDetailDownload().GetCertData(StrLab, StrCarat, StrLabReportNo);


            string StrGIAShape = "Shape", StrGIAColor = "Color", StrGIAClarity = "Clarity", StrGIACut = "Cut", StrGIAPol = "Polish", StrGIASym = "Symm", StrGIAFL = "Flr",
                StrGIATablePer = "TableDiameterPer", StrGIADepthPer = "TotalDepthPer", StrGIACrAngle = "CrownAngle", StrGIACrHeight = "CrownHeight", StrGIAPavAngle = "PavillionAngle",
                StrGIAPavHeight = "PavillionHeight", StrGIAGirdlePer = "GirdlePer", StrGIAGirdleDesc = "GirdleName", StrGIAKeyToSym = "KeyToSymbols",
                StrGIALength = "DiameterMin", StrGIAWidth = "DiameterMax", StrGIAHeight = "TotalDepth";

            if (DT.Rows.Count <= 0 || Val.ToString(DT.Rows[0]["Shape"]).Trim().Equals(string.Empty))
            {
                this.Cursor = Cursors.Default;
                return;
            }



            // Check : All Parameter Combination From GIA Is Exists in System
            if (CheckParameterValue(Val.ToString(DT.Rows[0][StrGIAShape]), "SHAPE"))
            {
                Global.Message("Combination Of Shape : [" + Val.ToString(DT.Rows[0][StrGIAShape]) + "] Is Not Exists Please Add This In Parameter Master Form First.");
                return;
            }

            //Bind GIA Paramter in Row
            var drShape = (from DrPara in DtabPara.AsEnumerable()
                           where Val.ToString(DrPara["REMARK"]).ToUpper().Split(',').Contains(Val.ToString(DT.Rows[0][StrGIAShape]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "SHAPE"
                           select DrPara);
            DrRow["SHAPE_ID"] = drShape.Count() > 0 ? Val.ToInt32(drShape.FirstOrDefault()["PARA_ID"]) : 0;
            DrRow["SHAPENAME"] = drShape.Count() > 0 ? Val.ToString(drShape.FirstOrDefault()["PARANAME"]) : "";


            var drCol = (from DrPara in DtabPara.AsEnumerable()
                         where Val.ToString(DrPara["REMARK"]).ToUpper().Split(',').Contains(Val.ToString(DT.Rows[0][StrGIAColor]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "COLOR"
                         && Val.ToString(DrPara["PARAGROUP"]).ToUpper() == mStrStockType
                         select DrPara);
            DrRow["COLOR_ID"] = drCol.Count() > 0 ? Val.ToInt32(drCol.FirstOrDefault()["PARA_ID"]) : 0;
            DrRow["COLORNAME"] = drCol.Count() > 0 ? Val.ToString(drCol.FirstOrDefault()["PARANAME"]) : "";


            var drClarity = (from DrPara in DtabPara.AsEnumerable()
                             where Val.ToString(DrPara["REMARK"]).ToUpper().Split(',').Contains(Val.ToString(DT.Rows[0][StrGIAClarity]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "CLARITY"
                             && Val.ToString(DrPara["PARAGROUP"]).ToUpper() == mStrStockType
                             select DrPara);
            DrRow["CLARITY_ID"] = drClarity.Count() > 0 ? Val.ToInt32(drClarity.FirstOrDefault()["PARA_ID"]) : 0;
            DrRow["CLARITYNAME"] = drClarity.Count() > 0 ? Val.ToString(drClarity.FirstOrDefault()["PARANAME"]) : "";

            var drCut = (from DrPara in DtabPara.AsEnumerable()
                         where Val.ToString(DrPara["REMARK"]).ToUpper().Split(',').Contains(Val.ToString(DT.Rows[0][StrGIACut]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "CUT"
                         select DrPara);
            DrRow["CUT_ID"] = drCut.Count() > 0 ? Val.ToInt32(drCut.FirstOrDefault()["PARA_ID"]) : 0;
            DrRow["CUTNAME"] = drCut.Count() > 0 ? Val.ToString(drCut.FirstOrDefault()["PARANAME"]) : "";


            var drPol = (from DrPara in DtabPara.AsEnumerable()
                         where Val.ToString(DrPara["REMARK"]).ToUpper().Split(',').Contains(Val.ToString(DT.Rows[0][StrGIAPol]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "POLISH"
                         select DrPara);
            DrRow["POL_ID"] = drPol.Count() > 0 ? Val.ToInt32(drPol.FirstOrDefault()["PARA_ID"]) : 0;
            DrRow["POLNAME"] = drPol.Count() > 0 ? Val.ToString(drPol.FirstOrDefault()["PARANAME"]) : "";

            var drSym = (from DrPara in DtabPara.AsEnumerable()
                         where Val.ToString(DrPara["REMARK"]).ToUpper().Split(',').Contains(Val.ToString(DT.Rows[0][StrGIASym]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "SYMMETRY"
                         select DrPara);
            DrRow["SYM_ID"] = drSym.Count() > 0 ? Val.ToInt32(drSym.FirstOrDefault()["PARA_ID"]) : 0;
            DrRow["SYMNAME"] = drSym.Count() > 0 ? Val.ToString(drSym.FirstOrDefault()["PARANAME"]) : "";

            var drFL = (from DrPara in DtabPara.AsEnumerable()
                        where Val.ToString(DrPara["REMARK"]).ToUpper().Split(',').Contains(Val.ToString(DT.Rows[0][StrGIAFL]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "FLUORESCENCE"
                        select DrPara);
            DrRow["FL_ID"] = drFL.Count() > 0 ? Val.ToInt32(drFL.FirstOrDefault()["PARA_ID"]) : 0;
            DrRow["FLNAME"] = drFL.Count() > 0 ? Val.ToString(drFL.FirstOrDefault()["PARANAME"]) : "";


            DrRow["LENGTH"] = Val.Val(DT.Rows[0][StrGIALength]);
            DrRow["WIDTH"] = Val.Val(DT.Rows[0][StrGIAWidth]);
            DrRow["HEIGHT"] = Val.Val(DT.Rows[0][StrGIAHeight]);

            DrRow["TABLEPER"] = Val.Val(DT.Rows[0][StrGIATablePer]);
            DrRow["DEPTHPER"] = Val.Val(DT.Rows[0][StrGIADepthPer]);

            DrRow["CRANGLE"] = Val.Val(DT.Rows[0][StrGIACrAngle]);
            DrRow["CRHEIGHT"] = Val.Val(DT.Rows[0][StrGIACrHeight]);

            DrRow["PAVANGLE"] = Val.Val(DT.Rows[0][StrGIAPavAngle]);
            DrRow["PAVHEIGHT"] = Val.Val(DT.Rows[0][StrGIAPavHeight]);

            DrRow["GIRDLEPER"] = Val.Val(DT.Rows[0][StrGIAGirdlePer]);
            DrRow["GIRDLEDESC"] = Val.ToString(DT.Rows[0][StrGIAGirdleDesc]);

            DrRow["KEYTOSYMBOL"] = Val.ToString(DT.Rows[0][StrGIAKeyToSym]);
        }


    }
}

