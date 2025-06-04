using BusLib;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using MahantExport.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;

namespace MahantExport.Stock
{
    public partial class FrmLiveStockSearch : Form
    {
        [DllImport("Winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetDefaultPrinter(string printerName);
        private string GetDefaultPrinter()
        {
            PrinterSettings settings = new PrinterSettings();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                settings.PrinterName = printer;
                if (settings.IsDefaultPrinter)
                    return printer;
            }
            return string.Empty;
        }

        BODevGridSelection ObjGridSelection;
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        BOTRN_KapanCreate ObjKapan = new BOTRN_KapanCreate();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();

        public FrmLiveStockSearch()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();


            if (MainGrid.RepositoryItems.Count == 0)
            {
                ObjGridSelection = new BODevGridSelection();
                ObjGridSelection.View = GrdDet;
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }
            else
            {
                ObjGridSelection.ClearSelection();
            }
            GrdDet.Columns["COLSELECTCHECKBOX"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }

            string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDet.Name);

            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDet.RestoreLayoutFromStream(stream);
            }

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
            ObjFormEvent.ObjToDisposeList.Add(ObjKapan);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTabPacketLiveStock = ObjKapan.GetDataForLiveStock();
                DTabPacketLiveStock.Columns.Add(new DataColumn("ERROR", typeof(string)));

                foreach (DataRow DRow in DTabPacketLiveStock.Rows)
                {
                    string StrError = "";

                    if (Val.ToString(DRow["CARAT"]) == "")
                    {
                        StrError = "H_Carat is Missing";
                    }

                    DRow["ERROR"] = StrError;
                }

                DTabPacketLiveStock.AcceptChanges();
                MainGrid.DataSource = DTabPacketLiveStock;
                GrdDet.BestFitColumns();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable Dtab = (DataTable)MainGrid.DataSource;
                //Dtab.AcceptChanges();

                DataTable Dtab = Global.GetSelectedRecordOfGrid(GrdDet, true, ObjGridSelection);

                if (Dtab == null || Dtab.Rows.Count == 0)
                {
                    Global.Message("Please Select Atleast One Stone For TransferTo Marketing");
                    return;
                }


                foreach (DataRow DRow in Dtab.Rows)
                {
                    Int64 pIntPacket_ID = Val.ToInt64(DRow["PACKET_ID"]);
                    if (pIntPacket_ID == 0)
                    {
                        Global.MessageError("You have not Proper detail");
                        return;
                    }

                    //string pStrError = "";
                    //pStrError = Val.ToString(DRow["ERROR"]);
                    //double pDouH_Carat = Val.Val(DRow["H_CARAT"]);
                    //if (pDouH_Carat == 0)
                    //{
                    //    if (Global.Confirm("H_Carat blank , Are You Sure For Entry") == System.Windows.Forms.DialogResult.No)
                    //    {
                    //        return;
                    //    }
                    //}
                    
                }


                Dtab.TableName = "Table";
                string MumbaiReceiveXml;
                using (StringWriter sw = new StringWriter())
                {
                    Dtab.WriteXml(sw);
                    MumbaiReceiveXml = sw.ToString();
                }
                LiveStockProperty Property = new LiveStockProperty();

                Property = ObjKapan.SaveMumbaiReceiveData(Property, MumbaiReceiveXml);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    var list = Dtab.AsEnumerable().Select(r => r["HELIUM_ID"].ToString());
                    string strHelium_Id = string.Join("','", list);

                    int IntRes = ObjKapan.UpdateMumbaiReceiveFlag(strHelium_Id);
                    if (IntRes != -1)
                    {
                        Global.Message(Property.ReturnMessageDesc);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDet, true, ObjGridSelection);

