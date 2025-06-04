using MahantExport.GIADownload;
using BusLib.Configuration;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MahantExport.Class
{
    public class CertiStoneDetailDownload
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        DataTable TableCert = new DataTable();

        #region GIA/IGI/HRD Stone Detail Download
        public DataTable GetCertData(string LabColumn, string WeightColumn, string LabReportno)
        {
            try
            {
                if (LabColumn.ToUpper() == "GIA")
                {
                    ReportCheckWSClient RF1 = new ReportCheckWSClient();
                    DataTable dt1 = new BOTRN_MemoEntry().GetGIALinkForLabResultPDF(LabReportno, WeightColumn);

                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            string Url  = "";
                               
                            //Url = @"<?xml version=1.0 encoding=UTF8?><REPORT_CHECK_REQUEST><HEADER><IP_ADDRESS>192.168.0.250</IP_ADDRESS></HEADER><BODY><REPORT_DTLS><REPORT_DTL><REPORT_NO>6222397432</REPORT_NO><REPORT_WEIGHT>1.00</REPORT_WEIGHT></REPORT_DTL></REPORT_DTLS></BODY></REPORT_CHECK_REQUEST>";
                            Url = dt1.Rows[i][0].ToString();

                            string str = RF1.processRequest(Url);

                            TableCert = new DataTable();
                            GenerateDataTableForResult();

                            if (str.Trim().Length > 0)
                            {
                                if (str.Contains("SUCCESS"))
                                {
                                    XmlDocument xmldoc = new XmlDocument();
                                    xmldoc.LoadXml(str);

                                    DataRow Drow = TableCert.NewRow();

                                    Drow["JobNo"] = "";
                                    Drow["ControlNo"] = "";
                                    Drow["DiamondDossier"] = "";
                                    Drow["LabReport"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/REPORT_NO").InnerText;
                                    Drow["ReportDate"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/REPORT_DT").InnerText;
                                    Drow["ClientRef"] = "";
                                    Drow["MemoNo"] = "";
                                    Drow["Shape"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/SHAPE").InnerText;
                                    Drow["DiameterMin"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/LENGTH").InnerText;
                                    Drow["DiameterMax"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/WIDTH").InnerText;
                                    Drow["TotalDepth"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/DEPTH").InnerText.ToString().Replace("%", "");
                                    Drow["TotalDepthPer"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/DEPTH_PCT").InnerText.Replace("%", "") == "" ? "0" : xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/DEPTH_PCT").InnerText.Replace("%", "");
                                    Drow["WeightInCarats"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/WEIGHT").InnerText;
                                    Drow["Color"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/COLOR").InnerText.Replace("*", "");
                                    Drow["ColorDescription"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/COLOR_DESCRIPTIONS").InnerText;
                                    Drow["Clarity"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/CLARITY").InnerText;
                                    Drow["ClarityStatus"] = "";
                                    Drow["Cut"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/FINAL_CUT").InnerText;
                                    Drow["Polish"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/POLISH").InnerText;
                                    Drow["Symm"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/SYMMETRY").InnerText;
                                    Drow["Flr"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/FLUORESCENCE_INTENSITY").InnerText;
                                    Drow["FLRColor"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/FLUORESCENCE_COLOR").InnerText;
                                    Drow["GirdleName"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/GIRDLE").InnerText;
                                    Drow["GirdleCondition"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/GIRDLE_CONDITION").InnerText;
                                    Drow["CuletCondition"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/CULET_SIZE").InnerText.ToString().Replace("%", "");
                                    Drow["TableDiameterPer"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/TABLE_PCT").InnerText.ToString().Replace("%", "");
                                    Drow["CrownAngle"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/CRN_AG").InnerText.ToString().Replace("°", "");
                                    Drow["CrownHeight"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/CRN_HT").InnerText.ToString().Replace("%", "");
                                    Drow["PavillionAngle"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/PAV_AG").InnerText.ToString().Replace("°", "");
                                    Drow["PavillionHeight"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/PAV_DP").InnerText.ToString().Replace("%", "");
                                    Drow["Star"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/STR_LN").InnerText.ToString().Replace("°", "").Replace("%", "");
                                    Drow["LH"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/LR_HALF").InnerText.ToString().Replace("°", "");
                                    Drow["Painting"] = "";
                                    Drow["Proportion"] = "";
                                    Drow["PaintComm"] = "";
                                    Drow["KeyToSymbols"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/KEY_TO_SYMBOLS").InnerText;
                                    Drow["ReportComment"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/REPORT_COMMENTS").InnerText;
                                    Drow["LaserInscription"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/INSCRIPTION").InnerText;
                                    Drow["SyntheticIndicator"] = "";
                                    Drow["GirdlePer"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/GIRDLE_PCT").InnerText.ToString().Replace("%", "");
                                    Drow["PolishFeatures"] = "";
                                    Drow["SymmetryFeatures"] = "";
                                    Drow["ShapeDescription"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/FULL_SHAPE_DESCRIPTION").InnerText;
                                    Drow["ReportType"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/REPORT_TYPE").InnerText;
                                    Drow["Sorting"] = "";
                                    Drow["BasketStatus"] = "";
                                    Drow["Department"] = "";


                                    TableCert.Rows.Add(Drow);
                                    return TableCert;
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString(), "GetCertData", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return TableCert;
        }

        public void GenerateDataTableForResult()
        {
            TableCert.Columns.Add("JobNo");
            TableCert.Columns.Add("ControlNo");
            TableCert.Columns.Add("DiamondDossier");
            TableCert.Columns.Add("LabReport");
            TableCert.Columns.Add("ReportDate");
            TableCert.Columns.Add("ClientRef");
            TableCert.Columns.Add("MemoNo");
            TableCert.Columns.Add("Shape");
            TableCert.Columns.Add("DiameterMin");
            TableCert.Columns.Add("DiameterMax");
            TableCert.Columns.Add("TotalDepth");
            TableCert.Columns.Add("WeightInCarats");
            TableCert.Columns.Add("Color");
            TableCert.Columns.Add("ColorDescription");
            TableCert.Columns.Add("Clarity");
            TableCert.Columns.Add("ClarityStatus");
            TableCert.Columns.Add("Cut");
            TableCert.Columns.Add("Polish");
            TableCert.Columns.Add("Symm");
            TableCert.Columns.Add("Flr");
            TableCert.Columns.Add("FLRColor");
            TableCert.Columns.Add("GirdleName");
            TableCert.Columns.Add("GirdleCondition");
            TableCert.Columns.Add("CuletCondition");
            TableCert.Columns.Add("TotalDepthPer");
            TableCert.Columns.Add("TableDiameterPer");
            TableCert.Columns.Add("CrownAngle");
            TableCert.Columns.Add("CrownHeight");
            TableCert.Columns.Add("PavillionAngle");
            TableCert.Columns.Add("PavillionHeight");
            TableCert.Columns.Add("Star");
            TableCert.Columns.Add("LH");
            TableCert.Columns.Add("Painting");
            TableCert.Columns.Add("Proportion");
            TableCert.Columns.Add("PaintComm");
            TableCert.Columns.Add("KeyToSymbols");
            TableCert.Columns.Add("ReportComment");
            TableCert.Columns.Add("SyntheticIndicator");
            TableCert.Columns.Add("GirdlePer");
            TableCert.Columns.Add("PolishFeatures");
            TableCert.Columns.Add("SymmetryFeatures");
            TableCert.Columns.Add("ShapeDescription");
            TableCert.Columns.Add("ReportType");
            TableCert.Columns.Add("Sorting");
            TableCert.Columns.Add("BasketStatus");
            TableCert.Columns.Add("CostRaprate");
            TableCert.Columns.Add("Websiterate");
            TableCert.Columns.Add("Websiteamount");
            TableCert.Columns.Add("CostRate");
            TableCert.Columns.Add("CostAmount");
            TableCert.Columns.Add("Department");
            TableCert.Columns.Add("LaserInscription");
        }
        #endregion

        #region Registration Approval



        #endregion





    }
}
