using BarcodeLib.Barcode;
using BusLib;
using BusLib.Account;
using BusLib.Configuration;
//Busing BusLib.Rapaport;
using BusLib.EInvoice;
using BusLib.Master;
using BusLib.Rapaport;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.Spreadsheet;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting.Export.Pdf;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using QRCoder;
using MahantExport.Class;
using MahantExport.Utility;
using Spire.Xls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows.Forms;
using TaxProEInvoice.API;
using DevExpress.ClipboardSource.SpreadsheetML;

namespace MahantExport.Stock
{
    public partial class FrmMemoEntryNew : DevExpress.XtraEditors.XtraForm
    {

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();
        BOFindRap ObjRap = new BOFindRap();

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

        string MenuTagName = "";

        public DataTable mDTab;

        public DataTable DTabMemoStock;
        public DataRow DRow { get; set; }

        string mStrStockType = "";
        string PstrOrder_MemoId = "";
        bool Autosave = false;

        string strMemo_ID = "";
        public FrmMemoEntryNew()
        {
            InitializeComponent();
        }

        public FORMTYPE mFormType = FORMTYPE.SALEINVOICE;
        public enum FORMTYPE
        {
            LABISSUE = 1,
            LABRETURN = 2,
            MEMOISSUE = 3,
            MEMORETURN = 4,
            CONSIGNMENTISSUE = 5,
            CONSIGNMENTRETURN = 6,
            ASSORTERISSUE = 7,
            ASSORTERRETURN = 8,
            HOLD = 9,
            RELEASE = 10,
            PURCHASEISSUE = 11,
            PURCHASERETURN = 12,
            SALEINVOICE = 13,
            SALESDELIVERYRETURN = 14,
            ORDERCONFIRM = 15,
            ORDERCONFIRMRETURN = 16,
            ONLINE = 17,
            OFFLINE = 18,
            UNGRADEDTOMIX = 19,
            GRADEDTOMIX = 20

        }

        public void ShowForm(FORMTYPE pFormType, DataTable pDtInvoice, string pStockType = "ALL", string StrMainMemo_ID = "")
        {
            strMemo_ID = string.Empty;
            if (pDtInvoice.Rows.Count != 0)
            {
                var list = pDtInvoice.AsEnumerable().Select(r => r["MEMO_ID"].ToString());
                strMemo_ID = string.Join(",", list);
            }

            DataSet DS = new DataSet();
            mFormType = pFormType;
            mStrStockType = "ALL";

            DataRow DR = ObjMemo.GetCurrency();
            if (DR == null)
            {
                txtCurrency.Text = "";
                txtCurrency.Tag = "";
            }
            else
            {
                txtCurrency.Text = Val.ToString(DR["CurrencyName"]);
                txtCurrency.Tag = Val.ToString(DR["Currency_ID"]);
            }

            DataRow DER = ObjMemo.GetExcRate();
            if (DER == null)
            {
                txtExcRate.Text = "";
                txtExcRate.Tag = "";
            }
            else
            {
                txtExcRate.Text = Val.ToString(DER["ExcRate"]);
            }
            StrMainMemo_ID = Val.ToString(StrMainMemo_ID).Trim().Equals(string.Empty) ? Val.ToString(Guid.Empty) : StrMainMemo_ID;

            DS = ObjMemo.GetMemoListData(-1, null, null, "", "", "", 0, "", 0, "", "", "", mStrStockType, false, -1);

            DTabMemo = DS.Tables[0];
            DTabMemoDetail = DS.Tables[1];
            DTabMemoDetailParcelFile = DS.Tables[2];

            lblMode.Text = "Add Mode";
            if (pDtInvoice == null && mFormType == FORMTYPE.PURCHASEISSUE)
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
            }

            pDtInvoice.Columns.Add(new DataColumn("RapAmt", typeof(double)));
            for (int i = 0; i < pDtInvoice.Rows.Count; i++)
            {
                double carat = Val.Val(pDtInvoice.Rows[i]["CARAT"]);
                if (mFormType == FORMTYPE.LABISSUE)
                {
                    double Rapaport = Val.Val(pDtInvoice.Rows[i]["COSTRAPAPORT"]);
                    pDtInvoice.Rows[i]["RapAmt"] = Val.Val(carat) * Val.Val(Rapaport);
                }

                else
                {
                    double Rapaport = Val.Val(pDtInvoice.Rows[i]["SALERAPAPORT"]);
                    pDtInvoice.Rows[i]["RapAmt"] = Val.Val(carat) * Val.Val(Rapaport);
                }

            }
            double TotalPcs = Val.Val(pDtInvoice.Compute("SUM(PCS)", string.Empty));
            double TotalCarat = Val.Val(pDtInvoice.Compute("SUM(CARAT)", string.Empty));
            double TotalAmt = 0;
            if (mFormType == FORMTYPE.LABISSUE)
            {
                TotalAmt = Val.Val(pDtInvoice.Compute("SUM(COSTAMOUNT)", string.Empty));
            }
            else
            {
                TotalAmt = Val.Val(pDtInvoice.Compute("SUM(SALEAMOUNT)", string.Empty));
            }

            double TotalPricePerCts = Val.Val(Val.Val(TotalAmt) / Val.Val(TotalCarat));
            double TotalAmtINR = Val.Val(Val.Val(TotalAmt) * Val.Val(txtExcRate.Text));

