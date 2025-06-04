using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
//using System.Linq;
using System.Net;
//using System.Text;
using System.Windows.Forms;
//using Amazon;
//using Amazon.S3;
//using Amazon.S3.Transfer;
//using Amazon.S3.IO;

//using System.Threading.Tasks;

//using Amazon.Runtime;

//using Amazon.S3.Model;




using System.Text;
using System.ComponentModel;
using Newtonsoft.Json;

namespace MahantExport

{
    public partial class FrmImageVideoUpload : Form
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FrmImageVideoUpload
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.HelpButton = true;
            this.Name = "FrmImageVideoUpload";
            this.ResumeLayout(false);

        }

        /*



                public FrmImageVideoUpload()
                {
                    InitializeComponent();
                }



                DATA_FUNCTION DF = new DATA_FUNCTION();
                FUNCTIONS FNC = new FUNCTIONS();

                DataTable DT = new DataTable();
                string FTP;
                string USER;
                string PASS;

                Image imgthumb = null;


                private void FRM_UPLOAD_VIC_Load(object sender, EventArgs e)
                {

                    // download_certi_gia("2446171472");
                    DT = DF.dataselect("select * from A_FTP where id='1' ");
                    FTP = DT.Rows[0]["FTP_PATH"].ToString();
                    USER = DT.Rows[0]["USERNAME"].ToString();
                    PASS = DT.Rows[0]["PASSWORD"].ToString();
                }

                private void BTN_GET_Click(object sender, EventArgs e)
                {

                    dataGridView1.DataSource = DF.EXCEUTESP("[dbo].[FRM_UPLOAD_VIC_GET]");

                    //#region IMAGE
                    //DataTable DT = new DataTable();
                    //DT = DF.dataselect("select * from A_FTP where id='1' ");


                    //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(DT.Rows[0]["FTP_PATH"].ToString());
                    //request.Method = WebRequestMethods.Ftp.ListDirectory;

                    //request.Credentials = new NetworkCredential(DT.Rows[0]["USERNAME"].ToString(), DT.Rows[0]["PASSWORD"].ToString());
                    //FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    //Stream responseStream = response.GetResponseStream();
                    //StreamReader reader = new StreamReader(responseStream);
                    //string names = reader.ReadToEnd();

                    //reader.Close();
                    //response.Close();

                    //String[] IMAGE = names.Replace("\r\n", "/").Split('/');


                    //foreach (string IM in IMAGE)
                    //{
                    //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    //    {
                    //        if (dataGridView1.Rows[i].Cells[0].Value.ToString().ToUpper() == IM.ToString().Replace(".jpg", "").Replace(".JPG", "").ToUpper())
                    //        {
                    //            dataGridView1.Rows[i].Cells["IMAGE"].Value = "DONE";
                    //            break;
                    //        }

                    //    }
                    //}
                    //#endregion

                    //#region CERTI
                    //CERTI();
                    //#endregion

                }

                public void CERTI()
                {
                    DataTable DT = new DataTable();
                    DT = DF.dataselect("select * from A_FTP where id='7' ");
                    string FTP = DT.Rows[0]["FTP_PATH"].ToString();
                    string USER = DT.Rows[0]["USERNAME"].ToString();
                    string PASS = DT.Rows[0]["PASSWORD"].ToString();
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTP);
                    request.Method = WebRequestMethods.Ftp.ListDirectory;

                    request.Credentials = new NetworkCredential(USER, PASS);
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string names = reader.ReadToEnd();

                    reader.Close();
                    response.Close();

                    String[] IMAGE = names.Replace("\r\n", "/").Split('/');


                    foreach (string IM in IMAGE)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().ToUpper() == IM.ToString().Replace(".pdf", "").Replace(".PDF", "").ToUpper())
                            {
                                dataGridView1.Rows[i].Cells["CERTI"].Value = "DONE";
                                break;
                            }

                        }
                    }
                }

                public void DELETE(string ID, string LINK, string TYPE)
                {
                    StringBuilder SB = new StringBuilder();
                    SB.Length = 0;
                    SB.AppendLine(" DELETE FROM A_IMAGE_VIDEO WHERE ID ='" + ID + "' AND IMAGE_VIDEO = '" + TYPE + "' ");

                    //SB.AppendLine(" )  ");

                    DF.datainupde(SB.ToString());
                }
                public void UPDATE(string ID, string LINK, string TYPE)
                {
                    StringBuilder SB = new StringBuilder();
                    SB.Length = 0;
                    SB.AppendLine(" DELETE FROM A_IMAGE_VIDEO WHERE ID ='" + ID + "' AND IMAGE_VIDEO = '" + TYPE + "' ");
                    SB.AppendLine(" INSERT INTO A_IMAGE_VIDEO (ID, LINK, IMAGE_VIDEO, DATE) VALUES (  ");
                    SB.AppendLine(" '" + ID + "',    ");
                    SB.AppendLine(" '" + LINK + "', ");
                    SB.AppendLine(" '" + TYPE + "', ");
                    SB.AppendLine(" CONVERT(DATETIME, '" + DateTime.Now.ToString("yyyy-MM-dd h:mm tt") + "', 102) ");

                    SB.AppendLine(" )  ");

                    DF.datainupde(SB.ToString());
                }
                private void BTN_AUTO_UPLOAD_Click(object sender, EventArgs e)
                {
                    if (!backgroundWorker1.IsBusy)
                        backgroundWorker1.RunWorkerAsync(2000);
                    else
                        MessageBox.Show("prosses is working");





                    //String ALL_STONE = @"\\191.168.7.111\g\VISION_DATA";
                    //String STOCK = @"\\191.168.7.111\g\STOCK";
                    //String SOLD = @"\\191.168.7.111\g\SOLD";

                    //DataTable dt = DF.dataselect("select top 10 s.id,i.link from A_STOCK_LIST as s left join a_image_video as i on s.id= i.id and image_video = 'video' where i.link is null");
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    vic(dt.Rows[i]["ID"].ToString(), ALL_STONE, STOCK, "COPY");
                    //}


                }
                //public void vic(string ID, string FROM_PATH, string TO_PATH, string MOVE_COPY)
                //{
                //    //StringBuilder SB = new StringBuilder();
                //    //SB.Length = 0;
                //    //SB.AppendLine("");

                //    if (MOVE_COPY == "MOVE" && ID != "")
                //    {
                //        try
                //        {
                //            Directory.Move(FROM_PATH + "\\" + ID, TO_PATH + "\\" + ID);
                //        }
                //        catch (IOException exp)
                //        {
                //            MessageBox.Show(exp.Message);
                //        }
                //    }
                //    else if (MOVE_COPY == "COPY" && ID != "")
                //    {
                //        if( Directory.Exists(FROM_PATH + "\\" + ID))
                //        {
                //            Directory.CreateDirectory(TO_PATH + "\\" + ID);
                //            foreach (string newPath in Directory.GetFiles(FROM_PATH + "\\" + ID, "*.*", SearchOption.AllDirectories))
                //            {
                //                File.Copy(newPath, newPath.Replace(FROM_PATH + "\\" + ID, TO_PATH + "\\" + ID), true);
                //            }

                //        }
                //        else
                //        {
                //            MessageBox.Show("VIDEO DON'T EXIST");
                //        }


                //    }
                //}

                private void BTN_AWS_UPLOAD_Click(object sender, EventArgs e)
                {
                    StringBuilder SB = new StringBuilder();
                    SB.Length = 0;
                    SB.AppendLine("   SELECT S.ID ,S.REPORT_NO,S.LAB,S.CARAT,   ");
                    SB.AppendLine("   V.IMAGE_VIDEO AS VIDEO,I.IMAGE_VIDEO AS [IMAGE],ITN.IMAGE_VIDEO AS THAMNIL,C.IMAGE_VIDEO AS CERTI,CP.IMAGE_VIDEO AS CERTIPLO,CD.IMAGE_VIDEO AS CERTIDIA  ,LO.LOCATION   ");
                    SB.AppendLine("   FROM H_STOCK_LIST AS S   ");
                    SB.AppendLine("   LEFT JOIN A_IMAGE_VIDEO AS V ON S.ID=V.ID AND V.IMAGE_VIDEO='VIDEO'   ");
                    SB.AppendLine("   LEFT JOIN A_IMAGE_VIDEO AS I ON S.ID=I.ID AND I.IMAGE_VIDEO='IMAGE'  LEFT JOIN A_IMAGE_VIDEO AS ITN ON S.ID=ITN.ID AND ITN.IMAGE_VIDEO='TN_IMAGE'   ");
                    SB.AppendLine("   LEFT JOIN A_IMAGE_VIDEO AS C ON S.ID=C.ID AND C.IMAGE_VIDEO='CERTI'   LEFT JOIN A_IMAGE_VIDEO AS CP ON S.ID=CP.ID AND CP.IMAGE_VIDEO='CERTIPLO'   ");
                    SB.AppendLine("   LEFT JOIN A_IMAGE_VIDEO AS CD ON S.ID=CD.ID AND CD.IMAGE_VIDEO='CERTIDIA'	   LEFT JOIN A_FRM_STOCKIN_ID AS lO ON S.ID=LO.ID  ");
                    SB.AppendLine("      ");
                    SB.AppendLine("      ");

                    dataGridView1.DataSource = DF.dataselect(SB.ToString());
                    ////AmazonUploader A = new AmazonUploader();
                    ////A.UPLOAD_FILE(@"\\191.168.7.111\g\VISION_DATA\258-14");


                    //S3Uploader a = new S3Uploader();
                    //// a.FILE_UPLOAD("252-9");
                    //// a.THUMNIL("252-9");

                    //a.GET_FOLDER_LIST();

                }

                public string GET_IMAGE_PATH()
                {
                    #region GET UPLOAD PATH FROM HELIUM.TXT

                    string FILE_PATH = "";// @"\\191.168.7.111\\vision_data\\";
                    foreach (string line in System.IO.File.ReadAllLines(@AppDomain.CurrentDomain.BaseDirectory + "HELIUM.TXT"))
                    {

                        string o = line.ToString();
                        int p = o.IndexOf("=");
                        string q = o.Substring(0, o.IndexOf("="));
                        if (q == "IMAGE")
                        {
                            FILE_PATH = line.Substring((line.IndexOf("=") + 1)).ToString();
                        }

                    }
                    #endregion
                    return FILE_PATH;
                }

                public void thumnil(string PATH)
                {
                    try
                    {
                        if (dataGridView1 != null)
                        {

                            if (File.Exists(PATH))
                            {
                                string file__name = Path.GetFileName(PATH).Replace(".", "TN.");
                                if (File.Exists(PATH.Replace(".", "TN.")))
                                {
                                    string ST = FNC.FTP_UPLOAD(PATH.Replace(".", "TN."), DT.Rows[0]["FTP_PATH"].ToString() + file__name, DT.Rows[0]["USERNAME"].ToString(), DT.Rows[0]["PASSWORD"].ToString());

                                }
                                else
                                {
                                    Image image = null;
                                    image = Image.FromFile(PATH);

                                    if (image != null)
                                    {


                                        imgthumb = image.GetThumbnailImage(200, 157, null, new IntPtr());


                                        imgthumb.Save(@PATH.Replace(".JPG", "TN.JPG"));
                                        string ST = FNC.FTP_UPLOAD(@PATH.Replace(".JPG", "TN.JPG"), DT.Rows[0]["FTP_PATH"].ToString() + file__name, DT.Rows[0]["USERNAME"].ToString(), DT.Rows[0]["PASSWORD"].ToString());

                                    }
                                }
                            }

                        }
                        //MessageBox.Show("FILE UPLOAD DONE");
                        // return "DONE";
                    }
                    catch (Exception ex)
                    {
                        // return "FAIL";
                        MessageBox.Show("FRM_UPLOAD_IMAGE:-" + ex.Message);
                    }


                }

                public void DownloadCertificateNewMethod(string REP_NO, string MUM_SURT)
                {
                    string ServiceProductKey = "";
                    if (MUM_SURT == "MUMBAI")
                    {
                        ServiceProductKey = "55311290-c723-4601-b8af-cfdba3d7e167";
                    }
                    else if (MUM_SURT == "SURAT")
                    {
                        ServiceProductKey = "8af2035d-2ea0-4dec-8bb9-094fbe69b45f";
                    }

                    string StrUrl = "https://api.reportresults.gia.edu/";
                    string ReportNumber = REP_NO;



                    string StrCertPdfFile = AppDomain.CurrentDomain.BaseDirectory + "CERTI";
                    if (Directory.Exists(StrCertPdfFile) == false)
                    {
                        Directory.CreateDirectory(StrCertPdfFile);
                    }


                    // Load the query from a file
                    string query = "";
                    try
                    {
                        // query = File.ReadAllText(Application.StartupPath + "//report_results.graphql");
                        query = @"query ReportQuery($ReportNumber: String!) {
                                                    getReport(report_number: $ReportNumber){
                                                        report_number
                                                        report_date
                                                        report_type
                                                        report_type_code
                                                        report_date_iso
                                                        info_message
                                                        is_digital
                                                        is_sleeve
                                                        links
                                                            {
                                                              pdf
                                                              plotting_diagram
                                                              proportions_diagram
                                                              image
                                                              rough_image
                                                              rough_video
                                                              polished_image
                                                              polished_video
                                                              dtlp_image_filename
                                                              dttl_pdf_filename
                                                              dtl_pdf_filename
                                                              dtlp_pdf_filename
                                                              dtlp_image_filename              
                                                            }
                                                        results {
                                                          ... on DiamondGradingReportResults {
                                                            shape_and_cutting_style            
                                                            measurements                 
                                                            carat_weight
                                                            color_grade
                                                            color_origin
                                                            color_distribution
                                                            clarity_grade
                                                            cut_grade
                                                            polish
                                                            symmetry
                                                            fluorescence          
                                                            diamond_type
                                                            country_of_origin
                                                            country_of_origin_code
                                                            clarity_characteristics
                                                            inscriptions
                                                            report_comments    
                                                            duplicate_comments            
                                                            key_to_symbols{
                                                              order
                                                              location
                                                              characteristic
                                                              image                          
                                                            }         
                                                            proportions {
                                                              table_pct
                                                              depth_pct
                                                              crown_angle
                                                              crown_height
                                                              pavilion_angle
                                                              pavilion_depth
                                                              star_length
                                                              lower_half
                                                              girdle
                                                              culet
                                                            }
                                                          }
                                                        }
                                                        quota {
                                                            remaining
                                                        }
                                                    }
                                                }";
                    }
                    catch (System.IO.FileNotFoundException ee)
                    {
                        Console.WriteLine(ee.Message);
                        System.Environment.Exit(1);
                    }
                    catch (Exception eE)
                    {
                        Console.WriteLine(eE.Message);
                        Console.WriteLine(eE.StackTrace);
                    }


                    // Construct the payload to be POSTed to the graphql server
                    var query_variables = new Dictionary<string, string> { { "ReportNumber", ReportNumber } };
                    var payload = new Dictionary<string, object> { { "query", query }, { "variables", query_variables } };

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var json = serializer.Serialize(payload);

                    using (var client = new WebClient())
                    {
                        client.Headers.Add(HttpRequestHeader.Authorization, ServiceProductKey);
                        client.Headers.Add(HttpRequestHeader.ContentType, "application/json");

                        string httpResponse = "";
                        try
                        {
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                            // Send the payload as a JSON to the endpoint
                            httpResponse = client.UploadString(StrUrl, json);
                            Rootobject account = JsonConvert.DeserializeObject<Rootobject>(httpResponse);

                            // Rootobject res =   System.Text.json. JsonSerializer.Deserialize<Rootobject>(httpResponse);
                            string certificate = account.data.getReport.links.pdf;
                            string plotting = account.data.getReport.links.plotting_diagram;
                            string DIAGRAM = account.data.getReport.links.proportions_diagram;

                            //string StrCertificateURL = GetValue(result, "pdf");
                            //    string StrPlotingURL = GetValue(result, "plotting_diagram");
                            using (WebClient client1 = new WebClient())
                            {
                                client1.DownloadFile(certificate, AppDomain.CurrentDomain.BaseDirectory + "CERTI\\" + ReportNumber + ".pdf");
                                if (plotting != null)
                                {
                                    client1.DownloadFile(plotting, AppDomain.CurrentDomain.BaseDirectory + "CERTI\\" + ReportNumber + "_PLO.jpg");
                                }
                                client1.DownloadFile(DIAGRAM, AppDomain.CurrentDomain.BaseDirectory + "CERTI\\" + ReportNumber + "_DIA.jpg");
                            }


                        }
                        catch (System.Net.WebException e)
                        {
                            Console.Write("Error accessing " + StrUrl + ": ");
                            Console.WriteLine(e.Message);
                            System.Environment.Exit(1);
                        }
                    }





                }



                private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
                {
                    try
                    {
                        string FILE_PATH = GET_IMAGE_PATH();
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {

                            string ID = dataGridView1.Rows[i].Cells["ID"].Value.ToString();


                            if (dataGridView1.Rows[i].Cells["VIDEO"].Value.ToString() == "")
                            {
                                if (Directory.Exists(GET_IMAGE_PATH() + ID) == true)
                                {
                                    S3Uploader a = new S3Uploader();
                                    dataGridView1.Rows[i].Cells["VIDEO"].Value = a.FILE_UPLOAD_RETURN_STATUS(dataGridView1.Rows[i].Cells["ID"].Value.ToString().Trim());
                                    dataGridView1.Rows[i].Cells["VIDEO"].Value = "DONE";
                                    UPDATE(ID, "https://MahantExport.s3.ap-south-1.amazonaws.com/Vision360.html?d=" + ID, "VIDEO");
                                }
                                else
                                {
                                    dataGridView1.Rows[i].Cells["VIDEO"].Value = "NO VIDEO";
                                    DELETE(ID, "https://MahantExport.s3.ap-south-1.amazonaws.com/Vision360.html?d=" + ID, "VIDEO");
                                }


                            }

                            if (dataGridView1.Rows[i].Cells["IMAGE"].Value.ToString() == "")
                            {
                                if (File.Exists(FILE_PATH + "\\" + ID + "\\still.JPG"))
                                {
                                    if (File.Exists(FILE_PATH + "\\" + ID + "\\" + ID + ".JPG"))
                                    { }
                                    else
                                    {
                                        System.IO.File.Copy(FILE_PATH + "\\" + ID + "\\still.JPG", FILE_PATH + "\\" + ID + "\\" + ID + ".JPG");
                                    }

                                    String S = FNC.FTP_UPLOAD(FILE_PATH + "\\" + ID + "\\" + ID + ".JPG", FTP + ID.ToString().Replace(FILE_PATH, "").Replace("\\", "") + ".JPG", USER, PASS);
                                    dataGridView1.Rows[i].Cells["IMAGE"].Value = "DONE";
                                    UPDATE(ID, "https://www.MahantExport.com/stone_image/" + ID + ".jpg", "IMAGE");

                                }
                                else
                                {
                                    DELETE(ID, "https://www.MahantExport.com/stone_image/" + ID + ".jpg", "IMAGE");
                                }
                            }

                            if (dataGridView1.Rows[i].Cells["THAMNIL"].Value.ToString() == "")
                            {
                                if (Directory.Exists(GET_IMAGE_PATH() + ID) == true)
                                {
                                    thumnil(FILE_PATH + "\\" + ID + "\\" + ID + ".JPG");
                                    dataGridView1.Rows[i].Cells["THAMNIL"].Value = "DONE";
                                    UPDATE(ID, "https://www.MahantExport.com/stone_image/" + ID + "TN.jpg", "TN_IMAGE");

                                }
                                else
                                {
                                    dataGridView1.Rows[i].Cells["THAMNIL"].Value = "NO THAMNIL";
                                    DELETE(ID, "https://www.MahantExport.com/stone_image/" + ID + "TN.jpg", "TN_IMAGE");
                                }


                            }

                            if (dataGridView1.Rows[i].Cells["LAB"].Value.ToString() == "GIA")
                            {

                                if (dataGridView1.Rows[i].Cells["CERTI"].Value.ToString() == "")
                                {
                                    if (dataGridView1.Rows[i].Cells["LOCATION"].Value.ToString().Trim() == "MUMBAI")
                                    {
                                        DownloadCertificateNewMethod(dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim(), "MUMBAI");
                                    }
                                    else if (dataGridView1.Rows[i].Cells["LOCATION"].Value.ToString().Trim() == "SURAT")
                                    {
                                        DownloadCertificateNewMethod(dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim(), "SURAT");
                                    }
                                    else
                                    {

                                    }


                                    DataTable DT1 = new DataTable();
                                    DT1 = DF.dataselect("select * from A_FTP where id='7' ");
                                    string FTP1 = DT1.Rows[0]["FTP_PATH"].ToString();
                                    string USER1 = DT1.Rows[0]["USERNAME"].ToString();
                                    string PASS1 = DT1.Rows[0]["PASSWORD"].ToString();

                                    dataGridView1.Rows[i].Cells["CERTI"].Value = FNC.FTP_UPLOAD(AppDomain.CurrentDomain.BaseDirectory + "CERTI\\" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + ".pdf", FTP1 + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString() + ".pdf", USER1, PASS1);
                                    UPDATE(ID, "https://www.MahantExport.com/certificate/" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + ".pdf", "CERTI");

                                    string StrCertPdfFile = AppDomain.CurrentDomain.BaseDirectory + "CERTI\\" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + "_PLO.JPG";
                                    if (File.Exists(StrCertPdfFile) == true)
                                    {
                                        dataGridView1.Rows[i].Cells["CERTIPLO"].Value = FNC.FTP_UPLOAD(AppDomain.CurrentDomain.BaseDirectory + "CERTI\\" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + "_PLO.jpg", FTP1 + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString() + "_plo.jpg", USER1, PASS1);
                                        UPDATE(ID, "https://www.MahantExport.com/certificate/" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + "_plo.jpg", "CERTIPLO");
                                    }

                                    dataGridView1.Rows[i].Cells["CERTIDIA"].Value = FNC.FTP_UPLOAD(AppDomain.CurrentDomain.BaseDirectory + "CERTI\\" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + "_DIA.jpg", FTP1 + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString() + "_dia.jpg", USER1, PASS1);
                                    UPDATE(ID, "https://www.MahantExport.com/certificate/" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + "_dia.jpg", "CERTIDIA");
                                }


                            }
                            else if (dataGridView1.Rows[i].Cells["LAB"].Value.ToString() == "IGI")
                            {

                                if (dataGridView1.Rows[i].Cells["CERTI"].Value.ToString() == "")
                                {
                                    using (WebClient client1 = new WebClient())
                                    {
                                        DataTable DT1 = new DataTable();
                                        DT1 = DF.dataselect("select * from A_FTP where id='7' ");
                                        string FTP1 = DT1.Rows[0]["FTP_PATH"].ToString();
                                        string USER1 = DT1.Rows[0]["USERNAME"].ToString();
                                        string PASS1 = DT1.Rows[0]["PASSWORD"].ToString();
                                        //client1.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                                        client1.Headers.Add("Accept: text/html, application/xhtml+xml, /");
                                        client1.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
                                        client1.Headers["content-type"] = "text/html; charset=utf-8";
                                        client1.Headers["PROSPEROUS"]= "true";

                                        //client1.UseDefaultCredentials = true;
                                        //client1.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                                        //+ "https://www.igi.org/viewpdf.php?r="
                                        //https://www.igi.org/API-IGI/viewpdf-url.php?r=
                                        client1.DownloadFile("https://www.igi.org/API-IGI/viewpdf-url.php?r=" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim(), AppDomain.CurrentDomain.BaseDirectory + "CERTI\\" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + ".pdf");
                                        //https://www.igi.org/verify-your-report/?r=
                                        dataGridView1.Rows[i].Cells["CERTI"].Value = "DONE";
                                        dataGridView1.Rows[i].Cells["CERTI"].Value = FNC.FTP_UPLOAD(AppDomain.CurrentDomain.BaseDirectory + "CERTI\\" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + ".pdf", FTP1 + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString() + ".pdf", USER1, PASS1);
                                        UPDATE(ID, "https://www.MahantExport.com/certificate/" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + ".pdf", "CERTI");

                                    }
                                }
                            }
                            else if (dataGridView1.Rows[i].Cells["LAB"].Value.ToString() == "HRD")
                            {
                                System.Diagnostics.Process.Start("https://my.hrdantwerp.com/Download/GetGradingReportPdf/?reportNumber=220000160738&printDocumentType=Cert");


                                if (dataGridView1.Rows[i].Cells["CERTI"].Value.ToString() == "")
                                {
                                    using (WebClient client1 = new WebClient())
                                    {
                                        DataTable DT1 = new DataTable();
                                        DT1 = DF.dataselect("select * from A_FTP where id='7' ");
                                        string FTP1 = DT1.Rows[0]["FTP_PATH"].ToString();
                                        string USER1 = DT1.Rows[0]["USERNAME"].ToString();
                                        string PASS1 = DT1.Rows[0]["PASSWORD"].ToString();
                                        // client1.DownloadFile("https://my.hrdantwerp.com/Download/GetGradingReportPdf/?reportNumber=" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + "&printDocumentType=Cert", AppDomain.CurrentDomain.BaseDirectory + "CERTI\\" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + ".pdf");

                                        //client1.DownloadFile("https://www.igi.org/viewpdf.php?r=" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim(), AppDomain.CurrentDomain.BaseDirectory + "CERTI\\" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + ".pdf");
                                        // dataGridView1.Rows[i].Cells["CERTI"].Value = "DONE";
                                        dataGridView1.Rows[i].Cells["CERTI"].Value = FNC.FTP_UPLOAD(AppDomain.CurrentDomain.BaseDirectory + "CERTI\\" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + ".pdf", FTP1 + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString() + ".pdf", USER1, PASS1);
                                        UPDATE(ID, "https://www.MahantExport.com/certificate/" + dataGridView1.Rows[i].Cells["REPORT_NO"].Value.ToString().Trim() + ".pdf", "CERTI");

                                    }
                                }

                            }
                        }

                    }
                    catch (Exception EX)
                    {
                        MessageBox.Show(EX.Message);

                    }
                }

                */
    }
}
