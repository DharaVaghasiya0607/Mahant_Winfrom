using BusLib.Master;
using BusLib.Transaction;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using OfficeOpenXml;
using MahantExport.GIADownload;
using MahantExport.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace MahantExport.Report
{
    public partial class FrmGIAAPIIntegration : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        DataTable DTabResult = new DataTable();
        BOTRN_SingleFileUpload ObjUpload = new BOTRN_SingleFileUpload();
        DataTable DtabPara = new DataTable();
        BODevGridSelection ObjGridSelection;
        BODevGridSelection ObjGridSelection1;


        DataTable DtabFinal = new DataTable();
        DataTable DtabFileUpload = new DataTable();
        DataSet DSet = new DataSet();
        Guid mUpload_ID = Guid.Empty;
        Guid mGroup_ID = Guid.Empty;
        int IntCheck = 0;
        string StrMessage = "";
        public FrmGIAAPIIntegration()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            mUpload_ID = Guid.Empty;
            DtabPara = new BOMST_Parameter().GetParameterData();
           
            DtabFileUpload = ObjUpload.GetFileUploadData(mUpload_ID, "", "", "", "", "", "");
            GrdDetail.BeginUpdate();
            if (MainGrid.RepositoryItems.Count == 0)
            {
                ObjGridSelection1 = new BODevGridSelection();
                ObjGridSelection1.View = GrdDetail;
                ObjGridSelection1.ClearSelection();
                ObjGridSelection1.CheckMarkColumn.VisibleIndex = 0;
            }
            else
            {
                ObjGridSelection1.ClearSelection();
            }
            GrdDetail.Columns["COLSELECTCHECKBOX"].OwnerBand = BandGeneral;
            GrdDetail.Columns["COLSELECTCHECKBOX"].VisibleIndex = 0;
            GrdDetail.EndUpdate();
            this.Show();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = false;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        public static async Task<string> SendSoapRequest(string url, string xmlPayload, string StrMethod)
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(xmlPayload, Encoding.UTF8, "text/xml");
                client.DefaultRequestHeaders.Add("SOAPAction", StrMethod); // Add the correct SOAPAction

                HttpResponseMessage response = await client.PostAsync(url, content);

                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }

        public DataTable ConvertXmlStringToDataTable(string response)
        {
            DTabResult = new DataTable();

            DTabResult.Columns.Add("JOBNO", typeof(string));
            DTabResult.Columns.Add("CONTROLNO", typeof(string));
            DTabResult.Columns.Add("DIAMONDDOSSIER", typeof(string));
            DTabResult.Columns.Add("REPORTNO", typeof(string));
            DTabResult.Columns.Add("REPORTDATE", typeof(string));
            DTabResult.Columns.Add("CLIENTREFNO", typeof(string));
            DTabResult.Columns.Add("MEMONO", typeof(string));
            DTabResult.Columns.Add("SHAPE", typeof(string));
            DTabResult.Columns.Add("LENGTH", typeof(decimal));
            DTabResult.Columns.Add("WIDTH", typeof(decimal));
            DTabResult.Columns.Add("DEPTH", typeof(decimal));
            DTabResult.Columns.Add("CARAT", typeof(decimal));
            DTabResult.Columns.Add("COLOR", typeof(string));
            DTabResult.Columns.Add("COLORDESC", typeof(string));
            DTabResult.Columns.Add("CLARITY", typeof(string));
            DTabResult.Columns.Add("CLARITYSTATUS", typeof(string));
            DTabResult.Columns.Add("CUT", typeof(string));
            DTabResult.Columns.Add("POLISH", typeof(string));
            DTabResult.Columns.Add("SYMMETRY", typeof(string));
            DTabResult.Columns.Add("FLUORESCENCE", typeof(string));
            DTabResult.Columns.Add("FLUORESCENCECOLOR", typeof(string));
            DTabResult.Columns.Add("GIRDLEDESC", typeof(string));
            DTabResult.Columns.Add("GIRDLECONDITION", typeof(string));
            DTabResult.Columns.Add("CULETSIZE", typeof(string));
            DTabResult.Columns.Add("DEPTHPER", typeof(decimal));
            DTabResult.Columns.Add("TABLE1", typeof(decimal));
            DTabResult.Columns.Add("CRANGLE", typeof(string));
            DTabResult.Columns.Add("CRHEIGHT", typeof(string));
            DTabResult.Columns.Add("PAVANGLE", typeof(string));
            DTabResult.Columns.Add("PAVHEIGHT", typeof(string));
            DTabResult.Columns.Add("STARLENGTH", typeof(string));
            DTabResult.Columns.Add("LOWERHALF", typeof(string));
            DTabResult.Columns.Add("PAINTING", typeof(string));
            DTabResult.Columns.Add("PROPORTIONS", typeof(string));
            DTabResult.Columns.Add("PAINTCOMM", typeof(string));
            DTabResult.Columns.Add("KEYTOSYMBOL", typeof(string));
            DTabResult.Columns.Add("REPORTCOMMENT", typeof(string));
            DTabResult.Columns.Add("INSCRIPTION", typeof(string));
            DTabResult.Columns.Add("SYNTHETICINDICATOR", typeof(string));
            DTabResult.Columns.Add("GIRDLEPER", typeof(string));
            DTabResult.Columns.Add("POLISHFEATURES", typeof(string));
            DTabResult.Columns.Add("SYMMETRYFEATURES", typeof(string));
            DTabResult.Columns.Add("SHAPEDESC", typeof(string));
            DTabResult.Columns.Add("REPORTTYPE", typeof(string));
            ////DTabResult.Columns.Add("Sorting", typeof(string));
            //DTabResult.Columns.Add("Basket Status", typeof(string));
            //DTabResult.Columns.Add("Country of origin", typeof(string));
            //DTabResult.Columns.Add("Diamond Type", typeof(string));
            //DTabResult.Columns.Add("Disclosed Source", typeof(string));
            //DTabResult.Columns.Add("AGSI", typeof(string));
            //DTabResult.Columns.Add("Provenance Status", typeof(string));
            //DTabResult.Columns.Add("Tracr ID", typeof(string));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);

            string innerXml = doc.SelectSingleNode("//return").InnerText;
            innerXml = System.Net.WebUtility.HtmlDecode(innerXml);

            XmlDocument innerDoc = new XmlDocument();
            innerDoc.LoadXml(innerXml);


            XmlNodeList results = innerDoc.SelectNodes("//RESULT");
            foreach (XmlNode result in results)
            {
                DataRow row = DTabResult.NewRow();

                row["JOBNO"] = result["JOB_NO"].InnerText;
                row["CONTROLNO"] = result["CONTROL_NO"].InnerText;
                row["DIAMONDDOSSIER"] = result["DIAMOND_DOSSIER"].InnerText;
                row["REPORTNO"] = result["REPORT_NO"].InnerText;
                row["REPORTDATE"] = result["REPORT_DT"].InnerText;
                row["CLIENTREFNO"] = result["CLIENT_REF"].InnerText;
                row["MEMONO"] = result["MEMO_NO"].InnerText;
                row["SHAPE"] = result["SHAPE"].InnerText;
                row["LENGTH"] = decimal.Parse(result["LENGTH"].InnerText);
                row["WIDTH"] = decimal.Parse(result["WIDTH"].InnerText);
                row["DEPTH"] = decimal.Parse(result["DEPTH"].InnerText);
                row["CARAT"] = decimal.Parse(result["WEIGHT"].InnerText);
                row["COLOR"] = result["COLOR"].InnerText;
                row["COLORDESC"] = result["COLOR_DESCRIPTIONS"].InnerText;
                row["CLARITY"] = result["CLARITY"].InnerText;
                row["CLARITYSTATUS"] = result["CLARITY_STATUS"].InnerText;
                row["CUT"] = result["FINAL_CUT"].InnerText;
                row["POLISH"] = result["POLISH"].InnerText;
                row["SYMMETRY"] = result["SYMMETRY"].InnerText;
                row["FLUORESCENCE"] = result["FLUORESCENCE_INTENSITY"].InnerText;
                row["FLUORESCENCECOLOR"] = result["FLUORESCENCE_COLOR"].InnerText;
                row["GIRDLEDESC"] = result["GIRDLE"].InnerText;
                row["GIRDLECONDITION"] = result["GIRDLE_CONDITION"].InnerText;
                row["CULETSIZE"] = result["CULET_SIZE"].InnerText;
                row["DEPTHPER"] = decimal.Parse(result["DEPTH_PCT"].InnerText);
                row["TABLE1"] = decimal.Parse(result["TABLE_PCT"].InnerText);
                row["CRANGLE"] = result["CRN_AG"].InnerText;
                row["CRHEIGHT"] = result["CRN_HT"].InnerText;
                row["PAVANGLE"] = result["PAV_AG"].InnerText;
                row["PAVHEIGHT"] = result["PAV_DP"].InnerText;
                row["STARLENGTH"] = result["STR_LN"].InnerText;
                row["LOWERHALF"] = result["LR_HALF"].InnerText;
                row["PAINTING"] = result["PAINTING"].InnerText;
                row["PROPORTIONS"] = result["PROPORTION"].InnerText;
                row["PAINTCOMM"] = result["PAINT_COMM"].InnerText;
                row["KEYTOSYMBOL"] = result["KEY_TO_SYMBOLS"].InnerText;
                row["REPORTCOMMENT"] = result["REPORT_COMMENTS"].InnerText;
                row["INSCRIPTION"] = result["INSCRIPTION"].InnerText;
                row["SYNTHETICINDICATOR"] = result["SYNTHETIC_INDICATOR"].InnerText;
                row["GIRDLEPER"] = result["GIRDLE_PCT"].InnerText;
                row["POLISHFEATURES"] = result["POLISH_FEATURES"].InnerText;
                row["SYMMETRYFEATURES"] = result["SYMMETRY_FEATURES"].InnerText;
                row["SHAPEDESC"] = result["SHAPE_DESC"].InnerText;
                row["REPORTTYPE"] = result["REPORT_TYPE"].InnerText;

                DTabResult.Rows.Add(row);
            }

            return DTabResult;
        }
        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                progressPanel1.Visible = true;                
                string StrGIAUserName = ObjUpload.GetGIAUsername();
                string StrGIAClientID = ObjUpload.GetGIAClientID();
                int StrGIASite_ID = Val.ToInt32(ObjUpload.GetGIASiteID());

                string url = "";
                url = "https://labws.gia.edu/ConsolidatedWS/ConsolidatedWS?WSDL";

                string xmlPayload = @"<env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/' xmlns:q0='http://service.consws.model.ngs.com/'>
                                    <env:Header />
                                    <env:Body>
                                        <q0:getPrimaryResults1>
                                            <arg0><![CDATA[<?xml version='1.0' encoding='UTF-8'?>
                                                <PRIMARY_RESULT_REQUEST>
                                                    <HEADER>
                                                        <USER_NAME>" + StrGIAUserName + @"</USER_NAME>
                                                        <CLIENT_ID>" + StrGIAClientID + @"</CLIENT_ID>
                                                    </HEADER>
                                                    <BODY>
                                                        <SITE_ID>" + StrGIASite_ID + @"</SITE_ID>
                                                    </BODY>
                                                </PRIMARY_RESULT_REQUEST>]]>
                                            </arg0>
                                        </q0:getPrimaryResults1>
                                    </env:Body>
                                </env:Envelope>";
                string response = await SendSoapRequest(url, xmlPayload, "getPrimaryResults1");

                DataTable DTab = ConvertXmlStringToDataTable(response);
               
                MainGrd.DataSource = DTab;
                GrdDet.Columns["JOBNO"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                GrdDet.Columns["JOBNO"].SummaryItem.DisplayFormat = "{0:N0}";

                if (MainGrd.RepositoryItems.Count == 0)
                {
                    ObjGridSelection = new BODevGridSelection();
                    ObjGridSelection.View = GrdDet;
                    ObjGridSelection.ISBoolApplicableForPageConcept = true;
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                    GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
                }
                else
                {
                    if (ObjGridSelection != null)
                        ObjGridSelection.ClearSelection();
                }
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }
                //if (MainGrd.RepositoryItems.Count == 0)
                //{
                //    ObjGridSelection = new BODevGridSelection();
                //    ObjGridSelection.View = GrdDet;
                //    ObjGridSelection.ClearSelection();
                //    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                //}
                //else
                //{
                //    ObjGridSelection.ClearSelection();
                //}
                GrdDet.Columns["COLSELECTCHECKBOX"].VisibleIndex = 0;
                GrdDet.BestFitColumns();
                progressPanel1.Visible = false;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("Lab Result", GrdDet);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
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


        private void BtnSaveResult_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTabSelected = GetTableOfSelectedRows(GrdDet, true);

                if (DTabSelected == null ||DTabSelected.Rows.Count == 0)
                {
                    Global.Message("Please Select Atleast One Record For Result Upload");
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                int IntSrNo = 1;
                mGroup_ID = Guid.NewGuid();

                int IntCount = 0;

                DtabFinal = DtabFileUpload.Clone();


                if (!DTabSelected.Columns.Contains("Status"))
                {
                    DTabSelected.Columns.Add("Status", typeof(string));
                }
                string StrResultStatus = "RESULT";

                foreach (DataRow Dr in DTabSelected.Rows)
                {
                    string Str = "";
                    string PacketNo = "";
                    string StrPcketTag = "";
                    string StrPartyStokNo = "";
                    DataRow DrFinal = DtabFinal.NewRow();
                    DrFinal["UPLOAD_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                    DrFinal["STOCK_ID"] = Guid.Empty;
                    DrFinal["GROUP_ID"] = mGroup_ID;

                    DrFinal["UPLOADDATE"] = DateTime.Now.ToShortDateString();
                    DrFinal["UPLOADTYPE"] = "GIA";

                    if (Val.ToString(Dr["CLIENTREFNO"]).Trim().Equals(string.Empty))
                        continue;

                    if (Val.ToString(Dr["CLIENTREFNO"]).Contains("/"))
                    {

                        DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]).Substring(0, Val.ToString(Dr["CLIENTREFNO"]).IndexOf("/")).Trim();
                        Str = Dr["CLIENTREFNO"].ToString().Substring(Val.ToString(Dr["CLIENTREFNO"]).LastIndexOf("/") + 1);
                        PacketNo = Regex.Replace(Val.ToString(Str), "[^0-9]+", string.Empty).Replace("-", "");
                    }
                    else if (Val.ToString(Dr["CLIENTREFNO"]).Contains("-"))
                    {
                        DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]).Substring(0, Val.ToString(Dr["CLIENTREFNO"]).IndexOf("-")).Trim();
                        Str = Dr["CLIENTREFNO"].ToString().Substring(Val.ToString(Dr["CLIENTREFNO"]).LastIndexOf("-") + 1);
                        PacketNo = Regex.Replace(Val.ToString(Str), "[^0-9]+", string.Empty);
                    }
                    else
                    {
                        DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]);
                        DrFinal["PACKETNO"] = "";
                    }
                    StrPcketTag = Regex.Replace(Str.ToString(), @"[\d-]", "");

                    IntCheck = 0;
                    StrMessage = "";
                    string StrStoneNo = Val.ToString(Dr["CLIENTREFNO"]);



                    DrFinal["STATUS"] = Dr["STATUS"];

                    DrFinal["PACKETNO"] = Val.ToString(PacketNo);
                    DrFinal["TAG"] = Val.ToString(StrPcketTag);

                    DrFinal["GRDFLAG"] = 0;
                    DrFinal["TYPEFLAG"] = 0;
                    DrFinal["UPLOADFILENAME"] = "GIA API RESULT DOWNLOAD";
                    DrFinal["SRNO"] = IntSrNo;
                    DrFinal["JOBNO"] = Dr["JOBNO"];
                    DrFinal["CONTROLNO"] = Val.ToString(Dr["CONTROLNO"]);
                    DrFinal["DIAMONDDOSSIER"] = Dr["DIAMONDDOSSIER"];
                    DrFinal["REPORTNO"] = Dr["REPORTNO"];
                    DrFinal["REPORTDATE"] = Val.ToString(Dr["REPORTDATE"]);

                    DrFinal["CLIENTREFNO"] = Dr["CLIENTREFNO"];
                    DrFinal["MEMONO"] = Dr["MEMONO"];

                    DrFinal["SHAPE"] = Dr["SHAPE"];
                    DrFinal["LENGTH"] = Dr["LENGTH"];
                    DrFinal["WIDTH"] = Dr["WIDTH"];
                    DrFinal["DEPTH"] = Dr["DEPTH"];
                    DrFinal["CARAT"] = Dr["CARAT"];
                    DrFinal["COLOR"] = Dr["COLOR"];

                    if (Val.ToString(Dr["COLOR"]) == "*")
                    {
                        string StrColorDesc = Val.ToString(Dr["COLORDESC"]);
                        if (StrColorDesc.Contains("[*]"))
                        {
                            StrColorDesc = StrColorDesc.Remove(0, 4);
                            if (StrColorDesc.ToUpper().Contains("NATURAL"))
                            {
                                StrColorDesc = Regex.Replace(StrColorDesc.ToUpper(), "NATURAL", string.Empty);
                                StrColorDesc = StrColorDesc.Remove(StrColorDesc.IndexOf(","), 2);
                                StrColorDesc = "NATURAL" + StrColorDesc;
                            }
                            if (StrColorDesc.ToUpper().Contains("UNDETERMINED"))
                            {
                                StrColorDesc = Regex.Replace(StrColorDesc.ToUpper(), "UNDETERMINED", string.Empty);
                                StrColorDesc = StrColorDesc.Remove(StrColorDesc.IndexOf(","), 2);
                                StrColorDesc = "UNDETERMINED" + StrColorDesc;
                            }
                            DrFinal["COLORDESC"] = StrColorDesc;

                            DtabPara = new BOMST_Parameter().GetFancyColorData();

                            int IntCnt = 0;
                            bool exitLoop = false;
                            foreach (DataRow DRow in DtabPara.Rows)
                            {
                                string[] strSplit = Val.ToString(DRow["REMARK"]).ToUpper().Split(',');
                                foreach (string STR in strSplit)
                                {
                                    if (STR.Contains(StrColorDesc))
                                    {
                                        IntCnt = 1;
                                        exitLoop = true;
                                        break;
                                    }
                                }
                                if (exitLoop)
                                {
                                    break;
                                }
                            }

                            if (IntCnt == 0)
                            {
                                Global.Message("Stone No : " + Dr["CLIENTREFNO"] + " ->  Fancy color : [ " + StrColorDesc + " ]  Not Exist in Master Table..");
                                this.Cursor = Cursors.Default;
                                return;
                            }
                        }
                    }
                    else
                    {
                        DrFinal["COLORDESC"] = Dr["COLORDESC"];
                    }

                    DrFinal["CLARITY"] = Dr["CLARITY"];
                    DrFinal["CLARITYSTATUS"] = Dr["CLARITYSTATUS"];
                    DrFinal["CUT"] = Dr["CUT"];
                    DrFinal["POLISH"] = Dr["POLISH"];
                    DrFinal["SYMMETRY"] = Dr["SYMMETRY"];
                    DrFinal["FLUORESCENCE"] = Dr["FLUORESCENCE"];
                    DrFinal["FLUORESCENCECOLOR"] = Dr["FLUORESCENCECOLOR"];

                    DrFinal["GIRDLEDESC"] = Dr["GIRDLEDESC"];
                    DrFinal["GIRDLECONDITION"] = Dr["GIRDLECONDITION"];

                    DrFinal["CULETSIZE"] = Dr["CULETSIZE"];
                    DrFinal["DEPTHPER"] = Dr["DEPTHPER"];
                    DrFinal["TABLE1"] = Dr["TABLE1"];


                    DrFinal["CRANGLE"] = Regex.Replace(Val.ToString(Dr["CRANGLE"]), @"[^0-9\.]+", "");
                    DrFinal["CRHEIGHT"] = Regex.Replace(Val.ToString(Dr["CRHEIGHT"]), @"[^0-9\.]+", "");
                    DrFinal["PAVANGLE"] = Regex.Replace(Val.ToString(Dr["PAVANGLE"]), @"[^0-9\.]+", "");
                    DrFinal["PAVHEIGHT"] = Regex.Replace(Val.ToString(Dr["PAVHEIGHT"]), @"[^0-9\.]+", "");

                    DrFinal["STARLENGTH"] = Regex.Replace(Val.ToString(Dr["STARLENGTH"]), @"[^0-9\.]+", "");
                    DrFinal["LOWERHALF"] = Regex.Replace(Val.ToString(Dr["LOWERHALF"]), @"[^0-9\.]+", "");

                    DrFinal["PAINTING"] = Dr["PAINTING"];
                    DrFinal["PROPORTIONS"] = Dr["PROPORTIONS"];
                    DrFinal["PAINTCOMM"] = Dr["PAINTCOMM"];

                    DrFinal["KEYTOSYMBOL"] = Dr["KEYTOSYMBOL"];

                    DrFinal["REPORTCOMMENT"] = Dr["REPORTCOMMENT"];

                    DrFinal["INSCRIPTION"] = Dr["INSCRIPTION"];

                    DrFinal["SYNTHETICINDICATOR"] = Dr["SYNTHETICINDICATOR"];
                    DrFinal["GIRDLEPER"] = Regex.Replace(Val.ToString(Dr["GIRDLEPER"]), @"[^0-9\.]+", "");
                    DrFinal["POLISHFEATURES"] = Dr["POLISHFEATURES"];
                    DrFinal["SYMMETRYFEATURES"] = Dr["SYMMETRYFEATURES"];
                    DrFinal["SHAPEDESC"] = Dr["SHAPEDESC"];

                    DrFinal["REPORTTYPE"] = Dr["REPORTTYPE"];

                    DrFinal["GIARESULTSTATUS"] = "RESULT";
                    

                    IntSrNo++;

                    DtabFinal.Rows.Add(DrFinal);
                }
                DSet = ObjUpload.GIAAPI_Save(DtabFinal, mGroup_ID, "GIA", Val.SqlDate(DateTime.Now.ToShortDateString()), StrResultStatus);

                MainGrid.DataSource = DSet.Tables[1];
                MainGrid.Refresh();
                GrdDetail.OptionsBehavior.Editable = false;
                GrdDet.BestFitColumns();

                mUpload_ID = Guid.Empty;
                DtabFinal.Rows.Clear();
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }

                int IntAll = 0;
                int IntIdle = 0; 
                int IntUP = 0;
                int IntDown = 0;

                foreach (DataRow DRow in DSet.Tables[1].Rows)
                {
                    IntAll++;
                    if (Val.Val(DRow["SHAPENAMESEQ"]) == Val.Val(DRow["SHAPESEQ"]) && Val.Val(DRow["COLORNAMESEQ"]) == Val.Val(DRow["COLORSEQ"]) && Val.Val(DRow["CLARITYNAMESEQ"]) == Val.Val(DRow["CLARITYSEQ"]) && Val.Val(DRow["CUTNAMESEQ"]) == Val.Val(DRow["CUTSEQ"]) && Val.Val(DRow["POLNAMESEQ"]) == Val.Val(DRow["POLISHSEQ"]) && Val.Val(DRow["SYMNAMESEQ"]) == Val.Val(DRow["SYMMETRYSEQ"]) && Val.Val(DRow["FLNAMESEQ"]) == Val.Val(DRow["FLUORESCENCESEQ"]) && Val.Val(DRow["STOCKCTS"]) == Val.Val(DRow["CARAT"]))
                    {
                        IntIdle++;
                    }
                    if (Val.Val(DRow["SHAPENAMESEQ"]) > Val.Val(DRow["SHAPESEQ"]) || Val.Val(DRow["COLORNAMESEQ"]) > Val.Val(DRow["COLORSEQ"]) || Val.Val(DRow["CLARITYNAMESEQ"]) > Val.Val(DRow["CLARITYSEQ"]) || Val.Val(DRow["CUTNAMESEQ"]) > Val.Val(DRow["CUTSEQ"]) || Val.Val(DRow["POLNAMESEQ"]) > Val.Val(DRow["POLISHSEQ"]) || Val.Val(DRow["SYMNAMESEQ"]) > Val.Val(DRow["SYMMETRYSEQ"]) || Val.Val(DRow["FLNAMESEQ"]) > Val.Val(DRow["FLUORESCENCESEQ"]) || Val.Val(DRow["STOCKCTS"]) > Val.Val(DRow["CARAT"]))
                    {
                        IntUP++;
                    }
                    if (Val.Val(DRow["SHAPENAMESEQ"]) < Val.Val(DRow["SHAPESEQ"]) || Val.Val(DRow["COLORNAMESEQ"]) < Val.Val(DRow["COLORSEQ"]) || Val.Val(DRow["CLARITYNAMESEQ"]) < Val.Val(DRow["CLARITYSEQ"]) || Val.Val(DRow["CUTNAMESEQ"]) < Val.Val(DRow["CUTSEQ"]) || Val.Val(DRow["POLNAMESEQ"]) < Val.Val(DRow["POLISHSEQ"]) || Val.Val(DRow["SYMNAMESEQ"]) < Val.Val(DRow["SYMMETRYSEQ"]) || Val.Val(DRow["FLNAMESEQ"]) < Val.Val(DRow["FLUORESCENCESEQ"]) || Val.Val(DRow["STOCKCTS"]) < Val.Val(DRow["CARAT"]))
                    {
                        IntDown++;
                    }
                }

                rbtnAll.Text = "ALL(" + IntAll.ToString() + ")";
                rbtnIdle.Text = "IDLE(" + IntIdle.ToString() + ")"; 
                rbtnUp.Text = "UP(" + IntUP.ToString() + ")";
                rbtnDown.Text = "DOWN(" + IntDown.ToString() + ")";

                this.Cursor = Cursors.Default;
                xtraTabActivity.SelectedTabPageIndex = 1;
                Global.Message("File Upload Successfully");           
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private async void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                GrdDetail.RefreshData();

                //DataTable DTabSelected = GetTableOfSelectedRows(GrdDetail, true);
                DataTable DTabSelected = Global.GetSelectedRecordOfGrid(GrdDetail,true,ObjGridSelection1);
                if (DTabSelected.Rows.Count == 0)
                {
                    Global.Message("Please Select Atleast One Record For Print Request");
                    return;
                }


                foreach (DataRow DRow in DTabSelected.Rows)
                {
                    if (Val.ToString(DRow["CONTROLNO"]) == "")
                    {
                        Global.Message("Control No Is Blank In This Packet No " + Val.ToString(DRow["CLIENTREFNO"]));
                        return;
                    }
                }

                if (Global.Confirm("Are You Sure To [PRINT]  {" + Val.ToInt32 (DTabSelected.Rows.Count) + "} Stones ? ") == DialogResult.No)
                {
                    return;
                }


                this.Cursor = Cursors.WaitCursor;

                var list = DTabSelected.AsEnumerable().Select(r => r["CONTROLNO"].ToString());
                string StrControlNo = string.Join(",", list);

                string StrGIAUserName = ObjUpload.GetGIAUsername();
                string StrGIAClientID = ObjUpload.GetGIAClientID();
                int StrGIASite_ID = Val.ToInt32(ObjUpload.GetGIASiteID());

                string url = "";
                url = "https://labws.gia.edu/ConsolidatedWS/ConsolidatedWS?WSDL";

                string xmlPayload = @"<env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/' xmlns:q0='http://service.consws.model.ngs.com/'>
                                    <env:Header />
                                    <env:Body>
                                        <q0:createPrintRequest>
                                            <arg0><![CDATA[<?xml version='1.0' encoding='UTF-8'?>
                                                <STONES_REPORT_INSTRUCTION_CHANGE_REQUEST>
                                                    <HEADER>
                                                        <USER_NAME>" + StrGIAUserName + @"</USER_NAME>
                                                        <CLIENT_ID>" + StrGIAClientID + @"</CLIENT_ID>
                                                    </HEADER>
                                                    <BODY>
                                                        <CONTROL_NOS>" + StrControlNo + @"</CONTROL_NOS>
                                                        <SITE_ID>" + StrGIASite_ID + @"</SITE_ID>
                                                    </BODY>
                                                </STONES_REPORT_INSTRUCTION_CHANGE_REQUEST>]]>
                                            </arg0>
                                        </q0:createPrintRequest>
                                    </env:Body>
                                </env:Envelope>";
                string response = await SendSoapRequest(url, xmlPayload, "createPrintRequest");

                XDocument doc = XDocument.Parse(response);

                string encodedInnerXml = doc.Descendants("return").First().Value;

                string decodedInnerXml = System.Net.WebUtility.HtmlDecode(encodedInnerXml);

                XDocument innerDoc = XDocument.Parse(decodedInnerXml);

                string status = innerDoc.Descendants("STATUS").FirstOrDefault()?.Value;
                string error = innerDoc.Descendants("ERROR").FirstOrDefault()?.Value;

                if (status == "SUCCESS")
                {
                    DTabSelected.TableName = "Table";
                    string StrXMLValues = string.Empty;
                    using (StringWriter sw = new StringWriter())
                    {
                        DTabSelected.WriteXml(sw);
                        StrXMLValues = sw.ToString();
                    }
                    int IntResult = ObjUpload.SaveGIA(StrXMLValues, "PRINT", "", "", "", "");

                    if (IntResult != -1)
                    {
                        Global.Message("Record Inserte Sucessfully");
                    }
                    else
                    {
                        Global.Message("Something Wrong...");
                    }
                    if (ObjGridSelection1 != null)
                    {
                        ObjGridSelection1.ClearSelection();
                        ObjGridSelection1.CheckMarkColumn.VisibleIndex = 0;
                    }

                    string[] SplitValue = StrControlNo.Split(',');
                    foreach (string value in SplitValue)
                    {
                        // Trim the value to ensure there are no leading/trailing spaces
                        string trimmedValue = value.Trim();

                        // Find rows in the DataTable that match the current value
                        DataRow[] rowsToDelete = DSet.Tables[1].Select($"CONTROLNO = '{trimmedValue}'");

                        // Delete the matching rows
                        foreach (DataRow row in rowsToDelete)
                        {
                            DSet.Tables[1].Rows.Remove(row);
                        }
                    }
                    DSet.Tables[1].AcceptChanges();
                    MainGrid.DataSource = DSet.Tables[1];
                    MainGrid.Refresh();
                    GrdDetail.OptionsBehavior.Editable = false;
                    GrdDet.BestFitColumns();

                    int IntAll = 0;
                    int IntIdle = 0;
                    int IntUP = 0;
                    int IntDown = 0;

                    foreach (DataRow DRow in DSet.Tables[1].Rows)
                    {
                        IntAll++;
                        if (Val.Val(DRow["SHAPENAMESEQ"]) == Val.Val(DRow["SHAPESEQ"]) && Val.Val(DRow["COLORNAMESEQ"]) == Val.Val(DRow["COLORSEQ"]) && Val.Val(DRow["CLARITYNAMESEQ"]) == Val.Val(DRow["CLARITYSEQ"]) && Val.Val(DRow["CUTNAMESEQ"]) == Val.Val(DRow["CUTSEQ"]) && Val.Val(DRow["POLNAMESEQ"]) == Val.Val(DRow["POLISHSEQ"]) && Val.Val(DRow["SYMNAMESEQ"]) == Val.Val(DRow["SYMMETRYSEQ"]) && Val.Val(DRow["FLNAMESEQ"]) == Val.Val(DRow["FLUORESCENCESEQ"]) && Val.Val(DRow["STOCKCTS"]) == Val.Val(DRow["CARAT"]))
                        {
                            IntIdle++;
                        }
                        if (Val.Val(DRow["SHAPENAMESEQ"]) > Val.Val(DRow["SHAPESEQ"]) || Val.Val(DRow["COLORNAMESEQ"]) > Val.Val(DRow["COLORSEQ"]) || Val.Val(DRow["CLARITYNAMESEQ"]) > Val.Val(DRow["CLARITYSEQ"]) || Val.Val(DRow["CUTNAMESEQ"]) > Val.Val(DRow["CUTSEQ"]) || Val.Val(DRow["POLNAMESEQ"]) > Val.Val(DRow["POLISHSEQ"]) || Val.Val(DRow["SYMNAMESEQ"]) > Val.Val(DRow["SYMMETRYSEQ"]) || Val.Val(DRow["FLNAMESEQ"]) > Val.Val(DRow["FLUORESCENCESEQ"]) || Val.Val(DRow["STOCKCTS"]) > Val.Val(DRow["CARAT"]))
                        {
                            IntUP++;
                        }
                        if (Val.Val(DRow["SHAPENAMESEQ"]) < Val.Val(DRow["SHAPESEQ"]) || Val.Val(DRow["COLORNAMESEQ"]) < Val.Val(DRow["COLORSEQ"]) || Val.Val(DRow["CLARITYNAMESEQ"]) < Val.Val(DRow["CLARITYSEQ"]) || Val.Val(DRow["CUTNAMESEQ"]) < Val.Val(DRow["CUTSEQ"]) || Val.Val(DRow["POLNAMESEQ"]) < Val.Val(DRow["POLISHSEQ"]) || Val.Val(DRow["SYMNAMESEQ"]) < Val.Val(DRow["SYMMETRYSEQ"]) || Val.Val(DRow["FLNAMESEQ"]) < Val.Val(DRow["FLUORESCENCESEQ"]) || Val.Val(DRow["STOCKCTS"]) < Val.Val(DRow["CARAT"]))
                        {
                            IntDown++;
                        }
                    }

                    rbtnAll.Text = "ALL(" + IntAll.ToString() + ")";
                    rbtnIdle.Text = "IDLE(" + IntIdle.ToString() + ")";
                    rbtnUp.Text = "UP(" + IntUP.ToString() + ")";
                    rbtnDown.Text = "DOWN(" + IntDown.ToString() + ")";

                }
                else
                {
                    Global.Message(error.ToString());
                    return;
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void BtnPrintAndInscripion_Click(object sender, EventArgs e)
        {
            GrpInscription.BringToFront();
            GrpInscription.Visible = true;
        }

        private void btnRecheck_Click(object sender, EventArgs e)
        {
            txtRecheckCode.Text = "";
            txtRecheckCode.Tag = "";

            GrpRecheck.Visible = true;
        }

        private void cDevSimpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTabDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection1);
                if (DTabDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                var list = DTabDetail.AsEnumerable().Select(r => r["CLIENTREFNO"].ToString());
                string StrFileName = ExportExcel(string.Join(",", list));

                if (StrFileName == "")
                {
                    Global.Message("Please Select Atleast One Packet");
                    return;
                }
                if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(StrFileName, "CMD");
                }
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }
        public string ExportExcel(string strClientRefNo)//Gunjan:14/03/2023
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataTable DTabDetail = ObjUpload.ExcelExportData(strClientRefNo);

                this.Cursor = Cursors.Default;


                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);

                Color BackColor = Color.White;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;



                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Result Upload");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;


                    #region Stock Detail
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabDetail, true);
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = FontSize;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
                    //worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    //worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    //worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    //worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Font.Color.SetColor(FontColor);

                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                    int CaratsColumn = DTabDetail.Columns["Carat"].Ordinal + 1;
                    int GIACaratColumn = DTabDetail.Columns["GIA Carat"].Ordinal + 1;
                    int RapColumn = DTabDetail.Columns["Rap"].Ordinal + 1;
                    int DiscColumn = DTabDetail.Columns["Disc"].Ordinal + 1;
                    int PerCtsColumn = DTabDetail.Columns["$/Cts"].Ordinal + 1;
                    int AmtColumn = DTabDetail.Columns["Amount"].Ordinal + 1;
                    int DepthperColumn = DTabDetail.Columns["Depth%"].Ordinal + 1;
                    int TablePerColumn = DTabDetail.Columns["Table%"].Ordinal + 1;
                    int CRAgColumn = DTabDetail.Columns["Crn Ag"].Ordinal + 1;

                    worksheet.Cells[StartRow, CaratsColumn, StartColumn, CaratsColumn].Style.Numberformat.Format = "0.000";
                    worksheet.Cells[StartRow, GIACaratColumn, StartRow, GIACaratColumn].Style.Numberformat.Format = "0.000";
                    worksheet.Cells[StartRow, RapColumn, StartRow, RapColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DiscColumn, StartRow, DiscColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, PerCtsColumn, StartRow, PerCtsColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AmtColumn, StartRow, AmtColumn].Style.Numberformat.Format = "0.000";
                    worksheet.Cells[StartRow, DepthperColumn, StartRow, DepthperColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, TablePerColumn, StartRow, TablePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, CRAgColumn, StartRow, CRAgColumn].Style.Numberformat.Format = "0.00";

                    int ShapeColumn = DTabDetail.Columns["GIA Shp"].Ordinal + 1;
                    int ShapeUpdownColumn = DTabDetail.Columns["ShapeUpDown"].Ordinal + 1;
                    int ColorColumn = DTabDetail.Columns["GIA Color"].Ordinal + 1;
                    int ColorUpdownColumn = DTabDetail.Columns["ColorUpDown"].Ordinal + 1;
                    int ClarityColumn = DTabDetail.Columns["GIA Clarity"].Ordinal + 1;
                    int ClarityUpdownColumn = DTabDetail.Columns["ClarityUpDown"].Ordinal + 1;
                    int CutColumn = DTabDetail.Columns["GIA Cut"].Ordinal + 1;
                    int CutUpdownColumn = DTabDetail.Columns["CutUpDown"].Ordinal + 1;
                    int PolColumn = DTabDetail.Columns["GIA Pol"].Ordinal + 1;
                    int PolUpdownColumn = DTabDetail.Columns["PolUpDown"].Ordinal + 1;
                    int SymColumn = DTabDetail.Columns["GIA Sym"].Ordinal + 1;
                    int SymUpdownColumn = DTabDetail.Columns["SymUpDown"].Ordinal + 1;
                    int FloColumn = DTabDetail.Columns["GIA Flo"].Ordinal + 1;
                    int FloUpdownColumn = DTabDetail.Columns["FloUpDown"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["GIA Carat"].Ordinal + 1;
                    int CaratUpdownColumn = DTabDetail.Columns["CaratUpDown"].Ordinal + 1;

                    for (int i = 2; i <= DTabDetail.Rows.Count + 1; i++)
                    {
                        if (Val.ToString(worksheet.Cells[i, ShapeUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, ShapeColumn, i, ShapeColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, ShapeUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, ShapeColumn, i, ShapeColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, ShapeColumn, i, ShapeColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, ColorUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, ColorColumn, i, ColorColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, ColorUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, ColorColumn, i, ColorColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, ColorColumn, i, ColorColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, ClarityUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, ClarityColumn, i, ClarityColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, ClarityUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, ClarityColumn, i, ClarityColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, ClarityColumn, i, ClarityColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, CutUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, CutColumn, i, CutColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, CutUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, CutColumn, i, CutColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, CutColumn, i, CutColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, PolUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, PolColumn, i, PolColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, PolUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, PolColumn, i, PolColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, PolColumn, i, PolColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, SymUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, SymColumn, i, SymColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, SymUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, SymColumn, i, SymColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, SymColumn, i, SymColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, FloUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, FloColumn, i, FloColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, FloUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, FloColumn, i, FloColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, FloColumn, i, FloColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, CaratUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, CaratColumn, i, CaratColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, CaratUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, CaratColumn, i, CaratColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, CaratColumn, i, CaratColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }
                    }

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();
                    worksheet.Column(ShapeUpdownColumn).Hidden = true;
                    worksheet.Column(ColorUpdownColumn).Hidden = true;
                    worksheet.Column(ClarityUpdownColumn).Hidden = true;
                    worksheet.Column(CutUpdownColumn).Hidden = true;
                    worksheet.Column(PolUpdownColumn).Hidden = true;
                    worksheet.Column(SymUpdownColumn).Hidden = true;
                    worksheet.Column(FloUpdownColumn).Hidden = true;
                    worksheet.Column(CaratUpdownColumn).Hidden = true;


                    #endregion

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

        private void txtRecheckCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "RECHECKNAME,RECHECKCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_RECHECKCODE);

                    FrmSearch.mStrColumnsToHide = "RECHECK_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtRecheckCode.Text = Val.ToString(FrmSearch.DRow["RECHECKCODE"]);
                        txtRecheckCode.Tag = Val.ToString(FrmSearch.DRow["RECHECK_ID"]);
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

        private async void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable DTabSelected = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection1);

                if (DTabSelected.Rows.Count == 0)
                {
                    Global.Message("Please Select Atleast One Record For Print Request");
                    return;
                }


                foreach (DataRow DRow in DTabSelected.Rows)
                {
                    if (Val.ToString(DRow["CONTROLNO"]) == "")
                    {
                        Global.Message("Control No Is Blank In This Packet No " + Val.ToString(DRow["CLIENTREFNO"]));
                        return;
                    }
                }

                if (Global.Confirm("Are You Sure To [Recheck]  {" + Val.ToInt32(DTabSelected.Rows.Count) + "} Stones ? ") == DialogResult.No)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                string status = "";
                string error = "";

                var list = DTabSelected.AsEnumerable().Select(r => r["CONTROLNO"].ToString());
                string StrControlNo = string.Join(",", list);

                for (int i = 0; i < DTabSelected.Rows.Count; i++)
                {
                    //var list = DTabSelected.AsEnumerable().Select(r => r["CONTROLNO"].ToString());
                   // string StrControlNo = Val.ToString(DTabSelected.Rows[i]["CONTROLNO"]);

                    string StrGIAUserName = ObjUpload.GetGIAUsername();
                    string StrGIAClientID = ObjUpload.GetGIAClientID();
                    int StrGIASite_ID = Val.ToInt32(ObjUpload.GetGIASiteID());

                    string url = "";
                    url = "https://labws.gia.edu/ConsolidatedWS/ConsolidatedWS?WSDL";

                    string xmlPayload = @"<env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/' xmlns:q0='http://service.consws.model.ngs.com/'>
                                    <env:Header />
                                    <env:Body>
                                        <q0:addServiceRequest>
                                            <arg0><![CDATA[<?xml version='1.0' encoding='UTF-8'?>
                                               <ADD_SERVICE_REQUEST>
                                                    <HEADER>
                                                        <USER_NAME>" + StrGIAUserName + @"</USER_NAME>
                                                        <CLIENT_ID>" + StrGIAClientID + @"</CLIENT_ID>
                                                    </HEADER>
                                                    <BODY>
                                                        <SITE_ID>" + StrGIASite_ID + @"</SITE_ID>
                                                        <CONTROL_NUMBER>" + Val.ToString(DTabSelected.Rows[i]["CONTROLNO"]) + @"</CONTROL_NUMBER>
                                                        <SERVICE_CODES>" + Val.ToString(txtRecheckCode.Text) + @"</SERVICE_CODES>
                                                        <INSCRIPTION_SERVICE_CODES></INSCRIPTION_SERVICE_CODES>
                                                        <INSCRIPTION_TEXT></INSCRIPTION_TEXT>
                                                        <CLIENT_COMMENT></CLIENT_COMMENT>
                                                    </BODY>
                                               </ADD_SERVICE_REQUEST>]]>
                                            </arg0>
                                        </q0:addServiceRequest>
                                    </env:Body>
                                </env:Envelope>";
                    string response = await SendSoapRequest(url, xmlPayload, "addServiceRequest");

                    XDocument doc = XDocument.Parse(response);

                    string encodedInnerXml = doc.Descendants("return").First().Value;

                    string decodedInnerXml = System.Net.WebUtility.HtmlDecode(encodedInnerXml);

                    XDocument innerDoc = XDocument.Parse(decodedInnerXml);

                    status = innerDoc.Descendants("STATUS").FirstOrDefault()?.Value;
                    error = innerDoc.Descendants("ERROR").FirstOrDefault()?.Value;

                    if (status != "SUCCESS")
                    {
                        break;
                    }
                }
                if (status != "SUCCESS")
                {
                    Global.Message(error.ToString());
                    return;
                }


                DTabSelected.TableName = "Table";
                string StrXMLValues = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabSelected.WriteXml(sw);
                    StrXMLValues = sw.ToString();
                }

                int IntResult = ObjUpload.SaveGIA(StrXMLValues, "RECHECK", Val.ToString(txtServiceCode.Text), "", Val.ToString(txtClientComment.Text), Val.ToString(txtRecheckCode.Text));

                if (IntResult != 0)
                {
                    Global.Message("Record Inserte Sucessfully");
                }
                else
                {
                    Global.Message("Something Wrong...");
                }
                if (ObjGridSelection1 != null)
                {
                    ObjGridSelection1.ClearSelection();
                    ObjGridSelection1.CheckMarkColumn.VisibleIndex = 0;
                }
                string[] SplitValue = StrControlNo.Split(',');
                foreach (string value in SplitValue)
                {
                    // Trim the value to ensure there are no leading/trailing spaces
                    string trimmedValue = value.Trim();

                    // Find rows in the DataTable that match the current value
                    DataRow[] rowsToDelete = DSet.Tables[1].Select($"CONTROLNO = '{trimmedValue}'");

                    // Delete the matching rows
                    foreach (DataRow row in rowsToDelete)
                    {
                        DSet.Tables[1].Rows.Remove(row);
                    }
                }
                DSet.Tables[1].AcceptChanges();
                MainGrid.DataSource = DSet.Tables[1];
                MainGrid.Refresh();
                GrdDetail.OptionsBehavior.Editable = false;
                GrdDet.BestFitColumns();
                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void BtnCloseRecheck_Click(object sender, EventArgs e)
        {
            txtRecheckCode.Text = "";
            txtRecheckCode.Tag = "";

            GrpRecheck.Visible = false;
        }

        private void txtServiceCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "INSCRIPTIONNAME,INSCRIPTIONCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_INSCRIPTIONCODE);

                    FrmSearch.mStrColumnsToHide = "INSCRIPTION_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtServiceCode.Text = Val.ToString(FrmSearch.DRow["INSCRIPTIONCODE"]);
                        txtServiceCode.Tag = Val.ToString(FrmSearch.DRow["INSCRIPTION_ID"]);
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

        private async void BtnPrintInscription_Click(object sender, EventArgs e)
        {
            try
            {

                //DataTable DTabSelected = GetTableOfSelectedRows(GrdDetail, true);
                DataTable DTabSelected = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection1);
                if (DTabSelected.Rows.Count == 0)
                {
                    Global.Message("Please Select Atleast One Record For Print Request");
                    return;
                }


                foreach (DataRow DRow in DTabSelected.Rows)
                {
                    if (Val.ToString(DRow["CONTROLNO"]) == "")
                    {
                        Global.Message("Control No Is Blank In This Packet No " + Val.ToString(DRow["CLIENTREFNO"]));
                        return;
                    }
                }

                if (Global.Confirm("Are You Sure To [INSCRIPTION AND PRINT]  {" + Val.ToInt32(DTabSelected.Rows.Count) + "} Stones ? ") == DialogResult.No)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;


                string status = "";
                string error = "";

                var list = DTabSelected.AsEnumerable().Select(r => r["CONTROLNO"].ToString());
                string StrControlNo = string.Join(",", list);

                for (int i = 0; i < DTabSelected.Rows.Count; i++)
                {
                    //var list = DTabSelected.AsEnumerable().Select(r => r["CONTROLNO"].ToString());
                    // string StrControlNo = Val.ToString(DTabSelected.Rows[i]["CONTROLNO"]);

                    string StrGIAUserName = ObjUpload.GetGIAUsername();
                    string StrGIAClientID = ObjUpload.GetGIAClientID();
                    int StrGIASite_ID = Val.ToInt32(ObjUpload.GetGIASiteID());

                    string url = "";
                    url = "https://labws.gia.edu/ConsolidatedWS/ConsolidatedWS?WSDL";

                    string xmlPayload = @"<env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/' xmlns:q0='http://service.consws.model.ngs.com/'>
                                    <env:Header />
                                    <env:Body>
                                        <q0:inscribeAndPrintRequest>
                                            <arg0><![CDATA[<?xml version='1.0' encoding='UTF-8'?>
                                                <ADD_SERVICE_REQUEST>
                                                    <HEADER>
                                                        <USER_NAME>" + StrGIAUserName + @"</USER_NAME>
                                                        <CLIENT_ID>" + StrGIAClientID + @"</CLIENT_ID>
                                                    </HEADER>
                                                    <BODY>
                                                        <SITE_ID>" + StrGIASite_ID + @"</SITE_ID>
                                                        <CONTROL_NUMBER>" + StrControlNo + @"</CONTROL_NUMBER>
                                                        <SERVICE_CODES>" + Val.ToString(txtServiceCode.Text) + @"</SERVICE_CODES>
                                                        <INSCRIPTION_TEXT></INSCRIPTION_TEXT>
                                                    </BODY>
                                               </ADD_SERVICE_REQUEST>]]>
                                            </arg0>
                                        </q0:inscribeAndPrintRequest>
                                    </env:Body>
                                </env:Envelope>";
                    string response = await SendSoapRequest(url, xmlPayload, "inscribeAndPrintRequest");

                    XDocument doc = XDocument.Parse(response);

                    string encodedInnerXml = doc.Descendants("return").First().Value;

                    string decodedInnerXml = System.Net.WebUtility.HtmlDecode(encodedInnerXml);

                    XDocument innerDoc = XDocument.Parse(decodedInnerXml);

                    status = innerDoc.Descendants("STATUS").FirstOrDefault()?.Value;
                    error = innerDoc.Descendants("ERROR").FirstOrDefault()?.Value;
                    status = innerDoc.Descendants("MESSAGE").FirstOrDefault()?.Value;


                    if (!status.Contains("SUCCESS"))
                    {
                        break;
                    }
                }
                if (!status.Contains("SUCCESS"))
                {
                    Global.Message(error.ToString());
                    return;
                }


                DTabSelected.TableName = "Table";
                string StrXMLValues = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabSelected.WriteXml(sw);
                    StrXMLValues = sw.ToString();
                }

                int IntResult = ObjUpload.SaveGIA(StrXMLValues, "INSCRIBE AND PRINT", Val.ToString(txtServiceCode.Text), Val.ToString(txtInscriptionText.Text), Val.ToString(txtClientComment.Text), "");

                if (IntResult != 0)
                {
                    Global.Message("Record Inserte Sucessfully");
                }
                else
                {
                    Global.Message("Something Wrong...");
                }
                if (ObjGridSelection1 != null)
                {
                    ObjGridSelection1.ClearSelection();
                    ObjGridSelection1.CheckMarkColumn.VisibleIndex = 0;
                }
                string[] SplitValue = StrControlNo.Split(',');
                foreach (string value in SplitValue)
                {
                    // Trim the value to ensure there are no leading/trailing spaces
                    string trimmedValue = value.Trim();

                    // Find rows in the DataTable that match the current value
                    DataRow[] rowsToDelete = DSet.Tables[1].Select($"CONTROLNO = '{trimmedValue}'");

                    // Delete the matching rows
                    foreach (DataRow row in rowsToDelete)
                    {
                        DSet.Tables[1].Rows.Remove(row);
                    }
                }
                DSet.Tables[1].AcceptChanges();
                MainGrid.DataSource = DSet.Tables[1];
                MainGrid.Refresh();
                GrdDetail.OptionsBehavior.Editable = false;
                GrdDet.BestFitColumns();
                this.Cursor = Cursors.Default;

                //var list = DTabSelected.AsEnumerable().Select(r => r["CONTROLNO"].ToString());
                //string StrControlNo = string.Join(",", list);

                //string StrGIAUserName = ObjUpload.GetGIAUsername();
                //string StrGIAClientID = ObjUpload.GetGIAClientID();
                //int StrGIASite_ID = Val.ToInt32(ObjUpload.GetGIASiteID());

                //string url = "";
                //url = "https://labws.gia.edu/ConsolidatedWS/ConsolidatedWS?WSDL";

                //string xmlPayload = @"<env:Envelope xmlns:env='http://schemas.xmlsoap.org/soap/envelope/' xmlns:q0='http://service.consws.model.ngs.com/'>
                //                    <env:Header />
                //                    <env:Body>
                //                        <q0:inscribeAndPrintRequest>
                //                            <arg0><![CDATA[<?xml version='1.0' encoding='UTF-8'?>
                //                                <ADD_SERVICE_REQUEST>
                //                                    <HEADER>
                //                                        <USER_NAME>" + StrGIAUserName + @"</USER_NAME>
                //                                        <CLIENT_ID>" + StrGIAClientID + @"</CLIENT_ID>
                //                                    </HEADER>
                //                                    <BODY>
                //                                        <SITE_ID>" + StrGIASite_ID + @"</SITE_ID>
                //                                        <CONTROL_NUMBER>" + StrControlNo + @"</CONTROL_NUMBER>
                //                                        <SERVICE_CODES>" + Val.ToString(txtServiceCode.Text) + @"</SERVICE_CODES>
                //                                        <INSCRIPTION_TEXT></INSCRIPTION_TEXT>
                //                                    </BODY>
                //                               </ADD_SERVICE_REQUEST>]]>
                //                            </arg0>
                //                        </q0:inscribeAndPrintRequest>
                //                    </env:Body>
                //                </env:Envelope>";
                //string response = await SendSoapRequest(url, xmlPayload, "inscribeAndPrintRequest");


                //XDocument doc = XDocument.Parse(response);

                //string encodedInnerXml = doc.Descendants("return").First().Value;

                //string decodedInnerXml = System.Net.WebUtility.HtmlDecode(encodedInnerXml);

                //XDocument innerDoc = XDocument.Parse(decodedInnerXml);

                //string status = innerDoc.Descendants("STATUS").FirstOrDefault()?.Value;
                //string error = innerDoc.Descendants("ERROR").FirstOrDefault()?.Value;

                //if (status == "SUCCESS")
                //{
                //    DTabSelected.TableName = "Table";
                //    string StrXMLValues = string.Empty;
                //    using (StringWriter sw = new StringWriter())
                //    {
                //        DTabSelected.WriteXml(sw);
                //        StrXMLValues = sw.ToString();
                //    }

                //    int IntResult = ObjUpload.SaveGIA(StrXMLValues, "INSCRIBE AND PRINT", Val.ToString(txtServiceCode.Text), Val.ToString(txtInscriptionText.Text), Val.ToString(txtClientComment.Text), "");

                //    if (IntResult != 0)
                //    {
                //        Global.Message("Record Inserte Sucessfully");
                //    }
                //    else
                //    {
                //        Global.Message("Something Wrong...");
                //    }
                //    if (ObjGridSelection1 != null)
                //    {
                //        ObjGridSelection1.ClearSelection();
                //        ObjGridSelection1.CheckMarkColumn.VisibleIndex = 0;
                //    }
                //    string[] SplitValue = StrControlNo.Split(',');
                //    foreach (string value in SplitValue)
                //    {
                //        // Trim the value to ensure there are no leading/trailing spaces
                //        string trimmedValue = value.Trim();

                //        // Find rows in the DataTable that match the current value
                //        DataRow[] rowsToDelete = DSet.Tables[1].Select($"CONTROLNO = '{trimmedValue}'");

                //        // Delete the matching rows
                //        foreach (DataRow row in rowsToDelete)
                //        {
                //            DSet.Tables[1].Rows.Remove(row);
                //        }
                //    }
                //    DSet.Tables[1].AcceptChanges();
                //    MainGrid.DataSource = DSet.Tables[1];
                //    MainGrid.Refresh();
                //    GrdDetail.OptionsBehavior.Editable = false;
                //    GrdDet.BestFitColumns();
                //}
                //else
                //{
                //    Global.Message(error.ToString());
                //    return;
                //}
                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void BtnCloseInscription_Click(object sender, EventArgs e)
        {
            txtServiceCode.Text = "";
            txtServiceCode.Tag = "";

            txtInscriptionText.Text = "Inscription check";
            txtClientComment.Text = "Inscribe and Print";
            GrpInscription.Visible = false;
        }

        private void GrdDetail_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                if (e.Column.FieldName == "SHAPE")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "SHAPENAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "SHAPESEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                if (e.Column.FieldName == "COLOR")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "COLORNAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "COLORSEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                if (e.Column.FieldName == "CLARITY")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "CLARITYNAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "CLARITYSEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                if (e.Column.FieldName == "CUT")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "CUTNAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "CUTSEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                if (e.Column.FieldName == "POLISH")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "POLNAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "POLISHSEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                if (e.Column.FieldName == "SYMMETRY")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "SYMNAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "SYMMETRYSEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                if (e.Column.FieldName == "FLUORESCENCE")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "FLNAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "FLUORESCENCESEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                if (e.Column.FieldName == "CARAT")
                {
                    double IntISSSeqNo = Val.ToDouble(GrdDetail.GetRowCellValue(e.RowHandle, "STOCKCTS"));
                    double IntRETSeqNo = Val.ToDouble(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }
        private void FillStoneReviseGrid()
        {
            if (DSet.Tables[1].Rows.Count == 0)
            {
                MainGrid.DataSource = null;
                return;
            }

            DataView dv = new DataView(DSet.Tables[1]);
            if (rbtnIdle.Checked)
            {
                dv.RowFilter = "SHAPENAMESEQ = SHAPESEQ AND COLORNAMESEQ = COLORSEQ AND CLARITYNAMESEQ = CLARITYSEQ " +
                  "AND CUTNAMESEQ = CUTSEQ AND POLNAMESEQ = POLISHSEQ AND SYMNAMESEQ = SYMMETRYSEQ " +
                  "AND FLNAMESEQ = FLUORESCENCESEQ AND STOCKCTS = CARAT";
            }
            else if (rbtnUp.Checked)
            {
                dv.RowFilter = "SHAPENAMESEQ > SHAPESEQ OR COLORNAMESEQ > COLORSEQ OR CLARITYNAMESEQ > CLARITYSEQ " +
                  "OR CUTNAMESEQ > CUTSEQ OR POLNAMESEQ > POLISHSEQ OR SYMNAMESEQ > SYMMETRYSEQ " +
                  "OR FLNAMESEQ > FLUORESCENCESEQ OR STOCKCTS > CARAT";
            }
            else if (rbtnDown.Checked)
            {
                dv.RowFilter = "SHAPENAMESEQ < SHAPESEQ OR COLORNAMESEQ < COLORSEQ OR CLARITYNAMESEQ < CLARITYSEQ " +
                   "OR CUTNAMESEQ < CUTSEQ OR POLNAMESEQ < POLISHSEQ OR SYMNAMESEQ < SYMMETRYSEQ " +
                   "OR FLNAMESEQ < FLUORESCENCESEQ OR STOCKCTS < CARAT";
            }

            MainGrid.DataSource = dv.ToTable();
            MainGrid.Refresh();
           
        }

        private void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                FillStoneReviseGrid();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
    }
}