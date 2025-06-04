using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using AxonDataLib;
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
using BusLib.EInvoice;
using Microsoft.VisualBasic;
using System.Net;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using Microsoft.VisualBasic.CompilerServices;
using BusLib.Rapaport;
using System.Data.SqlClient;

namespace BusLib.EInvoice
{
    public class BOTRN_EInvoiceAPI
    {
        public static DataTable dt = new DataTable();
        public static string result = "";
        public static string AuthToken = "";
        public static string sek = "";
        public static string key = "";
        public static string TokenNo = "";
        public static string AckNo = "";
        public static string IrnNo = "";
        public static string SignedInvoice = "";
        public static string SignedQRCode = "";
        public static string ErrorMessage = "";

        #region ::"Defult Funaction"::

        /**EncryptAsymmetric*/
        //public static string EncryptAsymmetric(string data, string key)
        //{
        //    byte[] keyBytes = Convert.FromBase64String(key);
        //    AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
        //    RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
        //    RSAParameters rsaParameters = new RSAParameters();
        //    rsaParameters.Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned();
        //    rsaParameters.Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned();
        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //    rsa.ImportParameters(rsaParameters);
        //    byte[] plaintext = Encoding.UTF8.GetBytes(data);
        //    byte[] ciphertext = rsa.Encrypt(plaintext, false);
        //    string cipherresult = Convert.ToBase64String(ciphertext);
        //    return cipherresult;
        //}