                if (DTab.Rows.Count == 0 || DTab == null)
                {
                    Global.Message("Please Select At Least One Record For Barcode Print.. ");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Print Barcode of All Selected Packets?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string StrBatchFileName = ""; string DefaultPrinter = "";
                StrBatchFileName = Application.StartupPath + "\\TSC_MakableBarcodeNew.txt ";

                string[] lines = File.ReadAllLines(StrBatchFileName);
                DefaultPrinter = GetDefaultPrinter();

                SetDefaultPrinter(lines[0]);

                this.Cursor = Cursors.WaitCursor;
                List<StiReport> rps = new List<StiReport>();

                int IntCount = 0;
                foreach (DataRow DRow in DTab.Rows)
                {
                    string StrBarcode = Val.ToString(DRow["Packet_ID"]);
                    string StrHeliumID = Val.ToString(DRow["Helium_ID"]);
                    string StrBlaInc = Val.ToString(DRow["Bla_Inc"]);
                    string StrColorShade = Val.ToString(DRow["Brown"]);
                    string Strmilky = Val.ToString(DRow["Milky"]);
                    string StrtabIne = Val.ToString(DRow["Tab_Inc"]);
                    string StrLuster = Val.ToString(DRow["Luster"]);
                    string StrTabOpen = Val.ToString(DRow["T_Open"]);
                    string StrCROpen = Val.ToString(DRow["C_Open"]);
                    string StrPavOpen = Val.ToString(DRow["P_Open"]);
                    string StrHA = Val.ToString(DRow["HA"]);
                    string StrGirdle = Val.ToString(DRow["H_Girdle"]);
                    decimal StrDisc = Val.ToDecimal(DRow["Carat"]);
                    string StrCom = "";

                    IntCount++;

                    StiReport report = new StiReport();
                    string BarcodeName = "TSC_JangandBackPrint";
                    report.Load(Application.StartupPath + "\\Barcode\\" + BarcodeName + ".mrt");
                    report.Compile();
                    report.RequestParameters = false;

                    foreach (Stimulsoft.Report.Dictionary.StiSqlDatabase item in report.CompiledReport.Dictionary.Databases)
                    {
                        item.ConnectionString = BusLib.Configuration.BOConfiguration.ConnectionString;
                    }
                    report["HELIUM_ID"] = "'" + StrHeliumID + "'";
                    report["BLACKINC"] = "'" + StrBlaInc + "'";
                    report["COLORSHADE"] = "'" + StrColorShade + "'";
                    report["MILKY"] = "'" + Strmilky + "'";
                    report["TABLEINC"] = "'" + StrtabIne + "'";
                    report["LUSTER"] = "'" + StrLuster + "'";
                    report["TABLEOPEN"] = "'" + StrTabOpen + "'";
                    report["CROWNOPEN"] = "'" + StrCROpen + "'";
                    report["PAVILIONOPEN"] = "'" + StrPavOpen + "'";
                    report["BARCODE"] = "'" + StrBarcode + "'";
                    report["HA"] = "'" + StrHA + "'";
                    report["GIRDLE"] = "'" + StrGirdle + "'";
                    report["DISC"] = StrDisc;
                    report["COM"] = "'" + StrCom + "'"; ;

                    StiSqlDatabase sql = new StiSqlDatabase("Connection", BusLib.Configuration.BOConfiguration.ConnectionString);
                    sql.Alias = "Connection";
                    report.CompiledReport.Dictionary.Databases.Clear();
                    report.CompiledReport.Dictionary.Databases.Add(sql);

                    report.PreviewMode = StiPreviewMode.StandardAndDotMatrix;
                    report.Render(false);
                    report.PrinterSettings.PrinterName = lines[0];
                    report.Print(false);
                    //rps.Add(report);
                }

                //StiReport singleFile = new StiReport();
                //singleFile.NeedsCompiling = false;
                //singleFile.IsRendered = true;

                //Stimulsoft.Report.Units.StiUnit newUnit = Stimulsoft.Report.Units.StiUnit.GetUnitFromReportUnit(singleFile.ReportUnit);
                //singleFile.RenderedPages.Clear();
                //foreach (StiReport rpt in rps)
                //{
                //    foreach (Stimulsoft.Report.Components.StiPage page in rpt.CompiledReport.RenderedPages)
                //    {
                //        page.Report = singleFile;
                //        page.NewGuid();
                //        Stimulsoft.Report.Units.StiUnit oldUnit = Stimulsoft.Report.Units.StiUnit.GetUnitFromReportUnit(rpt.ReportUnit);
                //        if (singleFile.ReportUnit != rpt.ReportUnit)
                //        {
                //            page.Convert(oldUnit, newUnit);
                //        }
                //        singleFile.RenderedPages.Add(page);
                //    }
                //}

                //SetDefaultPrinter(lines[0]);
                ////singleFile.PrinterSettings.PrinterName = lines[0];
                //singleFile.Print(false);
                //SetDefaultPrinter(DefaultPrinter);
                //rps.Clear();
                //rps = null;
                SetDefaultPrinter(DefaultPrinter);
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;

            }
        }

        private void BtnFrontPrint_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDet, true, ObjGridSelection);

                if (DTab.Rows.Count == 0 || DTab == null)
                {
                    Global.Message("Please Select At Least One Record For Barcode Print.. ");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Print Barcode of All Selected Packets?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }


                string StrBatchFileName = ""; string DefaultPrinter = "";
                StrBatchFileName = Application.StartupPath + "\\TSC_MakableBarcodeNew.txt ";

                string[] lines = File.ReadAllLines(StrBatchFileName);
                DefaultPrinter = GetDefaultPrinter();

                // Global.Message(lines[0]);

                SetDefaultPrinter(lines[0]);

                //Global.Message(GetDefaultPrinter());

                this.Cursor = Cursors.WaitCursor;
                List<StiReport> rps = new List<StiReport>();

                int IntCount = 0;
                foreach (DataRow DRow in DTab.Rows)
                {
                    string StrBarcode = Val.ToString(DRow["Packet_ID"]);
                    string StrHeliumID = Val.ToString(DRow["Helium_ID"]);
                    string StrShape = Val.ToString(DRow["Shape"]);
                    string StrColorShade = Val.ToString(DRow["Brown"]);
                    string Strmilky = Val.ToString(DRow["Milky"]);
                    string StrColor = Val.ToString(DRow["Color"]);
                    string StrCut = Val.ToString(DRow["Cut"]);
                    string StrPolish = Val.ToString(DRow["Polish"]);
                    string Strsym = Val.ToString(DRow["Symm"]);
                    string StrFlour = Val.ToString(DRow["Flour"]);
                    string StrMeasurment = Val.ToString(DRow["H_Measurment"]);
                    string StrClarity = Val.ToString(DRow["Clarity"]);
                    decimal StrCarat = Val.ToDecimal(DRow["Carat"]);
                    decimal StrPricePerCts = Val.ToDecimal(DRow["PricePerCts"]);

                    IntCount++;

                    StiReport report = new StiReport();
                    string BarcodeName = "TSC_FrontJangedPrint";
                    report.Load(Application.StartupPath + "\\Barcode\\" + BarcodeName + ".mrt");
                    report.Compile();
                    report.RequestParameters = false;

                    foreach (Stimulsoft.Report.Dictionary.StiSqlDatabase item in report.CompiledReport.Dictionary.Databases)
                    {
                        item.ConnectionString = BusLib.Configuration.BOConfiguration.ConnectionString;
                    }
                    report["HELIUM_ID"] = "'" + StrHeliumID + "'";
                    report["SHAPE"] = "'" + StrShape + "'";
                    report["COLORSHADE"] = "'" + StrColorShade + "'";
                    report["MILKY"] = "'" + Strmilky + "'";
                    report["COLOR"] = "'" + StrColor + "'";
                    report["CUT"] = "'" + StrCut + "'";
                    report["POLISH"] = "'" + StrPolish + "'";
                    report["SYM"] = "'" + Strsym + "'";
                    report["FLOUR"] = "'" + StrFlour + "'";
                    report["BARCODE"] = "'" + StrBarcode + "'";
                    report["MEASURMENT"] = "'" + StrMeasurment + "'";
                    report["CLARITY"] = "'" + StrClarity + "'";
                    report["CARAT"] = StrCarat;
                    report["PRICEPERCARAT"] = StrPricePerCts;

                    StiSqlDatabase sql = new StiSqlDatabase("Connection", BusLib.Configuration.BOConfiguration.ConnectionString);
                    sql.Alias = "Connection";
                    report.CompiledReport.Dictionary.Databases.Clear();
                    report.CompiledReport.Dictionary.Databases.Add(sql);

                    report.PreviewMode = StiPreviewMode.StandardAndDotMatrix;
                    report.Render(false);
                    report.Print(false);
                    //rps.Add(report);
                }

                //StiReport singleFile = new StiReport();
                //singleFile.NeedsCompiling = false;
                //singleFile.IsRendered = true;

                //Stimulsoft.Report.Units.StiUnit newUnit = Stimulsoft.Report.Units.StiUnit.GetUnitFromReportUnit(singleFile.ReportUnit);
                //singleFile.RenderedPages.Clear();
                //foreach (StiReport rpt in rps)
                //{
                //    foreach (Stimulsoft.Report.Components.StiPage page in rpt.CompiledReport.RenderedPages)
                //    {
                //        page.Report = singleFile;
                //        page.NewGuid();
                //        Stimulsoft.Report.Units.StiUnit oldUnit = Stimulsoft.Report.Units.StiUnit.GetUnitFromReportUnit(rpt.ReportUnit);
                //        if (singleFile.ReportUnit != rpt.ReportUnit)
                //        {
                //            page.Convert(oldUnit, newUnit);
                //        }
                //        singleFile.RenderedPages.Add(page);
                //    }
                //}


                //singleFile.PrinterSettings.PrinterName = lines[0];
                //singleFile.Print(false);

                //rps.Clear();
                //rps = null;

                //Global.Message(GetDefaultPrinter() + "Stage1");
                SetDefaultPrinter(DefaultPrinter);
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDet, true, ObjGridSelection);

                if (DTab.Rows.Count == 0 || DTab == null)
                {
                    Global.Message("Please Select At Least One Record For Barcode Print.. ");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Print Barcode of All Selected Packets?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                string StrBatchFileName = "";
                StrBatchFileName = Application.StartupPath + "\\TSC_MakableBarcodeNew.txt ";

                string[] lines = File.ReadAllLines(StrBatchFileName);
                string PATH = lines[0];

                //DialogResult dialogResult = MessageBox.Show("Sure You Want To PRINT ", "PRINT", MessageBoxButtons.YesNo);
                //if (dialogResult == DialogResult.Yes)
                //{
                TextWriter txt = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "TSC_TE210.txt");
                StringBuilder SB = new StringBuilder();
                SB.Length = 0;
                try
                {
                    for (int i = 0; i < DTab.Rows.Count; i++)
                    {

                        string ID = "", BLACK = "", TABLE = "", LUSTER = "", TOO = "", COO = "", POO = "", HA = "", BRO = "", MILKY = "";

                        ID = DTab.Rows[i]["Helium_ID"].ToString();
                        BLACK = DTab.Rows[i]["Bla_Inc"].ToString();
                        TABLE = DTab.Rows[i]["Tab_Inc"].ToString();
                        LUSTER = DTab.Rows[i]["Luster"].ToString();
                        TOO = DTab.Rows[i]["T_Open"].ToString();
                        COO = DTab.Rows[i]["C_Open"].ToString();
                        POO = DTab.Rows[i]["P_Open"].ToString();
                        //COMMENT AND GDDED BY GUNJAN:01/03/2024
                        HA = "";
                        //HA = DTab.Rows[i]["HA"].ToString();

                        BRO = DTab.Rows[i]["Brown"].ToString();
                        MILKY = DTab.Rows[i]["Milky"].ToString();
                        //END AS GUNJAN

                        // SB.AppendLine("     ");

                        SB.AppendLine("<xpml><page quantity='0' pitch='30.0 mm'></xpml>SIZE 57.5 mm, 30 mm ");
                        SB.AppendLine("GAP 2 mm, 0 mm ");
                        SB.AppendLine("DIRECTION 0,0 ");
                        SB.AppendLine("REFERENCE 0,0 ");
                        SB.AppendLine("OFFSET 0 mm ");
                        SB.AppendLine("SET TEAR OFF ");
                        SB.AppendLine("SET PEEL OFF ");
                        SB.AppendLine("SET PARTIAL_CUTTER OFF ");
                        SB.AppendLine("<xpml></page></xpml><xpml><page quantity='1' pitch='30.0 mm'></xpml>SET CUTTER 1 ");
                        SB.AppendLine("CLS ");
                        SB.AppendLine("BOX 5,8,453,185,2 ");
                        SB.AppendLine("BAR 125,120, 288, 2 ");
                        SB.AppendLine("BAR 6,64, 407, 2 ");
                        SB.AppendLine("BAR 158,9, 2, 176 ");
                        SB.AppendLine("BAR 253,9, 2, 176 ");
                        SB.AppendLine("BAR 317,9, 2, 176 ");
                        SB.AppendLine("BAR 285,9, 2, 176 ");
                        SB.AppendLine("BAR 126,9, 2, 176 ");
                        SB.AppendLine("BAR 189,9, 2, 176 ");
                        SB.AppendLine("BAR 222,9, 2, 176 ");
                        SB.AppendLine("BAR 86,9, 2, 176 ");
                        SB.AppendLine("BAR 46,9, 2, 176 ");
                        SB.AppendLine("BAR 413,9, 2, 176 ");
                        SB.AppendLine("BAR 349,9, 2, 176 ");
                        SB.AppendLine("CODEPAGE 1252 ");
                        SB.AppendLine("TEXT 403,17,\"0\",90,7,7,\"BLA\" ");
                        SB.AppendLine("TEXT 371,17,\"0\",90,7,7,\"TAB\" ");
                        SB.AppendLine("TEXT 339,17,\"0\",90,7,7,\"MIL\" ");
                        SB.AppendLine("TEXT 307,17,\"0\",90,7,7,\"LUS\" ");
                        SB.AppendLine("TEXT 275,17,\"0\",90,7,7,\"T.O.\" ");
                        SB.AppendLine("TEXT 211,17,\"0\",90,7,7,\"P.O.\" ");
                        SB.AppendLine("TEXT 180,17,\"0\",90,7,7,\"BRN\" ");
                        SB.AppendLine("TEXT 147,17,\"0\",90,7,7,\"H&A\" ");
                        SB.AppendLine("TEXT 243,17,\"0\",90,7,7,\"C.O.\" ");
                        SB.AppendLine("TEXT 36,17,\"0\",90,7,7,\"COM\" ");
                        SB.AppendLine("TEXT 116,17,\"0\",90,7,7,\"DIS%\" ");
                        SB.AppendLine("BARCODE 453,228,\"39\",41,0,180,2,5,\"" + ID + "/\" ");
                        //  SB.AppendLine("TEXT 453,182,\"0\",180,8,8,\"111-1111\" ");
                        SB.AppendLine("TEXT 451,33,\"0\",90,11,11,\"" + ID + "\" ");
                        SB.AppendLine("BAR 381,9, 2, 176 ");
                        SB.AppendLine("TEXT 75,17,\"0\",90,7,7,\"GRD\" ");
                        SB.AppendLine("TEXT 403,73,\"0\",90,7,7,\"" + BLACK + "\" ");
                        SB.AppendLine("TEXT 371,73,\"0\",90,7,7,\"" + TABLE + "\" ");
                        SB.AppendLine("TEXT 339,73,\"0\",90,7,7,\"" + MILKY + "\" ");
                        SB.AppendLine("TEXT 307,73,\"0\",90,7,7,\"" + LUSTER + "\" ");
                        SB.AppendLine("TEXT 275,73,\"0\",90,7,7,\"" + TOO + "\" ");
                        SB.AppendLine("TEXT 243,73,\"0\",90,7,7,\"" + COO + "\" ");
                        SB.AppendLine("TEXT 211,73,\"0\",90,7,7,\"" + POO + "\" ");
                        SB.AppendLine("TEXT 179,73,\"0\",90,7,7,\"" + BRO + "\" ");
                        SB.AppendLine("TEXT 151,81,\"0\",90,18,18,\"" + HA + "\" ");
                        SB.AppendLine("PRINT 1,1 ");
                        SB.AppendLine("<xpml></page></xpml><xpml><end/></xpml> ");
                    }
                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                    this.Cursor = Cursors.Default;
                }
                txt.Write(SB.ToString());
                txt.Close();

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C COPY " + AppDomain.CurrentDomain.BaseDirectory + "TSC_TE210.txt " + PATH;
                process.StartInfo = startInfo;
                process.Start();
                //}
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            try
            {


                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDet, true, ObjGridSelection);

                if (DTab.Rows.Count == 0 || DTab == null)
                {
                    Global.Message("Please Select At Least One Record For Barcode Print.. ");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Print Barcode of All Selected Packets?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string StrBatchFileName = "";
                StrBatchFileName = Application.StartupPath + "\\TSC_MakableBarcodeNew.txt ";

                string[] lines = File.ReadAllLines(StrBatchFileName);
                string PATH = lines[0];

                DialogResult dialogResult = MessageBox.Show("Sure You Want To PRINT ", "PRINT", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    TextWriter txt = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "TSC_TE210.txt");
                    StringBuilder SB1 = new StringBuilder();
                    SB1.Length = 0;

                    try
                    {
                        for (int i = 0; i < DTab.Rows.Count; i++)
                        {
                            string DPC = "", GIA = "", ID = "", weight = "", CUT = "", POL = "", SYM = "", FLO = "", PA = "", MEAS = "", SHAPE = "", SHADE = "", MILKY = "", COLOR = "", CLARITY = "";

                            // cno = grid.Rows[i].Cells[5].Value.ToString();

                            //Comment And Added By Gunjan:01/03/2024
                            //GIA = DTab.Rows[i]["GIA_SINMIX"].ToString();
                            GIA = "";
                            //End as Gunjan

                            ID = DTab.Rows[i]["Helium_ID"].ToString();
                            weight = DTab.Rows[i]["Carat"].ToString();
                            SHAPE = DTab.Rows[i]["Shape"].ToString();
                            CUT = DTab.Rows[i]["Cut"].ToString();
                            POL = DTab.Rows[i]["Polish"].ToString();
                            SYM = DTab.Rows[i]["Symm"].ToString();
                            FLO = DTab.Rows[i]["Flour"].ToString();
                            PA = "";
                            MEAS = DTab.Rows[i]["H_Measurment"].ToString();
                            SHADE = DTab.Rows[i]["Brown"].ToString();
                            MILKY = DTab.Rows[i]["Milky"].ToString();
                            COLOR = DTab.Rows[i]["Color"].ToString();
                            CLARITY = DTab.Rows[i]["Clarity"].ToString();
                            DPC = DTab.Rows[i]["PricePerCts"].ToString();

                            SB1.AppendLine("<xpml><page quantity='0' pitch='30.0 mm'></xpml>SIZE 60 mm, 30 mm");
                            SB1.AppendLine("GAP 2 mm, 0 mm");
                            SB1.AppendLine("DIRECTION 0,0");
                            SB1.AppendLine("REFERENCE 0,0");
                            SB1.AppendLine("OFFSET 0 mm");
                            SB1.AppendLine("SET TEAR OFF");
                            SB1.AppendLine("SET PEEL OFF");
                            SB1.AppendLine("SET PARTIAL_CUTTER OFF");
                            if (DTab.Rows.Count - 1 == i)
                            {
                                SB1.AppendLine("<xpml></page></xpml><xpml><page quantity='1' pitch='30.0 mm'></xpml>SET CUTTER 1");//SET CUTTER 1
                            }
                            else
                            {
                                SB1.AppendLine("<xpml></page></xpml><xpml><page quantity='1' pitch='30.0 mm'></xpml>");//SET CUTTER 1
                            }

                            SB1.AppendLine("CLS");
                            SB1.AppendLine("BARCODE 335,223,\"39\",38,0,180,2,5,\"" + ID + "/\"");
                            SB1.AppendLine("CODEPAGE 1252");
                            SB1.AppendLine("TEXT 471,229,\"ROMAN.TTF\",180,1,11,\"" + ID + "\"");
                            SB1.AppendLine("TEXT 471,196,\"0\",180,10,10,\"" + SHAPE + "\"");
                            SB1.AppendLine("TEXT 471,157,\"ROMAN.TTF\",180,1,9,\"" + SHADE + "\"");
                            SB1.AppendLine("TEXT 471,133,\"ROMAN.TTF\",180,1,9,\"" + MILKY + "\"");
                            SB1.AppendLine("TEXT 471,109,\"ROMAN.TTF\",180,1,9,\"" + COLOR + "\"");
                            SB1.AppendLine("TEXT 471,85,\"ROMAN.TTF\",180,1,9,\"" + CLARITY + "\"");
                            SB1.AppendLine("TEXT 471,61,\"ROMAN.TTF\",180,1,9,\"" + DPC + "\"");
                            SB1.AppendLine("TEXT 471,29,\"ROMAN.TTF\",180,1,9,\"" + CUT + "\"");
                            SB1.AppendLine("TEXT 431,29,\"ROMAN.TTF\",180,1,9,\"" + POL + "\"");
                            SB1.AppendLine("TEXT 391,29,\"ROMAN.TTF\",180,1,9,\"" + SYM + "\"");
                            SB1.AppendLine("TEXT 343,29,\"ROMAN.TTF\",180,1,9,\"" + FLO + "\"");
                            SB1.AppendLine("TEXT 215,29,\"ROMAN.TTF\",180,1,9,\"" + GIA + "\"");
                            SB1.AppendLine("TEXT 63,29,\"ROMAN.TTF\",180,1,9,\"" + weight + "\"");
                            SB1.AppendLine("TEXT 335,181,\"ROMAN.TTF\",180,1,7,\"" + MEAS + "\"");
                            SB1.AppendLine("CODEPAGE 850");
                            SB1.AppendLine("TEXT 287,31,\"8\",180,1,1,\"" + PA + "\"");
                            SB1.AppendLine("PRINT 1,1");
                            SB1.AppendLine("<xpml></page></xpml><xpml><end/></xpml>");

                        }
                    }
                    catch (Exception ex)
                    {
                        Global.Message(ex.Message.ToString());
                        this.Cursor = Cursors.Default;
                    }
                    txt.Write(SB1.ToString());
                    txt.Close();
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C COPY " + AppDomain.CurrentDomain.BaseDirectory + "TSC_TE210.txt " + PATH;//"\\\\191.168.2.62\\TSC_TE210"
                    process.StartInfo = startInfo;
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }
        }

        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            Stream str = new System.IO.MemoryStream();
            GrdDet.SaveLayoutToStream(str);
            str.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(str);
            string text = reader.ReadToEnd();

            int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDet.Name, text);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Saved");
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDet.Name);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Deleted");
            }
        }

        private void BtnBarcodePrint_Click(object sender, EventArgs e)
        {
            try
            {


                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDet, true, ObjGridSelection);

                if (DTab.Rows.Count == 0 || DTab == null)
                {
                    Global.Message("Please Select At Least One Record For Barcode Print.. ");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Print Barcode of All Selected Packets?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string StrBatchFileName = "";
                StrBatchFileName = Application.StartupPath + "\\BarcodePrintNew.txt ";

                string[] lines = File.ReadAllLines(StrBatchFileName);
                string PATH = lines[0];

                DialogResult dialogResult = MessageBox.Show("Sure You Want To PRINT ", "PRINT", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    TextWriter txt = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "StickerPrn.txt");
                    StringBuilder SB1 = new StringBuilder();
                    SB1.Length = 0;

                    try
                    {
                        for (int i = 0; i < DTab.Rows.Count; i++)
                        {
                            string ID = "", weight = "", SHAPE = "";

                            ID = DTab.Rows[i]["Helium_ID"].ToString();
                            weight = DTab.Rows[i]["Carat"].ToString();
                            SHAPE = DTab.Rows[i]["Shape"].ToString();

                            SB1.AppendLine("^AT");
                            SB1.AppendLine("^O0");
                            SB1.AppendLine("^D0");
                            SB1.AppendLine("^S4");
                            SB1.AppendLine("^H16");
                            SB1.AppendLine("^C1");
                            SB1.AppendLine("^P1");
                            SB1.AppendLine("^Q33.0,3.0");
                            SB1.AppendLine("^W100");
                            SB1.AppendLine("^L");
                            SB1.AppendLine("^O0");
                            SB1.AppendLine("^D0");
                            SB1.AppendLine("^C1");
                            SB1.AppendLine("^P1");
                            SB1.AppendLine("^Q15.0,3.0");
                            SB1.AppendLine("^W55");
                            SB1.AppendLine("^L");

                            SB1.AppendLine("BA,200,13,1,3,64,0,0," + ID + "/");
                            SB1.AppendLine("AB,360,85,1,1,0,0," + weight + "");
                            SB1.AppendLine("AD,23,13,1,1,0,0," + ID + "");
                            SB1.AppendLine("AD,23,51,1,1,0,0," + SHAPE + "");

                            SB1.AppendLine("E");
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.Message(ex.Message.ToString());
                        this.Cursor = Cursors.Default;
                    }
                    txt.Write(SB1.ToString());
                    txt.Close();
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C COPY " + AppDomain.CurrentDomain.BaseDirectory + "StickerPrn.txt " + PATH;//"\\\\191.168.2.62\\TSC_TE210"
                    process.StartInfo = startInfo;
                    process.Start();
                    if (ObjGridSelection != null)
                    {
                        ObjGridSelection.ClearSelection();
                        ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }
        }
    }
}