            double TotalRapaport = Val.Val(pDtInvoice.Compute("SUM(RapAmt)", string.Empty));
            double TotalAvgDisc = 0;
            if (Val.Val(TotalRapaport) == 0)
            {
                TotalAvgDisc = 0;
            }
            else
            {
                TotalAvgDisc = Val.Val(((Val.Val(TotalAmt) - Val.Val(TotalRapaport)) / Val.Val(TotalRapaport)) * 100);
            }


            txtTotalPcs.Text = Val.ToString(TotalPcs);
            txtTotalCarat.Text = Val.ToString(TotalCarat);
            txtPricePerCarat.Text = Val.ToString(TotalPricePerCts);
            txtNetAmount.Text = Val.ToString(TotalAmt);
            txtNetAmountFE.Text = Val.ToString(TotalAmtINR);

            if (mFormType == FORMTYPE.PURCHASERETURN || mFormType == FORMTYPE.SALESDELIVERYRETURN || mFormType == FORMTYPE.ORDERCONFIRMRETURN || mFormType == FORMTYPE.MEMORETURN
                  || mFormType == FORMTYPE.LABISSUE || mFormType == FORMTYPE.LABRETURN || mFormType == FORMTYPE.RELEASE)
            {
                txtBillingParty.Text = Val.ToString(pDtInvoice.Rows[0]["BILLPARTYNAME"]);
                txtBillingParty.Tag = Val.ToString(pDtInvoice.Rows[0]["BILLPARTY_ID"]);
            }


            txtAvgDisc.Text = Val.ToString(TotalAvgDisc);

            foreach (DataRow DRow in pDtInvoice.Rows)
            {

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
                        DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["SALEAMOUNT"]), 3); //#P : 27-01-2022
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