        public static byte[] generateSecureKey()
        {
            Aes KEYGEN = Aes.Create();
            byte[] secretKey = KEYGEN.Key;
            return secretKey;
        }
        //public static string Encrypt(byte[] data, string key)
        //{
        //    byte[] keyBytes = Convert.FromBase64String(key);
        //    AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
        //    RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
        //    RSAParameters rsaParameters = new RSAParameters();
        //    rsaParameters.Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned();
        //    rsaParameters.Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned();
        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //    rsa.ImportParameters(rsaParameters);
        //    byte[] plaintext = data;
        //    byte[] ciphertext = rsa.Encrypt(plaintext, false);
        //    string cipherresult = Convert.ToBase64String(ciphertext);
        //    return cipherresult;
        //}
        public static string DecryptBySymmetricKeyEBill(string encryptedText, string key)
        {

            //Decrypting SEK
            try
            {

                byte[] dataToDecrypt = Convert.FromBase64String(encryptedText);
                var keyBytes = Convert.FromBase64String(key);
                AesManaged tdes = new AesManaged();
                tdes.KeySize = 256;
                tdes.BlockSize = 128;
                tdes.Key = keyBytes;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform decrypt__1 = tdes.CreateDecryptor();
                byte[] deCipher = decrypt__1.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
                tdes.Clear();
                //return Convert.ToBase64String(deCipher);
                string EK_result = Convert.ToBase64String(deCipher);
                //byte[] toBytes = Encoding.ASCII.GetBytes(EK_result);
                return EK_result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable ConvertJsonToDataTable(string Json)
        {
            try
            {
                string[] jsonParts = Regex.Split(Json.Replace("[", "").Replace("]", ""), "},{");
                dt = new DataTable();
                List<string> dtcolumn = new List<string>();
                foreach (string jp in jsonParts)
                {
                    string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",");
                    foreach (string rowData in propData)
                    {
                        try
                        {
                            int idex = rowData.IndexOf(":");
                            string n = rowData.Substring(0, idex - 1);
                            string v = rowData.Substring(idex + 1);
                            if (!dtcolumn.Contains(n))
                            {
                                dtcolumn.Add(n.Replace("\"", ""));
                            }
                        }
                        catch (Exception)
                        {

                            throw new Exception(string.Format("Error Parasing Column Name : (0)", rowData));
                        }
                    }
                    break;
                }
                foreach (string c in dtcolumn)
                {
                    dt.Columns.Add(c);
                }
                foreach (string jp in jsonParts)
                {
                    string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",");
                    DataRow nr = dt.NewRow();
                    foreach (string rowData in propData)
                    {
                        try
                        {
                            int idex = rowData.IndexOf(":");
                            string n = rowData.Substring(0, idex - 1).Replace("\"", "");
                            string v = rowData.Substring(idex + 1).Replace("\"", "");
                            nr[n] = v;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    dt.Rows.Add(nr);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public static string DecryptBySymmetricKey(string encryptedText, byte[] key)
        {
            //Decrypting SEK
            try
            {

                byte[] dataToDecrypt = Convert.FromBase64String(encryptedText);
                var keyBytes = key;
                AesManaged tdes = new AesManaged();
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

        public static string EncryptBySymmetricKey(string text, string sek)
        {
            //Encrypting SEK
            try
            {
                byte[] dataToEncrypt = Convert.FromBase64String(text);
                var keyBytes = Convert.FromBase64String(sek);
                AesManaged tdes = new AesManaged();
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

        #endregion

        #region ::"GetToken"::
        //public GetTokenData GetToken(string ClientId, string ClientSecretId, string Gstin)
        //{
        //    try
        //    {
        //        GetTokenData Obj = new GetTokenData();
        //        string public_key = "";
        //        using (var reader = File.OpenText(Application.StartupPath + @"\\einv_sandbox.pem"))
        //        {
        //            public_key = reader.ReadToEnd().Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace(Constants.vbLf, "");
        //        }
        //        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

        //        string uri = "https://einv-apisandbox.nic.in/eivital/v1.04/auth";
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        //        request.Method = "POST";
        //        request.KeepAlive = true;
        //        // request.Timeout = 100000;

        //        request.ProtocolVersion = HttpVersion.Version11;
        //        request.ServicePoint.Expect100Continue = false;
        //        request.AllowAutoRedirect = false;
        //        request.Accept = "*/*";
        //        request.UnsafeAuthenticatedConnectionSharing = true;

        //        request.ContentType = "application/json";
        //        request.Headers.Add("client_id", ClientId);
        //        request.Headers.Add("client_secret", ClientSecretId);
        //        request.Headers.Add("Gstin", Gstin);

        //        string encPassword = EncryptAsymmetric("SKE123!@#", public_key);
        //        byte[] _aeskey = generateSecureKey(); //common.RandomString(32); //
        //        string straesKey = Convert.ToBase64String(_aeskey);
        //        string encAppKey = Encrypt(_aeskey, public_key);

        //        var data = new
        //        {
        //            UserName = "SKE",
        //            Password = "SKE123!@#",
        //            AppKey = straesKey,
        //            ForceRefreshAccessToken = true
        //        };

        //        var serializer = new JavaScriptSerializer();
        //        var json = serializer.Serialize(data);

        //        string authStr = JsonConvert.SerializeObject(data);
        //        byte[] authBytes = System.Text.Encoding.UTF8.GetBytes(authStr);
        //        RequestPayloadN EW = serializer.Deserialize<RequestPayloadN>(json);
        //        EW.Data = EncryptAsymmetric(Convert.ToBase64String(authBytes), public_key);
        //        string abc = JsonConvert.SerializeObject(EW);

        //        Byte[] byteArray = Encoding.UTF8.GetBytes(abc);
        //        using (Stream dataStream = request.GetRequestStream())
        //        {
        //            dataStream.Write(byteArray, 0, byteArray.Length);
        //            using (WebResponse tResponse = request.GetResponse())
        //            {
        //                using (Stream stream = tResponse.GetResponseStream())
        //                {
        //                    StreamReader streamreader = new StreamReader(stream);
        //                    result = streamreader.ReadToEnd();
        //                    DataTable dtResult = ConvertJsonToDataTable(result);
        //                    if (dtResult.Rows.Count > 0)
        //                    {
        //                        if (Convert.ToInt32(dtResult.Rows[0]["Status"]) == 1)
        //                        {

        //                            foreach (DataRow dr in dt.Rows)
        //                            {
        //                                Obj.AuthToken = dr["AuthToken"].ToString();
        //                                Obj.Sek = dr["sek"].ToString();
        //                                Obj.Key = DecryptBySymmetricKey(dr["sek"].ToString(), _aeskey);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            return Obj;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        #endregion

        #region ::"Get INR No::"
        public GetINRData GenerateINR(string ClientId, string ClientSecretId, string GSTIn, string UserName, string TokenNo, string SkeKey,string JsonData)
        {
            try
            {
                GetINRData Obj = new GetINRData();
                string public_key = "";
                using (var reader = File.OpenText(Application.StartupPath + @"\\einv_sandbox.pem"))
                {
                    public_key = reader.ReadToEnd().Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace(Constants.vbLf, "");
                }

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                string uri = "https://einv-apisandbox.nic.in/eicore/v1.03/Invoice";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.KeepAlive = true;
                request.ProtocolVersion = HttpVersion.Version10;
                request.ServicePoint.Expect100Continue = false;
                request.AllowAutoRedirect = false;
                request.Accept = "*/*";
                request.UnsafeAuthenticatedConnectionSharing = true;
                request.ContentType = "application/json";

                request.Headers.Add("client_id", ClientId);
                request.Headers.Add("client_secret", ClientSecretId);
                request.Headers.Add("Gstin", GSTIn);
                request.Headers.Add("user_name", UserName);
                request.Headers.Add("AuthToken", TokenNo);

                byte[] _aeskey = generateSecureKey(); //common.RandomString(32); //
                string straesKey = Convert.ToBase64String(_aeskey);
              
                var data = JsonData;
                data = data.Replace("\t", "");

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);

                RequestPayloadN requestPayload = serializer.Deserialize<RequestPayloadN>(data);
                requestPayload.Data = EncryptBySymmetricKey(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data)), SkeKey);

                using (var StreamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string Json = serializer.Serialize(requestPayload);
                    StreamWriter.Write(Json);
                    StreamWriter.Flush();
                    StreamWriter.Close();
                }

                WebResponse response = request.GetResponse();
                string Result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                serializer = new JavaScriptSerializer();
                EinvoiceApiRequest EW = serializer.Deserialize<EinvoiceApiRequest>(Result);
                if (EW.Data != null)
                {
                    string value = DecryptBySymmetricKeyEBill(EW.Data, SkeKey);
                    byte[] reqDataBytes = Convert.FromBase64String(value);
                    string requestData = System.Text.Encoding.UTF8.GetString(reqDataBytes);
                    DataTable dtResult = ConvertJsonToDataTable(requestData);
                    if (dtResult.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Obj.AckNo = dr["AckNo"].ToString();
                            Obj.IrnNo = dr["Irn"].ToString();
                            Obj.SignedInvoice = dr["SignedInvoice"].ToString();
                            Obj.SignedQRCode = dr["SignedInvoice"].ToString();
                            Obj.AckDt = dr["AckDt"].ToString();
                        }
                    }
                }
                return Obj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ::"Cancel INR No::"

        public GetCancelINR CancelNR(string clientId, string ClientSecretId, string GSTIN, string USERNAME, string TokenNo, string JSONDATA,string Skey)
        {
            try
            {
                GetCancelINR Obj = new GetCancelINR();
                string public_key = "";
                using (var reader = File.OpenText(Application.StartupPath + @"\\einv_sandbox.pem"))
                {
                    public_key = reader.ReadToEnd().Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace(Constants.vbLf, "");
                }

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

                string uri = "https://einv-apisandbox.nic.in/eicore/v1.03/Invoice/Cancel";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.KeepAlive = true;
                request.ProtocolVersion = HttpVersion.Version10;
                request.ServicePoint.Expect100Continue = false;
                request.AllowAutoRedirect = false;
                request.Accept = "*/*";
                request.UnsafeAuthenticatedConnectionSharing = true;
                request.ContentType = "application/json";

                request.Headers.Add("client_id", clientId);
                request.Headers.Add("client_secret", ClientSecretId);
                request.Headers.Add("Gstin", GSTIN);
                request.Headers.Add("user_name", USERNAME);
                request.Headers.Add("AuthToken", TokenNo);

                EinvoiceApiRequest eprq = new EinvoiceApiRequest();
                JavaScriptSerializer serial1 = new JavaScriptSerializer();

                var data = JSONDATA;
                data = data.Replace("\t", "");

                var json = serial1.Serialize(data);
                eprq.Data = EncryptBySymmetricKey(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json)), Skey);
                using (var streamwriter = new StreamWriter(request.GetRequestStream()))
                {
                    string Json = serial1.Serialize(eprq);
                    streamwriter.Write(Json);
                    streamwriter.Flush();
                    streamwriter.Close();
                }
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                serial1 = new JavaScriptSerializer();
                EinvoiceApiRequest Epapi = serial1.Deserialize<EinvoiceApiRequest>(result);
                if (Epapi.Data != null)
                {
                    string data1 = DecryptBySymmetricKeyEBill(Epapi.Data, Skey);
                    byte[] reqDatabytes = Convert.FromBase64String(data1);
                    string requestData = System.Text.Encoding.UTF8.GetString(reqDatabytes);
                    DataTable dtResult = ConvertJsonToDataTable(requestData);
                    if (dtResult.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Obj.Irn = dr["Irn"].ToString();
                            Obj.CancelDate = dr["CancelDate"].ToString();
                        }
                    }
                }
                return Obj;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

    }

    #region ::"E-INOVICE API PROPERTY"::

    public class RequestPayloadN
    {
        public string Data { get; set; }
    }

    public class GetTokenData
    {
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public string AuthToken { get; set; }
        public string Sek { get; set; }
        public string Key { get; set; }
        public string TokenExpiry { get; set; }
    }

    public class EinvoiceApiRequest
    {
        public string Data { get; set; }
        public string rek { get; set; }
        public List<EinvoiceApiRequest> Result { get; set; }
    }


    public class GetINRData
    {
        public string AckNo { get; set; }
        public string IrnNo { get; set; }
        public string SignedInvoice { get; set; }
        public string SignedQRCode { get; set; }
        public string ErrorMeeasge { get; set; }
        public int Status { get; set; }
        public string AckDt { get; set; }
    }


    public class GetCancelINR
    {
        public string Irn { get; set; }
        public string CancelDate { get; set; }
    }
    #endregion
}
