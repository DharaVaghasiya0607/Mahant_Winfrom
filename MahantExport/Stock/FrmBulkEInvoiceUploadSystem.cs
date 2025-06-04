using MahantExport.Stock;
using MahantExport.Utility;
using BusLib;
using BusLib.Configuration;
using BusLib.EInvoice;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
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
using System.Windows.Forms;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Microsoft.VisualBasic;
using BusLib.EInvoice;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using QRCoder;
using BarcodeLib.Barcode;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace MahantExport.Account
{
    public partial class FrmBulkEInvoiceUploadSystem : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_EInvoice ObjEInvoice = new BOTRN_EInvoice();

        DataTable DTabSummary = new DataTable();

        #region Property Settings

        public FrmBulkEInvoiceUploadSystem()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;

            DTPFromDate.Focus();
            RdbViewType.SelectedIndex = 0;
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            bgw.WorkerReportsProgress = true;
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


        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Memo Summary", GrdSummary);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string StrFromDate = "";
                string StrToDate = "";

                MainGrdSummary.DataSource = null;

                if (DTPFromDate.Checked)
                {
                    StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                }
                if (DTPToDate.Checked)
                {
                    StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                }

                if (txtBillTo.Text.Length == 0) txtBillTo.Tag = null;

                string StrStatus = "";
                if (RdbViewType.SelectedIndex == 0)
                    StrStatus = "ALL";
                else if (RdbViewType.SelectedIndex == 1)
                    StrStatus = "PENDING";
                else
                    StrStatus = "UPLOADED";


                DataSet DS = ObjEInvoice.GetSaleBillBulkEInvoiceUpload(StrFromDate, StrToDate, Val.ToString(txtBillTo.Tag), StrStatus);

                GrdSummary.BeginUpdate();
                DTabSummary = DS.Tables[0];

                MainGrdSummary.DataSource = DTabSummary;
                MainGrdSummary.Refresh();

                GrdSummary.BestFitColumns();
                GrdSummary.EndUpdate();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            GrdSummary.BestFitColumns();
        }

        private void GrdSummary_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }
            if (e.Clicks == 2 && e.Column.FieldName == "JANGEDNOSTR")
            {
                //FrmMemoEntryAccounting FrmMemoEntryAccounting = new FrmMemoEntryAccounting();
                //FrmMemoEntryAccounting.MdiParent = Global.gMainRef;
                //FrmMemoEntryAccounting.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                //FrmMemoEntryAccounting.ShowForm(Stock.FrmMemoEntryAccounting.FORMTYPE.SALEINVOICE, Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")), "SINGLE");
            }

            if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("ISEINVOICEUPLOAD") && Val.ToInt32(GrdSummary.GetFocusedRowCellValue("ISEINVOICEUPLOAD")) == 1)
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


            if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("ISEINVOICEPRINT") && Val.ToInt32(GrdSummary.GetFocusedRowCellValue("ISEINVOICEPRINT")) == 1)
            {
                PnlLoading.Visible = true;
                bgw.RunWorkerAsync(3);
            }


            if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("ISEINVOICECANCEL") && Val.ToInt32(GrdSummary.GetFocusedRowCellValue("ISEINVOICECANCEL")) == 1)
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

        }

        private void FrmMemoEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnSearch.PerformClick();
        }

        private void txtBillTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    DataTable DtabParty = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
                    FrmSearch.mDTab = DtabParty;

                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBillTo.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBillTo.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        #region E-Invoice Events And Method
        //For E-Invoice
        string AuthToken = "";
        string key = "";
        string AckNo = "";
        string IrnNo = "";
        string IrnDate = "";
        string StrCancelDate = "";

        string status = "";
        string SignedInvoice = "";
        string SignedQRCode = "";
        string StrMessage = "";
        int IntIsError = 0;

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

        public DataTable ToJsonDataTable(List<Output> list)
        {
            var dt = new DataTable();

            // insert enough amount of rows
            var numRows = list.Select(x => x.results.Length).Max();
            for (int i = 0; i < numRows; i++)
                dt.Rows.Add(dt.NewRow());

            // process the data
            foreach (var field in list)
            {
                dt.Columns.Add(field.name);
                for (int i = 0; i < numRows; i++)
                    // replacing missing values with empty strings
                    dt.Rows[i][field.name] = i < field.results.Length ? field.results[i] : string.Empty;
            }

            return dt;
        }

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

        //REF Code Do Not Delete 
        //private void BtnEInvoicePrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    DataRow DRow = ObjEInvoice.GetEInvoiceExists(Val.ToString(lblMemoNo.Tag));
        //    if (DRow == null)
        //    {
        //        Global.MessageError("First Generate IRN Number For E-Invoice , Upload Your Invoice");
        //        return;
        //    }
        //    AuthToken = GetToken(ref key);
        //    if (AuthToken == "")
        //    {
        //        Global.MessageError("No Any Token Generated For Invoice Posing");
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
        //            //string uri = dtsales[5]("Value").ToString() + "Invoice"; // "https://einv-apisandbox.nic.in/eicore/v1.03/Invoice"
        //            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Property.URL + "/Invoice/irn/" + Val.ToString(DRow["IRNNO"]));
        //            request.Method = "GET";
        //            request.KeepAlive = true;
        //            request.ProtocolVersion = HttpVersion.Version10;
        //            request.ServicePoint.Expect100Continue = false;
        //            request.AllowAutoRedirect = false;
        //            request.Accept = "*/*";
        //            request.UnsafeAuthenticatedConnectionSharing = true;
        //            request.ContentType = "application/json";
        //            request.Headers.Add("client_id", Property.CLIENTID);
        //            request.Headers.Add("client_secret", Property.CLIENTSECRET);
        //            request.Headers.Add("Gstin", Property.GSTIN);
        //            request.Headers.Add("user_name", Property.USERNAME);
        //            request.Headers.Add("AuthToken", AuthToken);

        //            try
        //            {
        //                WebResponse response = request.GetResponse();
        //                string Result = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //                var serializerResult = new JavaScriptSerializer();
        //                var item = serializerResult.Deserialize<DataRow>(Result);
        //                int status = Val.ToInt(item["Status"]);
        //                if (status == 0)
        //                {
        //                    string msg = "";
        //                    for (int i = 0; i <= Val.ToInt(item["ErrorDetails"]); i++)
        //                        msg += Val.ToString(item["ErrorMessage"]) + Environment.NewLine;
        //                    Global.MessageError(msg);
        //                    return;
        //                }


        //                DataTable dtresult = ConvertJsonToDataTable(Result);
        //                if (dtresult.Rows.Count > 0)
        //                {
        //                    if (Val.ToInt(dtresult.Rows[0]["Status"]) == 0)
        //                    {
        //                        Global.MessageError(Val.ToString(dtresult.Rows[0]["ErrorMessage"]));
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //                        Acct_EinvoiceApiRequest EW = serializer.Deserialize<Acct_EinvoiceApiRequest>(Result);
        //                        string value = DecryptBySymmetricKeyEBill(EW.Data, key);
        //                        var reqDataBytes = Convert.FromBase64String(value);
        //                        string requestData = Encoding.UTF8.GetString(reqDataBytes);
        //                        DataTable dt = ConvertJsonToDataTable(requestData);
        //                        foreach (DataRow dr in dt.Rows)
        //                        {
        //                            AckNo = Val.ToString(dr["AckNo"]);
        //                            IrnNo = Val.ToString(dr["Irn"]);
        //                            IrnDate = Val.ToString(dr["AckDt"]);
        //                            SignedInvoice = Val.ToString(dr["SignedInvoice"]);
        //                            SignedQRCode = Val.ToString(dr["SignedQRCode"]);
        //                        }
        //                        string result2 = "";
        //                        string resultFinal = "";
        //                        result2 = Decode(SignedInvoice);
        //                        resultFinal = DecodePayload(SignedInvoice);
        //                        Acct_EinvoiceApiRequest Einvre = serializer.Deserialize<Acct_EinvoiceApiRequest>(resultFinal);
        //                        string Data = Einvre.Data;
        //                        var dsRDS = new DataSet();
        //                        var dtFinal = new DataTable();
        //                        XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(Data, "friends");
        //                        string str1 = doc.InnerXml.Replace("<TranDtls>", "").Replace("</TranDtls>", "").Replace("<DocDtls>", "").Replace("</DocDtls>", "").Replace("<SellerDtls>", "").Replace("</SellerDtls>", "").Replace("<BuyerDtls>", "").Replace("</BuyerDtls>", "").Replace("<DispDtls>", "").Replace("</DispDtls>", "").Replace("<ItemList>", "").Replace("</ItemList>", "").Replace("<ValDtls>", "").Replace("</ValDtls>", "");
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
        //                        }

        //                        Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
        //                        FrmReportViewer.MdiParent = Global.gMainRef;
        //                        FrmReportViewer.ShowForm("rptEInvoice", dtFinal);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Global.MessageError(ex.Message.ToString());
        //                return;
        //            }
        //        }
        //    }
        //}
        //private void BtnEInvoiceUpload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        if (Global.Confirm("Do You Want To Upload E-Invoice On GSTR ?") == System.Windows.Forms.DialogResult.No)
        //        {
        //            return;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Message(ex.Message);
        //    }

        //    AuthToken = GetToken(ref key);
        //    if (AuthToken == "")
        //    {
        //        Global.MessageError("No Any Token Generated For Invoice Posing");
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
        //            //string uri = dtsales[5]("Value").ToString() + "Invoice"; // "https://einv-apisandbox.nic.in/eicore/v1.03/Invoice"
        //            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Property.URL + "/Invoice");
        //            request.Method = "POST";
        //            request.KeepAlive = true;
        //            request.ProtocolVersion = HttpVersion.Version10;
        //            request.ServicePoint.Expect100Continue = false;
        //            request.AllowAutoRedirect = false;
        //            request.Accept = "*/*";
        //            request.UnsafeAuthenticatedConnectionSharing = true;
        //            request.ContentType = "application/json";
        //            request.Headers.Add("client_id", Property.CLIENTID);
        //            request.Headers.Add("client_secret", Property.CLIENTSECRET);
        //            request.Headers.Add("Gstin", Property.GSTIN);
        //            request.Headers.Add("user_name", Property.USERNAME);
        //            request.Headers.Add("AuthToken", AuthToken);
        //            byte[] _aeskey = GenerateSecureKey();
        //            string straesKey = Convert.ToBase64String(_aeskey);

        //            // Dim JsonData As String = objsales.GetData_EInvoice(txtInvoiceNo.Text)
        //            DataSet dsJson = ObjEInvoice.GetEInvoiceInvoiceInfo(Val.ToString(lblMemoNo.Tag));

        //            if (dsJson.Tables.Count == 0)
        //            {
        //                Global.MessageError("No Data Found For Upload");
        //                return;
        //            }

        //            string JsonData = Val.ToString(dsJson.Tables[0].Rows[0]["Json"]);

        //            var serializer = new JavaScriptSerializer();
        //            var EPREQ = new Acct_EinvoiceApiRequest();
        //            EPREQ.Data = EncryptBySymmetricKey(Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonData)), key);
        //            string Json1;
        //            using (var StreamWriter = new StreamWriter(request.GetRequestStream()))
        //            {
        //                Json1 = serializer.Serialize(EPREQ);
        //                StreamWriter.Write(Json1);
        //                StreamWriter.Flush();
        //                StreamWriter.Close();
        //            }

        //            try
        //            {
        //                WebResponse response = request.GetResponse();
        //                string Result = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //                var serializerResult = new JavaScriptSerializer();
        //                var item = serializerResult.Deserialize<DataRow>(Result);
        //                int status = Val.ToInt(item["Status"]);
        //                if (status == 0)
        //                {
        //                    string msg = "";
        //                    for (int i = 0; i <= Val.ToInt(item["ErrorDetails"]); i++)
        //                        msg += Val.ToString(item["ErrorMessage"]) + Environment.NewLine;
        //                    Global.MessageError(msg);
        //                    return;
        //                }
        //                DataTable dtresult = ConvertJsonToDataTable(Result);
        //                if (dtresult.Rows.Count > 0)
        //                {
        //                    if (Val.ToInt(dtresult.Rows[0]["Status"]) == 0)
        //                    {
        //                        Global.MessageError(Val.ToString(dtresult.Rows[0]["ErrorMessage"]));
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        serializer = new JavaScriptSerializer();
        //                        Acct_EinvoiceApiRequest EW = serializer.Deserialize<Acct_EinvoiceApiRequest>(Result);
        //                        string value = DecryptBySymmetricKeyEBill(EW.Data, key);
        //                        var reqDataBytes = Convert.FromBase64String(value);
        //                        string requestData = Encoding.UTF8.GetString(reqDataBytes);
        //                        DataTable dt = ConvertJsonToDataTable(requestData);
        //                        foreach (DataRow dr in dt.Rows)
        //                        {
        //                            AckNo = Val.ToString(dr["AckNo"]);
        //                            IrnNo = Val.ToString(dr["Irn"]);
        //                            IrnDate = Val.ToString(dr["AckDt"]);
        //                            SignedInvoice = Val.ToString(dr["SignedInvoice"]);
        //                            SignedQRCode = Val.ToString(dr["SignedQRCode"]);
        //                        }

        //                        int RetValue = ObjEInvoice.InsertEInvoiceDetail(Val.ToString(lblMemoNo.Tag), AckNo, IrnNo, Val.SqlDate(IrnDate), SignedInvoice, SignedQRCode, Property);
        //                        if (RetValue == -1)
        //                        {
        //                            Global.MessageError("Record Not Inserted");
        //                            return;
        //                        }

        //                        Global.MessageError("E-Invoice Upload Successfully");
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Global.MessageError(ex.Message.ToString());
        //                return;
        //            }
        //        }
        //    }

        //}
        //public string GetToken(ref string key)
        //{
        //    try
        //    {
        //        this.Cursor = Cursors.WaitCursor;
        //        string public_key = "";
        //        string result = "";
        //        string sek = "";
        //        string AuthToken = "";

        //        using (var reader = File.OpenText(Application.StartupPath + @"\\einv_sandbox.pem"))
        //        {
        //            public_key = reader.ReadToEnd().Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace(Constants.vbLf, "");
        //        }
        //        string Json = "";

        //        TRN_EInvoiceProperty Property = ObjEInvoice.GetEInvoiceCredential(BusLib.Configuration.BOConfiguration.COMPANY_ID);
        //        if (Property.CLIENTID == "")
        //        {
        //            this.Cursor = Cursors.Default;
        //            Global.MessageError("Client ID IS Missing. Please Check In Company Master");
        //            return string.Empty;
        //        }
        //        else if (Property.GSTIN == "")
        //        {
        //            this.Cursor = Cursors.Default;
        //            Global.MessageError("GSTIN IS Missing. Please Check In Company Master");
        //            return string.Empty;
        //        }
        //        else if (Property.USERNAME == "")
        //        {
        //            this.Cursor = Cursors.Default;
        //            Global.MessageError("Username IS Missing. Please Check In Company Master");
        //            return string.Empty;
        //        }
        //        else if (Property.PASSWORD == "")
        //        {
        //            this.Cursor = Cursors.Default;
        //            Global.MessageError("Password IS Missing. Please Check In Company Master");
        //            return string.Empty;
        //        }
        //        else if (Property.CLIENTSECRET == "")
        //        {
        //            this.Cursor = Cursors.Default;
        //            Global.MessageError("CLIENT Secreat IS Missing. Please Check In Company Master");
        //            return string.Empty;
        //        }
        //        else if (Property.TOKENURL == "")
        //        {
        //            this.Cursor = Cursors.Default;
        //            Global.MessageError("E-Invoice Token URL IS Missing. Please Check In Company Master");
        //            return string.Empty;
        //        }
        //        else if (Property.URL == "")
        //        {
        //            this.Cursor = Cursors.Default;
        //            Global.MessageError("E-Invoice  URL IS Missing. Please Check In Company Master");
        //            return string.Empty;
        //        }

        //        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xC0 | 0x300 | 0xC00);
        //        // Dim uri As String = "https://einv-apisandbox.nic.in/eivital/v1.03/auth"

        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Property.TOKENURL);
        //        request.Method = "POST";
        //        request.KeepAlive = true;
        //        request.ProtocolVersion = HttpVersion.Version11;
        //        request.ServicePoint.Expect100Continue = false;
        //        request.AllowAutoRedirect = false;
        //        request.Accept = "*/*";
        //        request.UnsafeAuthenticatedConnectionSharing = true;
        //        request.ContentType = "application/json";
        //        request.Headers.Add("client_id", Property.CLIENTID);
        //        request.Headers.Add("client_secret", Property.CLIENTSECRET);
        //        string encPassword = EncryptAsymmetric(Property.PASSWORD, public_key);
        //        byte[] _aeskey = GenerateSecureKey();
        //        string straesKey = Convert.ToBase64String(_aeskey);
        //        string encAppKey = Encrypt(_aeskey, public_key);
        //        Json = "{\"data\":{\"UserName\":\"" + Property.USERNAME + "\",\"Password\":\"" + encPassword + "\",\"AppKey\":\"" + encAppKey + "\",\"ForceRefreshAccessToken\":true}}";
        //        var serializer = new JavaScriptSerializer();
        //        var jsondata = serializer.Serialize(Json);
        //        var byteArray = Encoding.UTF8.GetBytes(Json);
        //        using (Stream dataStream = request.GetRequestStream())
        //        {
        //            dataStream.Write(byteArray, 0, byteArray.Length);
        //            using (WebResponse tResponse = request.GetResponse())
        //            {
        //                using (Stream stream = tResponse.GetResponseStream())
        //                {
        //                    var streamreader = new StreamReader(stream);
        //                    result = streamreader.ReadToEnd();
        //                    DataTable dtresult = ConvertJsonToDataTable(result);
        //                    if (dtresult.Rows.Count > 0)
        //                    {
        //                        if (Val.ToInt(dtresult.Rows[0]["Status"]) == 0)
        //                        {
        //                            this.Cursor = Cursors.Default;
        //                            Global.MessageError(Val.ToString(dtresult.Rows[0]["ErrorMessage"]));
        //                            return "";
        //                        }
        //                        else
        //                        {
        //                            foreach (DataRow dr in dtresult.Rows) // dt.Rows
        //                            {
        //                                AuthToken = Val.ToString(dr["AuthToken"]);
        //                                sek = Val.ToString(dr["sek"]);
        //                            }
        //                            this.Cursor = Cursors.Default;

        //                            Global.Message("Token Successfuly Generated");
        //                        }
        //                    }
        //                }
        //            }

        //            key = DecryptBySymmetricKey(sek, _aeskey);
        //            return AuthToken;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Global.MessageError(ex.Message);
        //        return "";
        //    }
        //}
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
                DataSet dsJson = ObjEInvoice.GetEInvoiceInvoiceInfo(Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")));

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
                                ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, StrMessage, Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")));
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
                                ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, Val.ToString(dtresult.Rows[0]["message"]), Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")));
                                ObjEInvoice.UpdateEInvoiceUploadDate(IrnDate, Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")));
                                int RetValue = ObjEInvoice.InsertEInvoiceDetail(Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")), AckNo, IrnNo, Val.SqlDate(IrnDate), SignedInvoice, SignedQRCode, null, Property);
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
                DataSet dsJson = ObjEInvoice.GetEInvoiceCancelInvoiceInfo(Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")));

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
                                ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, StrMessage, Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")));
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
                                ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, Val.ToString(dtresult.Rows[0]["message"]), Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")));
                                ObjEInvoice.UpdateEInvoiceCancelDate(StrCancelDate, Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")));
                                int RetValue = ObjEInvoice.InsertEInvoiceDetail(Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")), AckNo, IrnNo, Val.SqlDate(IrnDate), SignedInvoice, SignedQRCode, StrCancelDate, Property);
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
            DataRow DRow = ObjEInvoice.GetEInvoiceExists(Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")));
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
                                ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, StrMessage, Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")));
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
                                ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, Val.ToString(dtresult.Rows[0]["message"]), Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")));
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

                                DataSet dsJson = ObjEInvoice.GetEInvoiceInvoiceInfo(Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")));

                                DataTable dtBuyer = dsJson.Tables[1];
                                DataTable dtSeller = dsJson.Tables[2];
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
        #endregion

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
            GrdSummary.SetFocusedRowCellValue("ISEINVOICEUPLOAD", 0);
            GrdSummary.SetFocusedRowCellValue("ISEINVOICEPRINT", 1);
            GrdSummary.SetFocusedRowCellValue("ISEINVOICECANCEL", 1);
            GrdSummary.RefreshData();
        }

        #endregion
    }


}