                    if (BOConfiguration.gStrLoginSection == "B" && (mFormType == FORMTYPE.PURCHASEISSUE || mFormType == FORMTYPE.SALEINVOICE))// Add by khushbu 07-02-22. b part ma purchase entry ma janged amount change nahti karvani
                    {
                        DRNew["JANGEDRAPAPORT"] = DRow["JANGEDRAPAPORT"];
                        DRNew["JANGEDDISCOUNT"] = DRow["JANGEDDISCOUNT"];
                        DRNew["JANGEDPRICEPERCARAT"] = DRow["JANGEDPRICEPERCARAT"];
                        DRNew["JANGEDAMOUNT"] = DRow["JANGEDAMOUNT"];
                    }
                    else
                    {
                        DRNew["JANGEDRAPAPORT"] = DRow["MEMORAPAPORT"];
                        DRNew["JANGEDDISCOUNT"] = DRow["MEMODISCOUNT"];
                        DRNew["JANGEDPRICEPERCARAT"] = DRow["MEMOPRICEPERCARAT"];
                        DRNew["JANGEDAMOUNT"] = DRow["MEMOAMOUNT"];
                    }


                }

                DRNew["EXPINVOICERATE"] = DRow["EXPINVOICERATE"];
                DRNew["EXPINVOICERATEFE"] = DRow["EXPINVOICERATEFE"];
                DRNew["EXPINVOICEAMT"] = DRow["EXPINVOICEAMT"];
                DRNew["EXPINVOICEAMTFE"] = DRow["EXPINVOICEAMTFE"];

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


                DTabMemoDetail.Rows.Add(DRNew);
            }

            CreateSummaryTable();
            DTPMemoDate.Focus();
            FillControlName();

            Val.FormGeneralSettingForPopup(this);
            AttachFormDefaultEvent();
            if (mFormType == FORMTYPE.PURCHASERETURN || mFormType == FORMTYPE.SALESDELIVERYRETURN || mFormType == FORMTYPE.ORDERCONFIRMRETURN || mFormType == FORMTYPE.MEMORETURN
                  || mFormType == FORMTYPE.LABRETURN || mFormType == FORMTYPE.RELEASE)
            {
                BtnSave_Click(null, null);
                this.Close();
            }
            else
            {
                this.Show();
            }


        }


        public void ShowForm(string pStrMemoID, string pStrStockType = "ALL") //Call When Double Click On Current Stock Color Clarity Wise Report Data
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Val.FormGeneralSetting(this);
                AttachFormDefaultEvent();

                this.Show();

                BtnSave.Enabled = true;
                BtnClear_Click(null, null);

                mStrStockType = pStrStockType;
                DataSet DS = ObjMemo.GetMemoListData(0, null, null, "", "", "", 0, "", 0, "", "ALL", pStrMemoID, mStrStockType, false, -1);

                DataRow DRow = DTabMemo.Rows[0];
                //lblMode.Text = "Edit Mode";
                //txtJangedNo.Text = Val.ToString(DRow["JANGEDNOSTR"]);
                //txtJangedNo.Tag = Val.ToString(DRow["MEMO_ID"]);
                //lblMemoNo.Tag = Val.ToString(DRow["MEMO_ID"]);
                //lblMemoNo.Text = Val.ToString(DRow["MEMONO"]);
                //lblStatus.Text = Val.ToString(DRow["STATUS"]);

                DTPMemoDate.Text = Val.ToString(DRow["MEMODATE"]);
                DTPMemoDate.Text = Val.ToDate(DateTime.Parse(DRow["MEMODATE"].ToString()), AxonDataLib.BOConversion.DateFormat.DDMMYYYY);

                txtBillingParty.Tag = Val.ToString(DRow["BILLINGPARTY_ID"]);
                txtBillingParty.Text = Val.ToString(DRow["BILLINGPARTYNAME"]);

                txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);
                txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                txtExcRate.Text = Val.ToString(DRow["EXCRATE"]);

                lblBillingAdd1.Text = Val.ToString(DRow["BILLINGADDRESS1"]);
                lblBillingAdd2.Text = Val.ToString(DRow["BILLINGADDRESS2"]);
                lblBillingAdd3.Text = Val.ToString(DRow["BILLINGADDRESS3"]);
                lblBillingCountry.Tag = Val.ToString(DRow["BILLINGCOUNTRY_ID"]);
                lblBillingCountry.Text = Val.ToString(DRow["BILLINGCOUNTRYNAME"]);
                lblBillingState.Text = Val.ToString(DRow["BILLINGSTATE"]);
                lblBillingCity.Text = Val.ToString(DRow["BILLINGCITY"]);
                lblBillingZipCode.Text = Val.ToString(DRow["BILLINGZIPCODE"]);
                txtRemark.Text = Val.ToString(DRow["REMARK"]);

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




        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPartyName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTY_ID,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    if(mFormType == FORMTYPE.ASSORTERISSUE || mFormType == FORMTYPE.ASSORTERRETURN)
                    {
                        FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPPARTY);
                    }
                    else
                    {
                        FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
                    }
                    
                    FrmSearch.mStrColumnsToHide = "SELLER_ID";
                    FrmSearch.mBoolISPostBack = true;
                    FrmSearch.mStrISPostBackColumn = "PARTYNAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBillingParty.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBillingParty.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                        lblBillingAdd1.Text = Val.ToString(FrmSearch.DRow["BILLINGADDRESS1"]);
                        lblBillingAdd2.Text = Val.ToString(FrmSearch.DRow["BILLINGADDRESS2"]);
                        lblBillingAdd3.Text = Val.ToString(FrmSearch.DRow["BILLINGADDRESS3"]);
                        lblBillingCountry.Text = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRYNAME"]);
                        lblBillingCountry.Tag = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRY_ID"]);
                        lblBillingState.Text = Val.ToString(FrmSearch.DRow["BILLINGSTATE"]);
                        lblBillingCity.Text = Val.ToString(FrmSearch.DRow["BILLINGCITY"]);
                        lblBillingZipCode.Text = Val.ToString(FrmSearch.DRow["BILLINGZIPCODE"]);
                        lblGSTNo.Text = Val.ToString(FrmSearch.DRow["GSTNO"]);
                        lblCompGSTNo.Text = Val.ToString(FrmSearch.DRow["COMPGSTNO"]);
                        LblCompanyName.Text = Val.ToString(FrmSearch.DRow["COMPANYNAME"]);

                    }

                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }
        }

        public void FillControlName()
        {

            DtabProcess = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PROCESSALL);
            DtabPara = new BOMST_Parameter().GetParameterData();

            if (mFormType == FORMTYPE.HOLD)
            {
                GetSelectedProcessIssue("HOLD");
            }
            else if (mFormType == FORMTYPE.RELEASE)
            {
                GetSelectedProcessIssue("RELEASE");
            }

            else if (mFormType == FORMTYPE.OFFLINE)
            {
                GetSelectedProcessIssue("OFFLINE");
            }

            else if (mFormType == FORMTYPE.ONLINE)
            {
                GetSelectedProcessIssue("ONLINE");
            }

            else if (mFormType == FORMTYPE.ORDERCONFIRM)
            {
                GetSelectedProcessIssue("ORDER CONFIRM");
            }
            else if (mFormType == FORMTYPE.ORDERCONFIRMRETURN)
            {
                GetSelectedProcessIssue("ORDER CONFIRM RETURN");
            }
            else if (mFormType == FORMTYPE.SALEINVOICE)
            {
                GetSelectedProcessIssue("SALES DELIVERY");
            }
            else if (mFormType == FORMTYPE.SALESDELIVERYRETURN)
            {
                GetSelectedProcessIssue("SALES DELIVERY RETURN");
            }

            else if (mFormType == FORMTYPE.MEMOISSUE)
            {
                GetSelectedProcessIssue("MEMO ISSUE");
            }

            else if (mFormType == FORMTYPE.MEMORETURN)
            {
                GetSelectedProcessIssue("MEMO RETURN");
            }

            else if (mFormType == FORMTYPE.LABISSUE)
            {
                GetSelectedProcessIssue("LAB ISSUE");
            }

            else if (mFormType == FORMTYPE.LABRETURN)
            {
                GetSelectedProcessIssue("LAB RETURN");
            }
            else if (mFormType == FORMTYPE.CONSIGNMENTISSUE)
            {
                GetSelectedProcessIssue("CONSIGNMENT ISSUE");
            }

            else if (mFormType == FORMTYPE.CONSIGNMENTRETURN)
            {
                GetSelectedProcessIssue("CONSIGNMENT RETURN");
            }

            else if (mFormType == FORMTYPE.PURCHASEISSUE)
            {
                GetSelectedProcessIssue("PURCHASE");
            }
            else if (mFormType == FORMTYPE.PURCHASERETURN)
            {
                GetSelectedProcessIssue("PURCHASE RETURN");
            }
            else if (mFormType == FORMTYPE.ASSORTERISSUE)
            {
                GetSelectedProcessIssue("ASSORTER ISSUE");
            }
            else if (mFormType == FORMTYPE.ASSORTERRETURN)
            {
                GetSelectedProcessIssue("ASSORTER RETURN");
            }
            else if (mFormType == FORMTYPE.UNGRADEDTOMIX)
            {
                GetSelectedProcessIssue("UNGRADED TO MIX");
            }
            else if (mFormType == FORMTYPE.GRADEDTOMIX)
            {
                GetSelectedProcessIssue("GRADED TO MIX");
            }

        }
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

        private void FrmMemoEntryNew_Load(object sender, EventArgs e)
        {
            //txtExcRate.Text = "CNBC" + GET_RATE("CNBC"); // Added By Keyur For Web Browser Exc Rate
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

            return false;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            DTPMemoDate.Text = string.Empty;
            txtBillingParty.Text = string.Empty;
            txtRemark.Text = string.Empty;
            txtCurrency.Text = string.Empty;
            txtExcRate.Text = string.Empty;
            txtTotalPcs.Text = string.Empty;
            txtTotalCarat.Text = string.Empty;
            txtPricePerCarat.Text = string.Empty;
            txtAvgDisc.Text = string.Empty;
            txtNetAmount.Text = string.Empty;
            txtNetAmountFE.Text = string.Empty;
            lblBillingAdd1.Text = string.Empty;
            lblBillingAdd2.Text = string.Empty;
            lblBillingAdd3.Text = string.Empty;
            lblBillingCity.Text = string.Empty;
            lblBillingCountry.Text = string.Empty;
            lblBillingState.Text = string.Empty;
            lblBillingZipCode.Text = string.Empty;
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
                   && mFormType != FORMTYPE.LABISSUE && mFormType != FORMTYPE.LABRETURN && mFormType != FORMTYPE.UNGRADEDTOMIX && mFormType != FORMTYPE.GRADEDTOMIX)
                {
                    Global.Message("Please Add Proper Stone Detail.");
                    return;
                }

                //if (txtBillingParty.Text.Length == 0 && mFormType == FORMTYPE.PURCHASERETURN && mFormType == FORMTYPE.SALESDELIVERYRETURN && mFormType == FORMTYPE.ORDERCONFIRMRETURN && mFormType == FORMTYPE.MEMORETURN
                //   && mFormType == FORMTYPE.LABISSUE && mFormType == FORMTYPE.LABRETURN)
                //{
                //    Global.Message("Billing Party Is Required");
                //    txtBillingParty.Focus();
                //    return;
                //}


                if (mFormType == FORMTYPE.PURCHASERETURN || mFormType == FORMTYPE.SALESDELIVERYRETURN || mFormType == FORMTYPE.ORDERCONFIRMRETURN || mFormType == FORMTYPE.MEMORETURN
                  || mFormType == FORMTYPE.LABISSUE || mFormType == FORMTYPE.LABRETURN || mFormType == FORMTYPE.RELEASE)
                {
                    Autosave = true;
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

                    Property.MEMO_ID = Val.ToString(lblMemoNo.Tag);


                    DataTable DTab = ObjMemo.ValDelete(Property);
                    if (DTab.Rows.Count != 0 && BOConfiguration.gStrLoginSection != "B")
                    {
                        //Global.Message("Some Stones Are In Other Process\n\n You Can Not Delete");
                        Global.Message("You can't Update Because Invoice entry is exceeded from account side...");
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
                    else if (DTab.Rows.Count == 0 && BOConfiguration.gStrLoginSection == "B")
                    {
                        Global.Message("You can't Update Because Invoice entry is exceeded from account side...");
                        DTab.Dispose();
                        DTab = null;

                        return;
                    }

                    DTab.Dispose();
                    DTab = null;
                }

                Property.MEMONO = Val.ToInt64(lblMemoNo.Text);
                Property.MEMOTYPE = "INTERNAL";
                Property.MEMODATE = Val.SqlDate(DTPMemoDate.Text);

                Property.BILLINGPARTY_ID = Val.ToString(txtBillingParty.Tag);
                if (Val.ToString(txtBillingParty.Tag) == "")
                {
                    Property.BILLINGPARTY_ID = Guid.Empty.ToString();
                }
                Property.BILLINGPARTNAME = Val.ToString(txtBillingParty.Text);

                Property.SHIPPINGPARTY_ID = null;

                //Property.BROKER_ID = Val.ToString(txtBrokername.Tag);
                Property.BROKERBASEPER = 0.00;
                Property.BROKERPROFITPER = 0.00;
                Property.BrokerAmountFE = 0.00;
                Property.BrokerAmount = 0.00;

                Property.ADAT_ID = null;
                Property.ADATPER = 0.00;
                Property.AdatAmtFE = 0.00;
                Property.AdatAmt = 0.00;

                Property.CURRENCY_ID = Val.ToInt32(txtCurrency.Tag);
                Property.EXCRATE = Val.Val(txtExcRate.Text);

                Property.MEMODISCOUNT = 0;

                Property.BILLINGADDRESS1 = Val.ToString(lblBillingAdd1.Text);
                Property.BILLINGADDRESS2 = Val.ToString(lblBillingAdd2.Text);
                Property.BILLINGADDRESS3 = Val.ToString(lblBillingAdd3.Text);
                Property.BILLINGCOUNTRY_ID = Val.ToInt32(lblBillingCountry.Tag);
                Property.BILLINGSTATE = Val.ToString(lblBillingState.Text);
                Property.BILLINGCITY = Val.ToString(lblBillingCity.Text);
                Property.BILLINGZIPCODE = Val.ToString(lblBillingZipCode.Text);

                Property.NETAMOUNT = Val.Val(txtNetAmount.Text);

                Property.FNETAMOUNT = Val.Val(txtNetAmountFE.Text);

                Property.REMARK = Val.ToString(txtRemark.Text);
                Property.SOURCE = "SOFTWARE";

                Property.PROCESS_ID = Val.ToInt32(lblTitle.Tag);
                Property.PROCESSNAME = Val.ToString(lblTitle.Text);

                Property.TOTALPCS = Val.ToInt32(txtTotalPcs.Text);
                Property.TOTALCARAT = Val.Val(txtTotalCarat.Text);
                Property.TOTALAVGRATE = Val.Val(txtPricePerCarat.Text);
                Property.NETAMOUNT = Val.Val(txtNetAmount.Text);
                Property.FNETAMOUNT = Val.Val(txtNetAmountFE.Text);
                Property.TOTALAVGDISC = Val.Val(txtAvgDisc.Text);
                if (Val.ToInt32(lblTitle.Tag) == 9)
                {
                    Property.ACCTYPE = "SALE";
                }


                if (PstrOrder_MemoId.Length == 0)
                {
                    Property.ORDERMEMO_ID = "00000000-0000-0000-0000-000000000000";
                }
                else
                {
                    Property.ORDERMEMO_ID = PstrOrder_MemoId;
                }
                Property.SELLER_ID = Val.ToString(BOConfiguration.gEmployeeProperty.LEDGER_ID);

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
                                {
                                    Property = ObjMemo.SaveSalesEntry(cn_T, Property, MemoEntryDetailForXML, lblMode.Text, "", MemoEntryDetailForXMLParcel);
                                }
                            }
                            else
                            {
                                Property = ObjMemo.SaveMemoEntry(cn_T, Property, MemoEntryDetailForXML, lblMode.Text, "", MemoEntryDetailForXMLParcel);
                            }
                            lblMemoNo.Text = Property.ReturnValue;
                        }
                        else // export value same as sales value
                        {
                            foreach (DataRow DR in DTabExpMemoDetail.Rows)
                            {
                                DR["FMEMOAMOUNT"] = Math.Round(Val.ToDecimal(DR["FMEMOAMOUNT"]) * 1000, 2);
                            }
                            DTabExpMemoDetail.AcceptChanges();

                            string MemoEntryDetailExpForXML = string.Empty;
                            using (StringWriter sw = new StringWriter())
                            {
                                DTabExpMemoDetail.WriteXml(sw);
                                MemoEntryDetailExpForXML = sw.ToString();
                                MemoEntryDetailExpForXML = MemoEntryDetailExpForXML.Replace("<DocumentElement>", "<NewDataSet>");
                                MemoEntryDetailExpForXML = MemoEntryDetailExpForXML.Replace("</DocumentElement>", "</NewDataSet>");
                            }

                            //AmtConversation_From1000();

                            Property.NETAMOUNT = Val.Val(txtNetAmount.Text);
                            Property.FNETAMOUNT = Math.Round(Val.Val(txtNetAmountFE.Text), 2);

                            Property.DISCOUNTPER = 0;
                            Property.DISCOUNTAMOUNT = 0;
                            Property.FDISCOUNTAMOUNT = 0;
                            Property.INSURANCEPER = 0;
                            Property.INSURANCEAMOUNT = 0;
                            Property.FINSURANCEAMOUNT = 0;
                            Property.SHIPPINGPER = 0;
                            Property.SHIPPINGAMOUNT = 0;
                            Property.FSHIPPINGAMOUNT = 0;
                            Property.GSTPER = 0;
                            Property.GSTAMOUNT = 0;
                            Property.FGSTAMOUNT = 0;

                            Property.IGSTPER = 0;
                            Property.IGSTAMOUNT = 0;
                            Property.FIGSTAMOUNT = 0;
                            Property.CGSTPER = 0;
                            Property.CGSTAMOUNT = 0;
                            Property.FCGSTAMOUNT = 0;
                            Property.SGSTPER = 0;
                            Property.SGSTAMOUNT = 0;
                            Property.FSGSTAMOUNT = 0;

                            //CalculationNew(false);

                            Property.NETAMOUNT = Val.Val(txtNetAmount.Text);
                            Property.FNETAMOUNT = Val.Val(txtNetAmountFE.Text);


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

                if (mFormType == FORMTYPE.PURCHASERETURN || mFormType == FORMTYPE.SALESDELIVERYRETURN || mFormType == FORMTYPE.ORDERCONFIRMRETURN || mFormType == FORMTYPE.MEMORETURN || mFormType == FORMTYPE.LABISSUE || mFormType == FORMTYPE.LABRETURN || mFormType == FORMTYPE.RELEASE)
                {

                }
                else
                {
                    Global.Message(ReturnMessageDesc);
                    //ADDED BY GUNJAN:21/05/2024
                    //BtnPrint_Click(null, null);
                    //END AS GUNJAN
                }

                if (Val.ToString(BOConfiguration.COMPANYNAME).ToUpper() == "TRP IMPEX")//TRP IMPEX
                {
                    if (mFormType == FORMTYPE.MEMOISSUE || mFormType == FORMTYPE.ASSORTERISSUE)
                    {
                        BtnPrint_Click(null,null);
                    }
                }
                if (Val.ToString(BOConfiguration.COMPANYNAME).ToUpper() == "RIJIYA GEMS")
                {
                    if (mFormType == FORMTYPE.MEMOISSUE )
                    {
                        BtnPrint_Click(null, null);
                    }
                }
                //Gunjan:20/12/2024
                if (mFormType == FORMTYPE.ASSORTERISSUE)
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

                        DataTable DTab = ObjMemo.GetAssortJangedPrintData(StrJangedNo);

                        if (DTab.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("There Is No Data Found For Print");
                            return;
                        }

                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowMemoInvoiceHKPrint("RPT_JangedAssortPrint", DTab);

                        this.Cursor = Cursors.Default;
                    }

                    catch (Exception ex)
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message(ex.Message);
                    }
                }
                //End As Gunjan
                DTabMemoStock = ObjMemo.StockGetDataForMemo(Val.ToString(lblMemoNo.Text));
                this.Close();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        public void ExportToExcel_GIA_SURAT(DataSet dataSet, string outputPath, string MEMONO, string DDATE, string rate, string amount_word)
        {
            // Create the Excel Application object
            Excel.Application excelApp = new Excel.Application();

            Excel.Workbook excelWorkbook = excelApp.Workbooks.Add(Type.Missing);

            //Microsoft.Office.Interop.Excel.Range xlRange;
            int sheetIndex = 0;

            // Copy each DataTable
            foreach (System.Data.DataTable dt in dataSet.Tables)
            {
                decimal RECORD_ONE_PAGGE = 37;
                decimal total_recored = dt.Rows.Count;
                decimal NUM_PAGE = Convert.ToDecimal(total_recored / RECORD_ONE_PAGGE);

                int PAGE = 0;
                string[] digits = NUM_PAGE.ToString().Split('.');
                PAGE = Convert.ToInt32(digits[0]);
                if (Convert.ToDecimal(digits[1]) > 0)
                {
                    PAGE = PAGE + 1;
                }

                int addrow = dt.Rows.Count + (PAGE * 14);


                int R_ROW = 9;
                int r_count = 0;


                // Copy the DataTable to an object array
                object[,] rawData = new object[addrow, dt.Columns.Count];

                // Copy the values to the object array
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    if (r_count == RECORD_ONE_PAGGE)
                    {
                        r_count = 0;

                        R_ROW = R_ROW + 14;


                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            rawData[R_ROW, col] = dt.Rows[row][col];
                        }
                        R_ROW += 1;
                    }
                    else
                    {
                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            rawData[R_ROW, col] = dt.Rows[row][col];
                        }
                        R_ROW += 1;
                    }
                    r_count = r_count + 1;
                }



                // Calculate the final column letter
                string finalColLetter = string.Empty;
                string colCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                int colCharsetLen = colCharset.Length;

                if (dt.Columns.Count > colCharsetLen)
                {
                    finalColLetter = colCharset.Substring((dt.Columns.Count - 1) / colCharsetLen - 1, 1);
                }

                finalColLetter += colCharset.Substring((dt.Columns.Count - 1) % colCharsetLen, 1);

                // Create a new Sheet
                Excel.Worksheet excelSheet = (Excel.Worksheet)excelWorkbook.Sheets.Add(excelWorkbook.Sheets.get_Item(++sheetIndex), Type.Missing, 1, Excel.XlSheetType.xlWorksheet);
                //excelSheet.Range["A1", "C1"].Interior.Color = System.Drawing.Color.Pink;
                //excelSheet.Range["D1", "G1"].Interior.Color = System.Drawing.Color.Yellow;
                excelSheet.Name = dt.TableName;
                //excelSheet.Name = "sheets";

                // Fast data export to Excel
                string excelRange = string.Format("A1:{0}{1}", finalColLetter, addrow);

                excelSheet.get_Range(excelRange, Type.Missing).Value2 = rawData;


                int F = 0;
                for (int i = 0; i < PAGE; i++)
                {

                    excelSheet.Columns[1].ColumnWidth = 5.14;
                    excelSheet.Columns[2].ColumnWidth = 8.57;
                    excelSheet.Columns[3].ColumnWidth = 6;
                    excelSheet.Columns[4].ColumnWidth = 6.71;
                    excelSheet.Columns[5].ColumnWidth = 6.71;
                    excelSheet.Columns[6].ColumnWidth = 9.71;
                    excelSheet.Columns[7].ColumnWidth = 6.29;
                    excelSheet.Columns[8].ColumnWidth = 6;
                    excelSheet.Columns[9].ColumnWidth = 7.14;
                    excelSheet.Columns[10].ColumnWidth = 9.14;
                    excelSheet.Columns[11].ColumnWidth = 8.57;
                    excelSheet.Columns[12].ColumnWidth = 12;


                    string A2 = "A" + (F + 2).ToString();
                    string G2 = "G" + (F + 2).ToString();
                    string A3 = "A" + (F + 3).ToString();
                    string G3 = "G" + (F + 3).ToString();
                    string A4 = "A" + (F + 4).ToString();
                    string G4 = "G" + (F + 4).ToString();
                    string A5 = "A" + (F + 5).ToString();
                    string G5 = "G" + (F + 5).ToString();

                    excelSheet.Range[A2, G2].Merge();
                    excelSheet.Range[A2, A2].Value = "TO,";
                    excelSheet.Range[A3, G3].Merge();
                    excelSheet.Range[A3, A3].Value = txtBillingParty.Text;
                    excelSheet.Range[A4, G4].Merge();
                    excelSheet.Range[A4, A4].Value = lblBillingAdd1.Text;
                    excelSheet.Range[A5, G5].Merge();
                    excelSheet.Range[A5, A5].Value = lblBillingAdd2.Text;

                    string H2 = "H" + (F + 2).ToString();
                    string L2 = "L" + (F + 2).ToString();
                    string H3 = "H" + (F + 3).ToString();
                    string L3 = "L" + (F + 3).ToString();
                    string H4 = "H" + (F + 4).ToString();
                    string L4 = "L" + (F + 4).ToString();
                    string H5 = "H" + (F + 5).ToString();
                    string L5 = "L" + (F + 5).ToString();

                    excelSheet.Range[H2, L2].Merge();
                    excelSheet.Range[H2, H2].Value = lblGSTNo.Text;
                    excelSheet.Range[H3, L3].Merge();
                    excelSheet.Range[H3, H3].Value = lblCompGSTNo.Text;
                    excelSheet.Range[H4, L4].Merge();
                    excelSheet.Range[H4, H4].Value = "MEMO NO. " + lblMemoNo.Text + " /DATE : " + DDATE;
                    excelSheet.Range[H5, L5].Merge();
                    excelSheet.Range[H5, H5].Value = "";

                    string A6 = "A" + (F + 6).ToString();
                    string l6 = "l" + (F + 6).ToString();

                    excelSheet.Range[A6, l6].Merge();
                    excelSheet.Range[A6, A6].Value = " Packing List Cut & Polish Diamond    ";
                    excelSheet.Range[A6, A6].HorizontalAlignment = -4108;
                    ((Excel.Range)excelSheet.Rows[(F + 6).ToString(), Type.Missing]).Font.Bold = true;

                    string A7 = "A" + (F + 7).ToString();
                    string l7 = "l" + (F + 7).ToString();

                    excelSheet.Range[A7, l7].Merge();
                    excelSheet.Range[A7, A7].Value = "PAGE-" + (i + 1);
                    excelSheet.Range[A7, A7].HorizontalAlignment = -4108;

                    string A8 = "A" + (F + 8).ToString();
                    string B8 = "B" + (F + 8).ToString();
                    string C8 = "C" + (F + 8).ToString();
                    string D8 = "D" + (F + 8).ToString();
                    string E8 = "E" + (F + 8).ToString();
                    string F8 = "F" + (F + 8).ToString();
                    string G8 = "G" + (F + 8).ToString();
                    string H8 = "H" + (F + 8).ToString();
                    string I8 = "I" + (F + 8).ToString();
                    string J8 = "J" + (F + 8).ToString();
                    string K8 = "K" + (F + 8).ToString();
                    string L8 = "L" + (F + 8).ToString();

                    excelSheet.Range[A8, A8].Value = "SR.NO";
                    excelSheet.Range[B8, B8].Value = "CLIENT ID";
                    excelSheet.Range[C8, C8].Value = "SHAPE";
                    excelSheet.Range[D8, D8].Value = "SERVICE";
                    excelSheet.Range[E8, E8].Value = "CARAT";
                    excelSheet.Range[F8, F8].Value = "DIAMETER";
                    excelSheet.Range[G8, G8].Value = "HEIGHT";
                    excelSheet.Range[H8, H8].Value = "COLOR";
                    excelSheet.Range[I8, I8].Value = "CLARITY";
                    excelSheet.Range[J8, J8].Value = "$/CT";
                    excelSheet.Range[K8, K8].Value = "TOTAL";
                    excelSheet.Range[L8, L8].Value = "CONTROL NO.";

                    string A10 = "A" + (F + 10).ToString();
                    string L10 = "L" + (F + 47).ToString();
                    excelSheet.Range[A10, L10].HorizontalAlignment = -4108;


                    string B47 = "B" + (F + 47).ToString();
                    string D47 = "D" + (F + 47).ToString();
                    string F47 = "F" + (F + 47).ToString();
                    string J47 = "J" + (F + 47).ToString();

                    excelSheet.Range[B47, D47].Merge();
                    excelSheet.Range[F47, J47].Merge();

                    string A47 = "A" + (F + 47).ToString();
                    excelSheet.Range[A47, A47].Value = "TOTAL";

                    string E47 = "E" + (F + 47).ToString();
                    string E46 = "E" + (F + 46).ToString();

                    string E9 = "E" + (F + 9).ToString();
                    string CT = "E" + (F - 4).ToString();
                    excelSheet.Range[E9, E9].Formula = "= IFERROR(ROUND(" + CT + ",2),\"\") ";

                    string K9 = "K" + (F + 9).ToString();
                    string AM = "K" + (F - 4).ToString();
                    excelSheet.Range[K9, K9].Formula = "= IFERROR(ROUND(" + AM + ",2),\"\") ";

                    excelSheet.Range[E47, E47].Formula = "=ROUND(SUM(" + E9 + ":" + E46 + "),2) ";

                    string K47 = "K" + (F + 47).ToString();

                    string K46 = "K" + (F + 46).ToString();
                    excelSheet.Range[K47, K47].Formula = "=ROUND(SUM(" + K9 + ":" + K46 + "),2) ";

                    if (i == PAGE - 1)
                    {
                        string F47F = "F" + (F + 47).ToString();
                        excelSheet.Range[F47F, F47F].Value = "Total US Dollar $";
                        excelSheet.Range[F47F, F47F].HorizontalAlignment = -4152;

                        //                        Left: -4131
                        //Center: -4108
                        //Right: -4152

                        excelSheet.Range[A47].HorizontalAlignment = -4152;

                        string K48 = "K" + (F + 48).ToString();
                        //excelSheet.Range[K48, K48].Formula = "= IFERROR(ROUND(" + K47 + "*" + rate + ",2),\"\") ";

                        string A48 = "A" + (F + 48).ToString();
                        string H48 = "H" + (F + 48).ToString();
                        excelSheet.Range[A48, H48].Merge();
                        excelSheet.Range[A48, A48].Value = amount_word;

                        string I48 = "I" + (F + 48).ToString();
                        string J48 = "J" + (F + 48).ToString();
                        excelSheet.Range[I48, J48].Merge();
                        excelSheet.Range[I48, I48].Value = "Total INR. Rupees";

                        string A49 = "A" + (F + 49).ToString();
                        string E52 = "E" + (F + 52).ToString();
                        excelSheet.Range[A49, E52].Merge();

                        string F49 = "F" + (F + 49).ToString();
                        string H49 = "H" + (F + 49).ToString();
                        string F50 = "F" + (F + 50).ToString();
                        string H51 = "H" + (F + 51).ToString();
                        string F52 = "F" + (F + 52).ToString();
                        string H52 = "H" + (F + 52).ToString();

                        excelSheet.Range[F49, H49].Merge();
                        excelSheet.Range[F49, F49].Value = "For-" + LblCompanyName.Text;
                        excelSheet.Range[F50, H51].Merge();
                        excelSheet.Range[F52, H52].Merge();
                        excelSheet.Range[F52, F52].Value = "PARTNER";


                        string I49 = "I" + (F + 49).ToString();
                        string L52 = "L" + (F + 52).ToString();
                        excelSheet.Range[I49, L52].Merge();



                        excelSheet.Range[A2, L52].Borders.LineStyle = BorderStyle.FixedSingle;
                        excelSheet.Range[A2, L52].Borders.Color = Color.Black;
                    }
                    else
                    {
                        string A48 = "A" + (F + 48).ToString();
                        string E51 = "E" + (F + 51).ToString();

                        excelSheet.Range[A48, E51].Merge();

                        string F48 = "F" + (F + 48).ToString();
                        string H48 = "H" + (F + 48).ToString();
                        string F49 = "F" + (F + 49).ToString();
                        string H50 = "H" + (F + 50).ToString();
                        string F51 = "F" + (F + 51).ToString();
                        string H51 = "H" + (F + 51).ToString();

                        excelSheet.Range[F48, H48].Merge();
                        excelSheet.Range[F48, F48].Value = "For- " + LblCompanyName.Text;

                        excelSheet.Range[F49, H50].Merge();
                        excelSheet.Range[F51, H51].Merge();
                        excelSheet.Range[F51, F51].Value = "PARTNER";


                        string I48 = "I" + (F + 48).ToString();
                        string L51 = "L" + (F + 51).ToString();
                        excelSheet.Range[I48, L51].Merge();

                        excelSheet.Range[A2, L51].Borders.LineStyle = BorderStyle.FixedSingle;
                        excelSheet.Range[A2, L51].Borders.Color = Color.Black;

                    }
                    F = F + 51;
                }
            }

            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            excelApp.DisplayAlerts = false;
            excelWorkbook.SaveCopyAs(outputPath);
            excelWorkbook.Saved = true;

            //string StrFilrPath = AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\LabIssueStone1.pdf";
            //excelWorkbook.SaveAs(outputPath,".pdf");

            excelWorkbook.Close(true, Type.Missing, Type.Missing);
            excelWorkbook = null;

            // Release the Application object
            excelApp.Quit();
            excelApp = null;
        }


        public string ExportExcelWithIGIStock(DataSet DS, string PStrFilePath) //Add Khushbu 12-07-21
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
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("IGI Stock List");

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

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    #endregion

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();


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

        private void txtBrokername_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "BEOKERNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BROKER);

                    FrmSearch.mStrColumnsToHide = "BROKER_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        //txtBrokername.Text = Val.ToString(FrmSearch.DRow["BROKERNAME"]);
                        //txtBrokername.Tag = Val.ToString(FrmSearch.DRow["BROKER_ID"]);
                    }

                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            //ADDED BY GUNJAN:21/05/2024
            if (Val.ToString(BOConfiguration.COMPANYNAME).ToUpper() == "TRP IMPEX")//TRP IMPEX
            {
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
                        DataTable DTab = ObjMemo.GetJangedPrintDataNew(StrJangedNo);

                        if (DTab.Rows.Count == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("There Is No Data Found For Print");
                            return;
                        }

                        int TotalRow = 14;
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

                        //Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        //FrmReportViewer.MdiParent = Global.gMainRef;
                        //FrmReportViewer.ShowFormPrintWithDuplicate("RPT_MemoJangedPrint", DS);
                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        FrmReportViewer.MdiParent = Global.gMainRef;
                        FrmReportViewer.ShowFormPrintWithDuplicate("RPT_MemoJangedPrintNew", DS);

                        //Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                        //FrmReportViewer.MdiParent = Global.gMainRef;
                        //FrmReportViewer.ShowForm("RPT_MemoJangedPrint", DTab);
                        this.Cursor = Cursors.Default;
                    }

                    catch (Exception ex)
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message(ex.Message);
                    }
                }
            }
            if (Val.ToString(BOConfiguration.COMPANYNAME).ToUpper() == "RIJIYA GEMS")
            {
                
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
                        FrmReportViewer.ShowFormPrintWithDuplicate("RPT_MemoJangedPrintRJ", DS);

                        this.Cursor = Cursors.Default;
                    }

                    catch (Exception ex)
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message(ex.Message);
                    }
                }
            }
            //END AS GUNJAN
        }
    }



}

