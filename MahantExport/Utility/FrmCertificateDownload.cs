using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Reflection;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using GhostscriptSharp;
using BusLib.Configuration;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace MahantExport.Utility
{
    public partial class FrmCertificateDownload : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();

        DataTable DTabResult = new DataTable();
        DataTable reportTable = new DataTable();

        private bool isPasteAction = false;
        private const Keys PasteKeys = Keys.Control | Keys.V;

        public FrmCertificateDownload()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();
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
       
        private void BtnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    progressPanel1.Visible = true;
                    CreateDatatable();
                    DataTable DTab = reportTable.Clone();

                    string[] valuesArray = txtStoneNo.Text.Split(',');

                    foreach (string Report in valuesArray)
                    {
                        if (Report != "")
                        {
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

                            string StrUrl = "https://api.reportresults.gia.edu/";
                            string ReportNumber = Report;

                            string ServiceProductKey = "55311290-c723-4601-b8af-cfdba3d7e167";

                            var query_variables = new Dictionary<string, string> {
                            { "ReportNumber", ReportNumber}
                            };
                            var payload = new Dictionary<string, object> {
                            { "query", query },
                            { "variables", query_variables }
                            };

                            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var json = serializer.Serialize(payload);

                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(StrUrl);
                            httpWebRequest.ContentType = "application/json; charset=utf-8";
                            httpWebRequest.AllowAutoRedirect = true;
                            httpWebRequest.Headers.Add(HttpRequestHeader.Authorization, ServiceProductKey);
                            httpWebRequest.Method = "POST";
                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                streamWriter.Write(json);
                                streamWriter.Flush();
                            }
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            string result = "";
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                result = streamReader.ReadToEnd();
                            }

                            //DataRow DRow = ConvertXmlStringToDataTable(result);
                            var json1 = JObject.Parse(result);


                            // Extract data from JSON
                            var reportData = json1["data"]["getReport"];
                            var getReport = json1["data"]?["getReport"] as JObject;
                            if (getReport != null && getReport.Count > 0)
                            {
                                var row = DTab.NewRow();

                                // Safely access top-level properties
                                row["Report Number"] = reportData["report_number"]?.ToString() ?? "N/A";
                                row["Report Date"] = reportData["report_date"]?.ToString() ?? "N/A";
                                row["Report Type"] = reportData["report_type"]?.ToString() ?? "N/A";

                                var results = reportData["results"];
                                if (results != null)
                                {
                                    row["Carat Weight"] = results["carat_weight"]?.ToString() ?? "N/A";
                                    row["Shape and Cutting Style"] = results["shape_and_cutting_style"]?.ToString() ?? "N/A";
                                    row["Measurements"] = results["measurements"]?.ToString() ?? "N/A";
                                    row["Color Grade"] = results["color_grade"]?.ToString() ?? "N/A";
                                    row["Clarity Grade"] = results["clarity_grade"]?.ToString() ?? "N/A";
                                    row["Cut Grade"] = results["cut_grade"]?.ToString() ?? "N/A";
                                    row["Polish"] = results["polish"]?.ToString() ?? "N/A";
                                    row["Symmetry"] = results["symmetry"]?.ToString() ?? "N/A";
                                    row["Fluorescence"] = results["fluorescence"]?.ToString() ?? "N/A";
                                    row["Inscriptions"] = results["inscriptions"]?.ToString() ?? "N/A";

                                    // Safely access nested "proportions" object
                                    var proportions = results["proportions"];
                                    if (proportions != null)
                                    {
                                        row["Table Percentage"] = proportions["table_pct"]?.ToString() ?? "N/A";
                                        row["Depth Percentage"] = proportions["depth_pct"]?.ToString() ?? "N/A";
                                        row["Crown Angle"] = proportions["crown_angle"]?.ToString() ?? "N/A";
                                        row["Crown Height"] = proportions["crown_height"]?.ToString() ?? "N/A";
                                        row["Pavilion Angle"] = proportions["pavilion_angle"]?.ToString() ?? "N/A";
                                        row["Pavilion Depth"] = proportions["pavilion_depth"]?.ToString() ?? "N/A";
                                        row["Star Length"] = proportions["star_length"]?.ToString() ?? "N/A";
                                        row["Lower Half"] = proportions["lower_half"]?.ToString() ?? "N/A";
                                        row["Girdle"] = proportions["girdle"]?.ToString() ?? "N/A";
                                        row["Culet"] = proportions["culet"]?.ToString() ?? "N/A";
                                    }
                                }

                                // Add the row to the DataTable
                                DTab.Rows.Add(row);
                                // Add a new row with all the values
                                //var row = DTab.NewRow();
                                //row["Report Number"] = reportData["report_number"].ToString();
                                //row["Report Date"] = reportData["report_date"].ToString();
                                //row["Report Type"] = reportData["report_type"].ToString();
                                //row["Carat Weight"] = reportData["results"]["carat_weight"].ToString();
                                //row["Shape and Cutting Style"] = reportData["results"]["shape_and_cutting_style"].ToString();
                                //row["Measurements"] = reportData["results"]["measurements"].ToString();
                                //row["Color Grade"] = reportData["results"]["color_grade"].ToString();
                                //row["Clarity Grade"] = reportData["results"]["clarity_grade"].ToString();
                                //row["Cut Grade"] = reportData["results"]["cut_grade"].ToString();
                                //row["Polish"] = reportData["results"]["polish"].ToString();
                                //row["Symmetry"] = reportData["results"]["symmetry"].ToString();
                                //row["Fluorescence"] = reportData["results"]["fluorescence"].ToString();
                                //row["Inscriptions"] = reportData["results"]["inscriptions"].ToString();

                                //// Fill proportions data
                                //var proportions = reportData["results"]["proportions"];
                                //row["Table Percentage"] = proportions["table_pct"].ToString();
                                //row["Depth Percentage"] = proportions["depth_pct"].ToString();
                                //row["Crown Angle"] = proportions["crown_angle"].ToString();
                                //row["Crown Height"] = proportions["crown_height"].ToString();
                                //row["Pavilion Angle"] = proportions["pavilion_angle"].ToString();
                                //row["Pavilion Depth"] = proportions["pavilion_depth"].ToString();
                                //row["Star Length"] = proportions["star_length"].ToString();
                                //row["Lower Half"] = proportions["lower_half"].ToString();
                                //row["Girdle"] = proportions["girdle"].ToString();
                                //row["Culet"] = proportions["culet"].ToString();
                                //DTab.Rows.Add(row);
                            }
                        }
                    }
                    MainGrd.DataSource = DTab;
                    GrdDet.BestFitColumns();

                    progressPanel1.Visible = false;

                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public DataTable CreateDatatable()
        {
            // Create DataTable to store report details row-wise
            reportTable = new DataTable();

            // Add columns to DataTable
            reportTable.Columns.Add("Report Number", typeof(string));
            reportTable.Columns.Add("Report Date", typeof(string));
            reportTable.Columns.Add("Report Type", typeof(string));
            reportTable.Columns.Add("Carat Weight", typeof(string));
            reportTable.Columns.Add("Shape and Cutting Style", typeof(string));
            reportTable.Columns.Add("Measurements", typeof(string));
            reportTable.Columns.Add("Color Grade", typeof(string));
            reportTable.Columns.Add("Clarity Grade", typeof(string));
            reportTable.Columns.Add("Cut Grade", typeof(string));
            reportTable.Columns.Add("Polish", typeof(string));
            reportTable.Columns.Add("Symmetry", typeof(string));
            reportTable.Columns.Add("Fluorescence", typeof(string));
            reportTable.Columns.Add("Inscriptions", typeof(string));

            // Add columns for proportions
            reportTable.Columns.Add("Table Percentage", typeof(string));
            reportTable.Columns.Add("Depth Percentage", typeof(string));
            reportTable.Columns.Add("Crown Angle", typeof(string));
            reportTable.Columns.Add("Crown Height", typeof(string));
            reportTable.Columns.Add("Pavilion Angle", typeof(string));
            reportTable.Columns.Add("Pavilion Depth", typeof(string));
            reportTable.Columns.Add("Star Length", typeof(string));
            reportTable.Columns.Add("Lower Half", typeof(string));
            reportTable.Columns.Add("Girdle", typeof(string));
            reportTable.Columns.Add("Culet", typeof(string));

            return reportTable;

        }
        public DataRow ConvertXmlStringToDataTable(string response)
        {
            var json = JObject.Parse(response);

           
            // Extract data from JSON
            var reportData = json["data"]["getReport"];
           
            // Add a new row with all the values
            var row = reportTable.NewRow();
            row["Report Number"] = reportData["report_number"].ToString();
            row["Report Date"] = reportData["report_date"].ToString();
            row["Report Type"] = reportData["report_type"].ToString();
            row["Carat Weight"] = reportData["results"]["carat_weight"].ToString();
            row["Shape and Cutting Style"] = reportData["results"]["shape_and_cutting_style"].ToString();
            row["Measurements"] = reportData["results"]["measurements"].ToString();
            row["Color Grade"] = reportData["results"]["color_grade"].ToString();
            row["Clarity Grade"] = reportData["results"]["clarity_grade"].ToString();
            row["Cut Grade"] = reportData["results"]["cut_grade"].ToString();
            row["Polish"] = reportData["results"]["polish"].ToString();
            row["Symmetry"] = reportData["results"]["symmetry"].ToString();
            row["Fluorescence"] = reportData["results"]["fluorescence"].ToString();
            row["Inscriptions"] = reportData["results"]["inscriptions"].ToString();

            // Fill proportions data
            var proportions = reportData["results"]["proportions"];
            row["Table Percentage"] = proportions["table_pct"].ToString();
            row["Depth Percentage"] = proportions["depth_pct"].ToString();
            row["Crown Angle"] = proportions["crown_angle"].ToString();
            row["Crown Height"] = proportions["crown_height"].ToString();
            row["Pavilion Angle"] = proportions["pavilion_angle"].ToString();
            row["Pavilion Depth"] = proportions["pavilion_depth"].ToString();
            row["Star Length"] = proportions["star_length"].ToString();
            row["Lower Half"] = proportions["lower_half"].ToString();
            row["Girdle"] = proportions["girdle"].ToString();
            row["Culet"] = proportions["culet"].ToString();
           
            return row;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
          
        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {           
            progressPanel1.Visible = false;
        }

        private void txtStoneNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
               
                    String str1 = Val.ToString(txtStoneNo.Text);
                    string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                    if (result.EndsWith(",,"))
                    {
                        result = result.Remove(result.Length - 1);
                    }
                    txtStoneNo.Text = result;
                    txtStoneNo.Select(txtStoneNo.Text.Length, 0);
                if (isPasteAction)
                {
                    isPasteAction = false;
                    txtStoneNo.Select(txtStoneNo.Text.Length, 0);
                }
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void txtStoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                isPasteAction = true;
            }
        }
    }
}