using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MahantExport.Report
{
    public partial class FrmReportViewer  : DevControlLib.cDevXtraForm
    {
        DataTable dtExport = new DataTable();
        public FrmReportViewer()
        {
            InitializeComponent();
        }
        string StrPrint = "";
        private void FrmReportViewer_Load(object sender, EventArgs e)
        {
            
        }

        public void ShowForm(string pStrReport, DataTable pDTab)
        {
            try
            {
                RepDoc.Load(Application.StartupPath + "\\RPT\\" + pStrReport + ".rpt");
                RepDoc.SetDataSource(pDTab);

                crystalReportViewer1.ReportSource = RepDoc;



                //if (RepDoc.Subreports.Count == 1)
                //{
                //    //MessageBox.Show("Message Display");
                //    //MessageBox.Show("User : " + Glb.gStrDBUser);
                //    //MessageBox.Show("Pass : " + Glb.gStrDBPass);
                //    //MessageBox.Show("Ser : " + Glb.gStrServerName);
                //    //MessageBox.Show("DB : " + Glb.gStrDBName);
                //    RepDoc.SetDatabaseLogon(Glb.gStrDBUser, Glb.gStrDBPass, Glb.gStrServerName, Glb.gStrDBName);
                //}
                crystalReportViewer1.Zoom(120);
                crystalReportViewer1.Text = "100";
                crystalReportViewer1.ShowGroupTreeButton = true;
                crystalReportViewer1.ShowGroupTree();
                this.Show();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
            
        }
        public void ShowFormPrintWithDuplicate(string pStrReport, DataSet DS)
        {
            try
            {
                RepDoc.Load(Application.StartupPath + "\\RPT\\" + pStrReport + ".rpt");

                RepDoc.Subreports["Original"].SetDataSource(DS.Tables[0]);
                RepDoc.Subreports["Dublicate"].SetDataSource(DS.Tables[1]);
                StrPrint = pStrReport;
                crystalReportViewer1.ReportSource = RepDoc;

                crystalReportViewer1.Zoom(120);
                crystalReportViewer1.Text = "100";

                this.Show();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }
        public void ShowFormJangadPrintWithDuplicate(string pStrReport, DataSet DS)
        {
            try
            {
                string reportPath = Path.Combine(Application.StartupPath, "RPT", pStrReport + ".rpt");

                if (!File.Exists(reportPath))
                {
                    Global.Message("Report file not found: " + reportPath);
                    return;
                }

                RepDoc.Load(reportPath);

                RepDoc.Subreports["Original.rpt"].SetDataSource(DS.Tables[0]);
                RepDoc.Subreports["Duplicate.rpt"].SetDataSource(DS.Tables[1]);

                StrPrint = pStrReport;
                crystalReportViewer1.ReportSource = RepDoc;
                crystalReportViewer1.Zoom(120);
                crystalReportViewer1.Text = "100";

                this.Show();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }


        public void ShowMemoInvoiceHKPrint(string pStrReport, DataTable pDTab, string PartyName = "", string InvoiceNo = "")
        {
            try
            {
                RepDoc.Load(Application.StartupPath + "\\RPT\\" + pStrReport + ".rpt");
                RepDoc.SetDataSource(pDTab);

                crystalReportViewer1.ReportSource = RepDoc;
                crystalReportViewer1.Zoom(100);
                crystalReportViewer1.Text = "100";

                int FormatOptions = (int)(CrystalDecisions.Shared.ViewerExportFormats.PdfFormat | CrystalDecisions.Shared.ViewerExportFormats.WordFormat | CrystalDecisions.Shared.ViewerExportFormats.ExcelFormat);

                crystalReportViewer1.AllowedExportFormats = FormatOptions;

                RepDoc.SummaryInfo.ReportTitle = PartyName.ToUpper() + " " + InvoiceNo;

                this.Show();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }
        public void ShowJangedReport(string pStrReport, DataSet DS)
        {
            try
            {
                RepDoc.Load(Application.StartupPath + "\\RPT\\" + pStrReport + ".rpt");

                RepDoc.Subreports["Original"].SetDataSource(DS.Tables[0]);
                RepDoc.Subreports["Duplicate"].SetDataSource(DS.Tables[1]);

                crystalReportViewer1.ReportSource = RepDoc;

                crystalReportViewer1.Zoom(120);
                crystalReportViewer1.Text = "100";

                //this.Show();
                PrinterSettings getprinterName = new PrinterSettings();
                RepDoc.PrintOptions.PrinterName = getprinterName.PrinterName;
                RepDoc.PrintToPrinter(getprinterName.Copies, false, getprinterName.FromPage, getprinterName.ToPage);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }


        public void ShowFormDirectPrint(string pStrReport, DataTable pDTab)
        {
            try
            {
                RepDoc.Load(Application.StartupPath + "\\RPT\\" + pStrReport + ".rpt");
                RepDoc.SetDataSource(pDTab);

                crystalReportViewer1.ReportSource = RepDoc;

                //if (RepDoc.Subreports.Count == 1)
                //{
                //    //MessageBox.Show("Message Display");
                //    //MessageBox.Show("User : " + Glb.gStrDBUser);
                //    //MessageBox.Show("Pass : " + Glb.gStrDBPass);
                //    //MessageBox.Show("Ser : " + Glb.gStrServerName);
                //    //MessageBox.Show("DB : " + Glb.gStrDBName);
                //    RepDoc.SetDatabaseLogon(Glb.gStrDBUser, Glb.gStrDBPass, Glb.gStrServerName, Glb.gStrDBName);
                //}

                crystalReportViewer1.Zoom(120);
                crystalReportViewer1.Text = "100";

                PrinterSettings getprinterName = new PrinterSettings();
                RepDoc.PrintOptions.PrinterName = getprinterName.PrinterName;
                RepDoc.PrintToPrinter(getprinterName.Copies, false, getprinterName.FromPage, getprinterName.ToPage);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
            
        }

        public void ShowWithPrint(string pStrReport, DataTable pDTab)
        {
            try
            {
                RepDoc.Load(Application.StartupPath + "\\RPT\\" + pStrReport + ".rpt");
                RepDoc.SetDataSource(pDTab);

                crystalReportViewer1.ReportSource = RepDoc;

                //if (RepDoc.Subreports.Count == 1)
                //{
                //    //MessageBox.Show("Message Display");
                //    //MessageBox.Show("User : " + Glb.gStrDBUser);
                //    //MessageBox.Show("Pass : " + Glb.gStrDBPass);
                //    //MessageBox.Show("Ser : " + Glb.gStrServerName);
                //    //MessageBox.Show("DB : " + Glb.gStrDBName);
                //    RepDoc.SetDatabaseLogon(Glb.gStrDBUser, Glb.gStrDBPass, Glb.gStrServerName, Glb.gStrDBName);
                //}

                crystalReportViewer1.Zoom(120);
                crystalReportViewer1.Text = "100";

                this.Show();

                PrinterSettings getprinterName = new PrinterSettings();
                RepDoc.PrintOptions.PrinterName = getprinterName.PrinterName;
                RepDoc.PrintToPrinter(getprinterName.Copies, false, getprinterName.FromPage, getprinterName.ToPage);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
            
        }


        public void ExportPDF(string pStrReport, DataTable pDTab,string pStrExportFilePath)
        {
            try
            {
                RepDoc.Load(Application.StartupPath + "\\RPT\\" + pStrReport + ".rpt");
                RepDoc.SetDataSource(pDTab);

                crystalReportViewer1.ReportSource = RepDoc;

                //if (RepDoc.Subreports.Count == 1)
                //{
                //    //MessageBox.Show("Message Display");
                //    //MessageBox.Show("User : " + Glb.gStrDBUser);
                //    //MessageBox.Show("Pass : " + Glb.gStrDBPass);
                //    //MessageBox.Show("Ser : " + Glb.gStrServerName);
                //    //MessageBox.Show("DB : " + Glb.gStrDBName);
                //    RepDoc.SetDatabaseLogon(Glb.gStrDBUser, Glb.gStrDBPass, Glb.gStrServerName, Glb.gStrDBName);
                //}
                crystalReportViewer1.Zoom(120);
                crystalReportViewer1.Text = "100";

                RepDoc.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pStrExportFilePath);
            
            }
            catch (Exception ex)
            {

            }
            
        }


        public void AddNewRow(string ReportHeaderName,DataTable DTab)
        {
            if (DTab.Rows.Count == 0)
            {
                DataRow DRow = DTab.NewRow();
                DRow["ReportHeaderName"] =ReportHeaderName;
                DTab.Rows.Add(DRow); 
            }            
        }


        public void ShowFormMemoPrint(string pStrReport, DataSet DS,string BrokerName = "")
        {
            try
            {
                RepDoc.Load(Application.StartupPath + "\\RPT\\" + pStrReport + ".rpt");

                RepDoc.Subreports["Original"].SetDataSource(DS.Tables[0]);
                RepDoc.Subreports["Dublicate"].SetDataSource(DS.Tables[1]);

                crystalReportViewer1.ReportSource = RepDoc;

                //if (RepDoc.Subreports.Count == 1)
                //{
                //    //MessageBox.Show("Message Display");
                //    //MessageBox.Show("User : " + Glb.gStrDBUser);
                //    //MessageBox.Show("Pass : " + Glb.gStrDBPass);
                //    //MessageBox.Show("Ser : " + Glb.gStrServerName);
                //    //MessageBox.Show("DB : " + Glb.gStrDBName);
                //    RepDoc.SetDatabaseLogon(Glb.gStrDBUser, Glb.gStrDBPass, Glb.gStrServerName, Glb.gStrDBName);
                //}
                crystalReportViewer1.Zoom(90);
                crystalReportViewer1.Text = "100";

                int FormatOptions = (int)(CrystalDecisions.Shared.ViewerExportFormats.PdfFormat | CrystalDecisions.Shared.ViewerExportFormats.WordFormat | CrystalDecisions.Shared.ViewerExportFormats.ExcelFormat);

                crystalReportViewer1.AllowedExportFormats = FormatOptions;

                //crystalReportViewer1.AllowedExportFormats = 1;//Add shiv
                //crystalReportViewer1.AllowedExportFormats = 2;//Add shiv
                //crystalReportViewer1.AllowedExportFormats = 3;//Add shiv
                RepDoc.SummaryInfo.ReportTitle = BrokerName.ToUpper();

                this.Show();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        public void ShowFormMultiPrint(string pStrReport, DataSet DS, string BrokerName = "")
        {
            try
            {
                RepDoc.Load(Application.StartupPath + "\\RPT\\" + pStrReport + ".rpt");

                RepDoc.Subreports["01"].SetDataSource(DS.Tables[0]);
                RepDoc.Subreports["02"].SetDataSource(DS.Tables[1]);
                RepDoc.Subreports["03"].SetDataSource(DS.Tables[2]);
                RepDoc.Subreports["05"].SetDataSource(DS.Tables[3]);

                crystalReportViewer1.ReportSource = RepDoc;

                crystalReportViewer1.Zoom(100);
                crystalReportViewer1.Text = "100";

                crystalReportViewer1.AllowedExportFormats = 1;//Add shiv
                crystalReportViewer1.AllowedExportFormats = 2;//Add shiv
                crystalReportViewer1.AllowedExportFormats = 3;//Add shiv
                RepDoc.SummaryInfo.ReportTitle = BrokerName.ToUpper();

                this.Show();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }
        public void ShowFormInvoicePrint(string pStrReport, DataTable pDTab,string PartyName = "",string InvoiceNo = "")
        {
            try

            {
                //Added by Gunjan on 12/10/2023
                if (pStrReport == "RPT_LabIssueStonePrint")
                {
                    string StrFilrPath = AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\LabIssueStone.pdf";                   
                    RepDoc.Load(Application.StartupPath + "\\RPT\\" + pStrReport + ".rpt");
                    RepDoc.SetDataSource(pDTab);
                    RepDoc.PrintOptions.PrinterName = "microsoft print to pdf"; //comment when gives update to client
                    RepDoc.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, StrFilrPath);
                    return;
                }
                //End as Gunjan
                RepDoc.Load(Application.StartupPath + "\\RPT\\" + pStrReport + ".rpt");
                RepDoc.SetDataSource(pDTab);
                //RepDoc.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\SURAT.xls");

                crystalReportViewer1.ReportSource = RepDoc;
                crystalReportViewer1.Zoom(120);
                crystalReportViewer1.Text = "100";

                int FormatOptions = (int)(CrystalDecisions.Shared.ViewerExportFormats.PdfFormat | CrystalDecisions.Shared.ViewerExportFormats.WordFormat | CrystalDecisions.Shared.ViewerExportFormats.XLSXFormat) ;

                crystalReportViewer1.AllowedExportFormats = FormatOptions;
                //crystalReportViewer1.AllowedExportFormats = 2;
                //crystalReportViewer1.AllowedExportFormats = 3;

                RepDoc.SummaryInfo.ReportTitle = PartyName.ToUpper() + " " + InvoiceNo;
                this.Show();

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        
        public void ShowForm(string pStrReport, DataSet DS)
        {
            try
            {
                RepDoc.Load(Application.StartupPath + "\\RPT\\" + pStrReport + ".rpt");
                RepDoc.SetDataSource(DS.Tables[0]);

                AddNewRow("Fresh Packet Size Wise", DS.Tables[1]);
                AddNewRow("Fresh Packet FL Wise", DS.Tables[2]);

                AddNewRow("Rough Prediction Size Wise", DS.Tables[3]);
                AddNewRow("Rough Prediction Color Wise", DS.Tables[4]);
                AddNewRow("Rough Prediction Clarity Wise", DS.Tables[5]);

                AddNewRow("Final Prediction Size Wise", DS.Tables[6]);
                AddNewRow("Final Prediction Color Wise", DS.Tables[7]);
                AddNewRow("Final Prediction Clarity Wise", DS.Tables[8]);

                AddNewRow("Makable Prediction Size Wise", DS.Tables[9]);
                AddNewRow("Makable Prediction Color Wise", DS.Tables[10]);
                AddNewRow("Makable Prediction Clarity Wise", DS.Tables[11]);

                AddNewRow("Final Grading Size Wise", DS.Tables[12]);
                AddNewRow("Final Grading Color Wise", DS.Tables[13]);
                AddNewRow("Final Grading Clarity Wise", DS.Tables[14]);

                AddNewRow("Department Wise Loss", DS.Tables[15]);
                AddNewRow("Rejections", DS.Tables[16]);

                RepDoc.Subreports["RoughPacketSize"].SetDataSource(DS.Tables[1]);
                RepDoc.Subreports["RoughPacketFL"].SetDataSource(DS.Tables[2]);

                RepDoc.Subreports["RoughPrdSize"].SetDataSource(DS.Tables[3]);
                RepDoc.Subreports["RoughPrdColor"].SetDataSource(DS.Tables[4]);
                RepDoc.Subreports["RoughPrdClarity"].SetDataSource(DS.Tables[5]);

                RepDoc.Subreports["FinalPrdSize"].SetDataSource(DS.Tables[6]);
                RepDoc.Subreports["FinalPrdColor"].SetDataSource(DS.Tables[7]);
                RepDoc.Subreports["FinalPrdClarity"].SetDataSource(DS.Tables[8]);

                RepDoc.Subreports["MakPrdSize"].SetDataSource(DS.Tables[9]);
                RepDoc.Subreports["MakPrdColor"].SetDataSource(DS.Tables[10]);
                RepDoc.Subreports["MakPrdClarity"].SetDataSource(DS.Tables[11]);

                RepDoc.Subreports["GrdPrdSize"].SetDataSource(DS.Tables[12]);
                RepDoc.Subreports["GrdPrdColor"].SetDataSource(DS.Tables[13]);
                RepDoc.Subreports["GrdPrdClarity"].SetDataSource(DS.Tables[14]);

                RepDoc.Subreports["DepartmentLoss"].SetDataSource(DS.Tables[15]);
                RepDoc.Subreports["Rejection"].SetDataSource(DS.Tables[16]);

                crystalReportViewer1.ReportSource = RepDoc;

                //if (RepDoc.Subreports.Count == 1)
                //{
                //    //MessageBox.Show("Message Display");
                //    //MessageBox.Show("User : " + Glb.gStrDBUser);
                //    //MessageBox.Show("Pass : " + Glb.gStrDBPass);
                //    //MessageBox.Show("Ser : " + Glb.gStrServerName);
                //    //MessageBox.Show("DB : " + Glb.gStrDBName);
                //    RepDoc.SetDatabaseLogon(Glb.gStrDBUser, Glb.gStrDBPass, Glb.gStrServerName, Glb.gStrDBName);
                //}
                crystalReportViewer1.Zoom(120);
                crystalReportViewer1.Text = "100";
               
                this.Show();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }


        public void ExportPDF(string pStrReport, DataSet DS, string pStrExportFilePath)
        {
            try
            {
                RepDoc.Load(Application.StartupPath + "\\RPT\\" + pStrReport + ".rpt");
                RepDoc.SetDataSource(DS.Tables[0]);

                AddNewRow("Fresh Packet Size Wise", DS.Tables[1]);
                AddNewRow("Fresh Packet FL Wise", DS.Tables[2]);

                AddNewRow("Rough Prediction Size Wise", DS.Tables[3]);
                AddNewRow("Rough Prediction Color Wise", DS.Tables[4]);
                AddNewRow("Rough Prediction Clarity Wise", DS.Tables[5]);

                AddNewRow("Final Prediction Size Wise", DS.Tables[6]);
                AddNewRow("Final Prediction Color Wise", DS.Tables[7]);
                AddNewRow("Final Prediction Clarity Wise", DS.Tables[8]);

                AddNewRow("Makable Prediction Size Wise", DS.Tables[9]);
                AddNewRow("Makable Prediction Color Wise", DS.Tables[10]);
                AddNewRow("Makable Prediction Clarity Wise", DS.Tables[11]);

                AddNewRow("Final Grading Size Wise", DS.Tables[12]);
                AddNewRow("Final Grading Color Wise", DS.Tables[13]);
                AddNewRow("Final Grading Clarity Wise", DS.Tables[14]);

                AddNewRow("Department Wise Loss", DS.Tables[15]);
                AddNewRow("Rejections", DS.Tables[16]);

                RepDoc.Subreports["RoughPacketSize"].SetDataSource(DS.Tables[1]);
                RepDoc.Subreports["RoughPacketFL"].SetDataSource(DS.Tables[2]);

                RepDoc.Subreports["RoughPrdSize"].SetDataSource(DS.Tables[3]);
                RepDoc.Subreports["RoughPrdColor"].SetDataSource(DS.Tables[4]);
                RepDoc.Subreports["RoughPrdClarity"].SetDataSource(DS.Tables[5]);

                RepDoc.Subreports["FinalPrdSize"].SetDataSource(DS.Tables[6]);
                RepDoc.Subreports["FinalPrdColor"].SetDataSource(DS.Tables[7]);
                RepDoc.Subreports["FinalPrdClarity"].SetDataSource(DS.Tables[8]);

                RepDoc.Subreports["MakPrdSize"].SetDataSource(DS.Tables[9]);
                RepDoc.Subreports["MakPrdColor"].SetDataSource(DS.Tables[10]);
                RepDoc.Subreports["MakPrdClarity"].SetDataSource(DS.Tables[11]);

                RepDoc.Subreports["GrdPrdSize"].SetDataSource(DS.Tables[12]);
                RepDoc.Subreports["GrdPrdColor"].SetDataSource(DS.Tables[13]);
                RepDoc.Subreports["GrdPrdClarity"].SetDataSource(DS.Tables[14]);

                RepDoc.Subreports["DepartmentLoss"].SetDataSource(DS.Tables[15]);
                RepDoc.Subreports["Rejection"].SetDataSource(DS.Tables[16]);

                //rptdoc.SetDataSource(ds.Tables[0]);
                //rptdoc.Subreports[0].SetDataSource(ds2.Tables[0]);
                //rptdoc.Subreports[1].SetDataSource(ds3.Tables[0]);

                crystalReportViewer1.ReportSource = RepDoc;

                //if (RepDoc.Subreports.Count == 1)
                //{
                //    //MessageBox.Show("Message Display");
                //    //MessageBox.Show("User : " + Glb.gStrDBUser);
                //    //MessageBox.Show("Pass : " + Glb.gStrDBPass);
                //    //MessageBox.Show("Ser : " + Glb.gStrServerName);
                //    //MessageBox.Show("DB : " + Glb.gStrDBName);
                //    RepDoc.SetDatabaseLogon(Glb.gStrDBUser, Glb.gStrDBPass, Glb.gStrServerName, Glb.gStrDBName);
                //}
                crystalReportViewer1.Zoom(120);
                crystalReportViewer1.Text = "100";

                RepDoc.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pStrExportFilePath);                
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (StrPrint == "RPT_MemoJangedPrintNew")
            {
                PrinterSettings getprinterName = new PrinterSettings();
                RepDoc.PrintOptions.PrinterName = getprinterName.PrinterName;
                RepDoc.PrintToPrinter(2, false, getprinterName.FromPage, getprinterName.ToPage);
            }
            else
            {
                PrinterSettings getprinterName = new PrinterSettings();
                RepDoc.PrintOptions.PrinterName = getprinterName.PrinterName;
                RepDoc.PrintToPrinter(getprinterName.Copies, false, getprinterName.FromPage, getprinterName.ToPage);
            }
            Global.Message("Print Done");
            //crystalReportViewer1.PrintReport();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmReportViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void crystalReportViewer1_DrillDownSubreport(object source, CrystalDecisions.Windows.Forms.DrillSubreportEventArgs e)
        {
            e.Handled = true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet DS = new DataSet();
                DS.Tables.Add(dtExport);
                ExportToExcel_GIA_SURAT(DS, AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\SURAT.xls", " ", " ", " ", " ");

                if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
                {
                    //System.Diagnostics.Process.Start(StrFilePath, "CMD");
                }

                //this.Cursor = Cursors.WaitCursor;
                //if (dtExport.Rows.Count > 0)
                //{
                //    dtExport.DefaultView.Sort = "SrNo";
                //    //dtExport = dtExport.DefaultView.ToTable(false, "SRNO", "ClientRefNo", "service", "Shape", "Carat", "Value$", "InscriptionService", "Color", "Clarity", "Pol", "Sym", "Cut", "FL", "FluorescenceColor", "LENGTH", "WIDTH", "HEIGHT", "amount", "GIAControlNo", "ClientRegistrationNo", "GSTNo");

                //    SaveFileDialog svDialog = new SaveFileDialog();
                //    svDialog.DefaultExt = ".xlsx";
                //    svDialog.Title = "Export to Excel";
                //    svDialog.FileName = "PackingList_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
                //    svDialog.Filter = "Excel File (*.xlsx)|*.xlsx ";
                //    string StrFilePath = "";
                //    if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                //    {
                //        StrFilePath = svDialog.FileName;
                //        ExcelExportNew(dtExport, StrFilePath);
                //    }
                //    if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
                //    {
                //        System.Diagnostics.Process.Start(StrFilePath, "CMD");
                //    }
                //}

            }
            catch (Exception ex)
            {
                
                Global.Message(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        public static void ExportToExcel_GIA_SURAT(DataSet dataSet, string outputPath, string MEMONO, string DDATE, string rate, string amount_word)
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
                    excelSheet.Range[A3, A3].Value = "GIA INDIA PVT. LTD ";
                    excelSheet.Range[A4, G4].Merge();
                    excelSheet.Range[A4, A4].Value = "2-3 FL, SWASTICK UNIVERSAL, NEAR VELANTINE CINEMA, ";
                    excelSheet.Range[A5, G5].Merge();
                    excelSheet.Range[A5, A5].Value = "DUMAS ROAD, PIPLOD, SURAT    ";

                    string H2 = "H" + (F + 2).ToString();
                    string L2 = "L" + (F + 2).ToString();
                    string H3 = "H" + (F + 3).ToString();
                    string L3 = "L" + (F + 3).ToString();
                    string H4 = "H" + (F + 4).ToString();
                    string L4 = "L" + (F + 4).ToString();
                    string H5 = "H" + (F + 5).ToString();
                    string L5 = "L" + (F + 5).ToString();

                    excelSheet.Range[H2, L2].Merge();
                    excelSheet.Range[H2, H2].Value = "GST No. 24AANFR1290A1ZI";
                    excelSheet.Range[H3, L3].Merge();
                    excelSheet.Range[H3, H3].Value = "GIA-GST No. 24AACCG9457G1ZH";
                    excelSheet.Range[H4, L4].Merge();
                    excelSheet.Range[H4, H4].Value = "MEMO NO. " + MEMONO + " /DATE : " + DDATE;
                    excelSheet.Range[H5, L5].Merge();
                    excelSheet.Range[H5, H5].Value = "GIA Client ID : 300300845142";

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
                        excelSheet.Range[F49, F49].Value = "For- Shivam GEMS";
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
                        excelSheet.Range[F48, F48].Value = "For- Shivam GEMS";
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
            excelWorkbook.Close(true, Type.Missing, Type.Missing);
            excelWorkbook = null;

            DialogResult DIALOGRESULT = MessageBox.Show("Yes To Open ..!! No To Save Only...!! ", "Are You Sure You Want To open or Save", MessageBoxButtons.YesNo);
            if (DIALOGRESULT == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(outputPath);
            }
            else if (DIALOGRESULT == DialogResult.No)
            {  // Release the Application object
                excelApp.Quit();
                excelApp = null;
                //do something else
                // Collect the unreferenced objects
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }



        public string ExcelExportNew(DataTable DTabDetail, string PStrFilePath)
        {
            try
            {

                DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();

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
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Sheet1");

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

        internal void ShowFormPrintWithDuplicate(string v, DataTable dTab)
        {
            throw new NotImplementedException();
        }
    }
}
