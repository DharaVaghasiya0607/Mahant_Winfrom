using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OfficeOpenXml;
using Spire.Xls;
using DevExpress.Data;
using DevExpress.XtraPrintingLinks;
using System.Drawing.Printing;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using BusLib.Transaction;
using System.Xml;
using SDK_SC_RFID_Devices;
using DataClass;
using System.Threading;
using MahantExport.Utility;

namespace MahantExport.Stock
{
    public partial class FrmRFIDScanning : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOTRN_RFIDConnection ObjBORFIDCon = new BOTRN_RFIDConnection();

        DataTable DtabRFIDNo = new DataTable();
        DataTable DtabFoundList = new DataTable();
        DataTable DtabNotFoundList = new DataTable();

        RFID_Device RFIDCurrDevice;

        BODevGridSelection ObjGridSelection;

        string StrMessage;

        private rfidPluggedInfo[] _arrayOfPluggedDevice;
        private int _selectedDevice;
        bool IsEventInitilized = true;
        int IntCntScanTotal = 0, IntCntScanMatched = 0, IntCntScanUnMatched = 0;

        #region Property Settings

        public FrmRFIDScanning()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();
            finddevice();

            DtabRFIDNo.Columns.Add("RFIDTAGNO", typeof(string));

            //DtabRFIDNo.Rows.Add("1000554");
            //DtabRFIDNo.Rows.Add("454");
            //DtabRFIDNo.Rows.Add("454511");
            //DtabRFIDNo.Rows.Add("445345345");
            //DtabRFIDNo.Rows.Add("1033364");

            //MainGrdRFIDTagNo.DataSource = DtabRFIDNo;
            //GrdRFIDTagNo.RefreshData();

            //GrdFoundList.BeginUpdate();
            //if (MainGrdFoundList.RepositoryItems.Count == 0)
            //{
            //    ObjGridSelection = new BODevGridSelection();
            //    ObjGridSelection.View = GrdFoundList;
            //    ObjGridSelection.ISBoolApplicableForPageConcept = true;
            //    ObjGridSelection.ClearSelection();
            //    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            //    GrdFoundList.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
            //}
            //else
            //{
            //    ObjGridSelection.ClearSelection();
            //}

            //GrdFoundList.EndUpdate();
            //if (ObjGridSelection != null)
            //{
            //    ObjGridSelection.ClearSelection();
            //    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            //}
            RFIDCurrDevice = BOConfiguration.RFIDCurrDevice;

            DtabNotFoundList.Columns.Add("NOTFOUNDNO", typeof(string));
            DtabNotFoundList.Columns.Add("BOXNAME", typeof(string));
            Clear();
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

        public void Clear()
        {
            try
            {

                //CmbMachineName.SelectedIndex = -1;
                DtabRFIDNo.Clear();
                DtabFoundList.Clear();
                DtabNotFoundList.Clear();

                txtEditableStoneID.Text = string.Empty;
                lblTotal.Text = "0";


                GrdFoundList.BeginUpdate();
                if (MainGrdFoundList.RepositoryItems.Count == 0)
                {
                    ObjGridSelection = new BODevGridSelection();
                    ObjGridSelection.View = GrdFoundList;
                    ObjGridSelection.ISBoolApplicableForPageConcept = true;
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                    GrdFoundList.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
                }
                else
                {
                    ObjGridSelection.ClearSelection();
                }
                GrdFoundList.EndUpdate();
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }

                MainGrdRFIDTagNo.DataSource = DtabRFIDNo;
                MainGrdRFIDTagNo.RefreshDataSource();

                MainGrdFoundList.DataSource = DtabFoundList;
                MainGrdFoundList.RefreshDataSource();

                MainGrdNotFoundList.DataSource = DtabNotFoundList;
                MainGrdNotFoundList.RefreshDataSource();

                ChkSelRej.Checked = false;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BntClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            try
            {

                if (RFIDCurrDevice != null && RFIDCurrDevice.ConnectionStatus == ConnectionStatus.CS_Connected)
                    RFIDCurrDevice.ReleaseDevice();

                RFIDCurrDevice = new RFID_Device();
                int num = (int)MessageBox.Show("Device connected.");
                //int num = (int)Global.Message("Device connected.");

                RFIDCurrDevice.NotifyRFIDEvent += new NotifyHandlerRFIDDelegate(rfidDev_NotifyRFIDEvent);
                ThreadPool.QueueUserWorkItem((WaitCallback)delegate
                {
                    RFIDCurrDevice.Create_NoFP_Device(_arrayOfPluggedDevice[_selectedDevice].SerialRFID, _arrayOfPluggedDevice[_selectedDevice].portCom);
                });

                ////RFIDCurrDevice = ObjBORFIDCon.ConnectDevice();
                //if (!backgroundWorker1.IsBusy)
                //{
                //    backgroundWorker1.RunWorkerAsync();
                //}
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnDispose_Click(object sender, EventArgs e)
        {
            try
            {
                if (RFIDCurrDevice == null) return;

                if (RFIDCurrDevice.ConnectionStatus == ConnectionStatus.CS_Connected)
                    RFIDCurrDevice.ReleaseDevice();

                CmbMachineName.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        private void finddevice()
        {
            this._arrayOfPluggedDevice = (rfidPluggedInfo[])null;
            RFID_Device rfidDevice = new RFID_Device();
            this._arrayOfPluggedDevice = rfidDevice.getRFIDpluggedDevice(true);
            rfidDevice.ReleaseDevice();
            CmbMachineName.Items.Clear();

            if (this._arrayOfPluggedDevice != null)
            {
                foreach (rfidPluggedInfo rfidPluggedInfo in this._arrayOfPluggedDevice)
                    CmbMachineName.Items.Add((object)rfidPluggedInfo.SerialRFID);
                if (CmbMachineName.Items.Count <= 0)
                    return;
                CmbMachineName.SelectedItem = (object)0;
                BtnCreate.Enabled = true;
            }
            else
            {
                int num = (int)MessageBox.Show("Device not connected");
            }
        }

        private void BtnScan_Click(object sender, EventArgs e)
        {
            try
            {
                //DtabRFIDNo.Rows.Add("100748007501");
                //DtabRFIDNo.Rows.Add("100747937241");
                //DtabRFIDNo.Rows.Add("100748062896");
                //DtabRFIDNo.Rows.Add("100747753741");
                //DtabRFIDNo.Rows.Add("100747940194");
                //return;


                DtabRFIDNo.Rows.Clear();

                if (IsEventInitilized == false)
                {
                    RFIDCurrDevice.NotifyRFIDEvent += (rfidDev_NotifyRFIDEvent);
                    IsEventInitilized = true;
                }
                IntCntScanTotal = 0;
                IntCntScanMatched = 0; IntCntScanUnMatched = 0;
                if (RFIDCurrDevice != null)
                {
                    if (!ObjBORFIDCon.StartScan(RFIDCurrDevice, true))
                        MessageBox.Show("Device is not ready or not connected");
                }
                DtabRFIDNo.AcceptChanges();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        private void rfidDev_NotifyRFIDEvent(object sender, SDK_SC_RfidReader.rfidReaderArgs args)
        {
            switch (args.RN_Value)
            {
                // Event when failed to connect          
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_FailedToConnect:
                    UpdateStatusRFID("Info : Failed to connect");
                    Invoke((MethodInvoker)delegate
                    {
                        //BtnFindAndConnect.Enabled = true;
                    });
                    break;
                // Event when release the object
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_Disconnected:
                    Invoke((MethodInvoker)delegate
                    {
                        //BtnScan.Enabled = false;
                        //BtnStop.Enabled = false;
                        // BtnLEDAllAtOnce.Enabled = false;
                    });
                    break;

                //Event when device is connected
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_Connected:
                    UpdateStatusRFID("Info : Device connected");
                    Invoke((MethodInvoker)delegate
                    {
                        //BtnFindAndConnect.Enabled = false;
                        //BtnScan.Enabled = true;

                    });
                    break;

                // Event when scan started
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ScanStarted:
                    UpdateStatusRFID("Info : Scan started");
                    Invoke((MethodInvoker)delegate
                    {
                        //BtnScan.Enabled = false;
                        //BtnStop.Enabled = true;
                    });
                    break;

                //event when fail to start scan
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ReaderFailToStartScan:
                    Invoke((MethodInvoker)delegate
                    {
                        BtnScan.Enabled = true;
                        BtnStop.Enabled = false;
                    });
                    UpdateStatusRFID("Info : Failed to start scan");
                    break;

                //event when a new tag is identify
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_TagAdded:
                    lblTotal.Invoke((MethodInvoker)delegate
                    {
                        DataRow DR = DtabRFIDNo.NewRow();
                        IntCntScanTotal++;

                        if (!Val.ToString(args.Message).Equals(string.Empty))
                        {
                            DR["RFIDTAGNO"] = args.Message;
                            lblTotal.Text = Val.ToString(IntCntScanTotal);
                            DtabRFIDNo.Rows.Add(DR);
                        }

                        /*
                        if (IntCntScanTotal == 1)
                        {
                            DR["RFIDTAGNO"] = args.Message;
                            //txtGIAControlNo.Text = args.Message;
                        }
                        else
                        {
                            DR["RFIDTAGNO"] = args.Message;
                            //txtGIAControlNo.Text += "," + args.Message;
                        }
                        */


                    });
                    Application.DoEvents();
                    break;

                // Event when scan completed
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ScanCompleted:
                    Invoke((MethodInvoker)delegate
                    {
                        // BtnLEDAllAtOnce.Enabled = true;
                        BtnScan.Enabled = true;
                        BtnStop.Enabled = false;
                    });
                    UpdateStatusRFID("Info : Scan completed");
                    // GetPacketNoAndScanFromRFID();
                    break;

                //error when error during scan
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ReaderScanTimeout:
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ErrorDuringScan:
                    Invoke((MethodInvoker)delegate
                    {
                        BtnScan.Enabled = true;
                        BtnStop.Enabled = false;
                    });
                    UpdateStatusRFID("Info : Scan has error");
                    break;
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ScanCancelByHost:
                    Invoke((MethodInvoker)delegate
                    {
                        BtnScan.Enabled = true;
                        BtnStop.Enabled = false;
                    });
                    UpdateStatusRFID("Info : Scan cancel by host");
                    break;
            }

            Application.DoEvents();
        }
        private void UpdateStatusRFID(string message)
        {
            //Invoke((MethodInvoker)delegate { lblRFIDMessgae.Text = message; });
        }



        private void BtnSelectionRejection_Click(object sender, EventArgs e)
        {
            try
            {
                //ObjGridSelection.ClearSelection();
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                }


                DtabFoundList.Rows.Clear();
                if (ChkSelRej.Checked)
                {
                    GetPacketNoAndScanFromRFID();
                    txtEditableStoneID.Text = string.Empty;
                }
                else
                {
                    string[] Str = txtEditableStoneID.Text.Split(',');
                    DataTable resultTable = new DataTable();
                    List<string> listBoxTag = new List<string>();
                    //hinal 18-01-2022
                    resultTable.Columns.Add("PARTYSTOCKNO");
                    foreach (string s in Str)
                    {
                        resultTable.Rows.Add(s);
                    }
                    resultTable.TableName = "Table1";

                    string StrStockIDXml = "";
                    using (StringWriter sw = new StringWriter())
                    {
                        resultTable.WriteXml(sw);
                        StrStockIDXml = sw.ToString();
                    }

                    DataTable DtFound = new DataTable();
                    DataTable DtNotFound = new DataTable();

                    DataSet DSStoneData = ObjStock.GetPartyStockNofromRFIDTagNo(StrStockIDXml, "SEARCHSTOCKIDWISE");
                    if (DSStoneData.Tables.Count > 0)
                    {
                        DtFound = DSStoneData.Tables[0];
                        DtNotFound = DSStoneData.Tables[1];
                    }

                    DataTable DtabMatchRFIDNO = new DataTable();
                    DtabMatchRFIDNO = DtFound.Clone();
                    DataTable DtabNotMatchNotFound = new DataTable();
                    DtabNotMatchNotFound = DtNotFound.Clone();
                    DataTable DtabNotMatchRFIDNO = new DataTable();
                    DtabNotMatchRFIDNO = DtFound.Clone();

                    int intt = DtabRFIDNo.Rows.Count;

                    if (DtFound.Rows.Count > 0)
                    {
                        var DrowsMatch = (from r in DtFound.AsEnumerable()
                                          where DtabRFIDNo.AsEnumerable().Any(r2 => r["RFIDTAGNO"].ToString().Trim().ToLower() == r2["RFIDTAGNO"].ToString().Trim().ToLower()
                                         )
                                          select r);

                        if (DrowsMatch.Any())
                        {
                            DtabMatchRFIDNO = DrowsMatch.CopyToDataTable();
                        }
                    }
                    if (DtFound.Rows.Count > 0)
                    {
                        //DtabNotMatchRFIDNO = (from r in DtFound.AsEnumerable()
                        //                      where !DtabRFIDNo.AsEnumerable().Any(r2 => r["RFIDTAGNO"].ToString().Trim().ToLower() == r2["RFIDTAGNO"].ToString().Trim().ToLower()
                        //                     )
                        //                      select r).CopyToDataTable();

                        //DtabNotMatchRFIDNO = DtabRFIDNo.AsEnumerable()
                        //                    .Where(ra => DtFound.AsEnumerable()
                        //                    .Any(rb => rb.Field<string>("RFIDTAGNO") != ra.Field<string>("RFIDTAGNO")))
                        //                              .CopyToDataTable();

                        var DrowsNotMatch = DtFound.Rows.OfType<DataRow>().Where(a => DtFound.Rows.OfType<DataRow>().Select
                                             (k => Convert.ToString(k["RFIDTAGNO"])).Except
                                             (DtabRFIDNo.Rows.OfType<DataRow>().Select(k => Convert.ToString(k["RFIDTAGNO"])).ToList()).Contains(Convert.ToString(a["RFIDTAGNO"])));

                        if (DrowsNotMatch.Any())
                        {
                            DtabNotMatchRFIDNO = DrowsNotMatch.CopyToDataTable();
                            
                        }

                    }
                    //if (DtNotFound.Rows.Count > 0)
                    //{

                    //    DtabNotMatchNotFound = (from r in DtNotFound.AsEnumerable()
                    //                            where !DtabNotFoundList.AsEnumerable().Any(r2 => r["NOTFOUNDNO"].ToString().Trim().ToLower() == r2["NOTFOUNDNO"].ToString().Trim().ToLower()
                    //                           )
                    //                            select r).CopyToDataTable();
                    //}

                    DtabFoundList = DtFound.Clone();
                    if (DtabMatchRFIDNO.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in DtabMatchRFIDNO.Rows)
                        {
                            if (Dr != null)
                            {
                                DataRow DRNew = DtabFoundList.NewRow();
                                DRNew["STOCK_ID"] = Dr["STOCK_ID"];
                                DRNew["PARTYSTOCKNO"] = Dr["PARTYSTOCKNO"];
                                DRNew["RFIDTAGNO"] = Dr["RFIDTAGNO"];
                                DRNew["STATUS"] = Dr["STATUS"];
                                DRNew["STOCKTYPE"] = Dr["STOCKTYPE"];
                                DRNew["BOXNAME"] = Dr["BOXNAME"];

                                DtabFoundList.Rows.Add(DRNew);
                                DtabFoundList.AcceptChanges();
                            }
                        }
                    }

                    DtabNotFoundList = DtNotFound.Clone();
                    if (DtNotFound.Rows.Count > 0)
                    {
                        //foreach (DataRow DrNotFnd in DtabNotMatchNotFound.Rows)
                        foreach (DataRow DrNotFnd in DtNotFound.Rows)
                        {
                            if (DrNotFnd != null)
                            {
                                DataRow DRNotNew = DtabNotFoundList.NewRow();

                                DRNotNew["NOTFOUNDNO"] = DrNotFnd["NOTFOUNDNO"];
                                DRNotNew["BOXNAME"] = DrNotFnd["BOXNAME"];

                                DtabNotFoundList.Rows.Add(DRNotNew);
                                DtabNotFoundList.AcceptChanges();
                            }
                        }
                    }

                    if (DtabNotMatchRFIDNO.Rows.Count > 0)
                    {
                        foreach (DataRow DrNotMatchRFID in DtabNotMatchRFIDNO.Rows)
                        {
                            if (DrNotMatchRFID != null)
                            {
                                DataRow DRNotNew = DtabNotFoundList.NewRow();

                                DRNotNew["NOTFOUNDNO"] = DrNotMatchRFID["PARTYSTOCKNO"];
                                DRNotNew["BOXNAME"] = DrNotMatchRFID["BOXNAME"];

                                DtabNotFoundList.Rows.Add(DRNotNew);
                                DtabNotFoundList.AcceptChanges();
                            }
                        }
                    }

                    MainGrdFoundList.DataSource = DtabFoundList;
                    GrdFoundList.RefreshData();

                    MainGrdNotFoundList.DataSource = DtabNotFoundList;
                    GrdNotFoundList.RefreshData();

                    var list = DtabNotFoundList.AsEnumerable().Select(r => r["NOTFOUNDNO"].ToString());
                    txtEditableStoneID.Text = string.Join(",", list);

                    for (int IntI = 0; IntI < GrdFoundList.RowCount; IntI++)
                    {
                        GrdFoundList.SetRowCellValue(IntI, "COLSELECTCHECKBOX", true);
                    }
                    

                    //if (txtEditableStoneID.Text.Trim().Equals(string.Empty))
                    //{
                    //    Global.Message("Please Enter StoneNo");
                    //    txtEditableStoneID.Focus();
                    //    return;
                    //}




                    //string str3 = "";
                    //this.txtMultiStoneId.Text = "";
                    //foreach (string str4 in this.listBox1.Items)
                    //    str3 = str3 + str4 + "','";
                    //this.ObjStkVerify.GetStoneIdList("TrnMast", "StoneId", " and RfIdTag in('" + str3.Substring(0, str3.Length - 2) + ")");
                    //for (int index = 0; index <= this.ObjStkVerify.DataSet.Tables[this.ObjStkVerify.TableGetStoneIdList].Rows.Count - 1; ++index)
                    //{
                    //    Console.WriteLine(Environment.NewLine);
                    //    Console.WriteLine(this.ObjStkVerify.DataSet.Tables[this.ObjStkVerify.TableGetStoneIdList].Rows[index]["StoneId"].ToString());
                    //    this.txtMultiStoneId.AppendText(this.ObjStkVerify.DataSet.Tables[this.ObjStkVerify.TableGetStoneIdList].Rows[index]["StoneId"].ToString() + "\n");
                    //}
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DisposeRFIDObject();
                MessageBox.Show("After Dispose");
                if (StrMessage != "Info : No device detected")
                {
                    RFIDCurrDevice = ObjBORFIDCon.ConnectDevice();
                }
                MessageBox.Show("After ConnectDevice");
            }
            catch (Exception ex)
            {
                //PanelProgress.Visible = false;
                MessageBox.Show("Catch Found");
            }
        }
        private void DisposeRFIDObject()
        {
            if (RFIDCurrDevice == null) return;

            if (RFIDCurrDevice.ConnectionStatus == ConnectionStatus.CS_Connected)
                RFIDCurrDevice.ReleaseDevice();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (RFIDCurrDevice != null)
                {
                    BOConfiguration.RFIDCurrDevice = RFIDCurrDevice;
                    Global.Message("Info : Device Connected");
                }
                else
                    Global.MessageError("Info : No Device Detected");
                PanelProgress.Visible = false;
                this.Close();
            }
            catch (Exception ex)
            {
                PanelProgress.Visible = false;
            }
        }
        private void GetPacketNoAndScanFromRFID()
        {
            if (DtabRFIDNo.Rows.Count > 0)
            {
                DataSet DSStoneData = new DataSet();

                DtabRFIDNo.TableName = "Table1";

                string StrRFIDTagNoXml = "";
                using (StringWriter sw = new StringWriter())
                {
                    DtabRFIDNo.WriteXml(sw);
                    StrRFIDTagNoXml = sw.ToString();
                }
                //      MessageBox.Show(StrRFIDTagNoXml);

                DSStoneData = ObjStock.GetPartyStockNofromRFIDTagNo(StrRFIDTagNoXml, "SEARCHRFIDTAGNOWISE");

                if (DSStoneData.Tables.Count > 0)
                {
                    DtabFoundList = DSStoneData.Tables[0];
                    DtabNotFoundList = DSStoneData.Tables[1];

                    MainGrdFoundList.DataSource = DtabFoundList;
                    GrdFoundList.RefreshData();

                    MainGrdNotFoundList.DataSource = DtabNotFoundList;
                    GrdNotFoundList.RefreshData();
                }


            }
        }

        private void BtnHold_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtStoneDetail = Global.GetSelectedRecordOfGrid(GrdFoundList, true, ObjGridSelection);

                if (DtStoneDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                if (DtStoneDetail.DefaultView.ToTable(true, "STATUS").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Status Stone. Please Select Only Single Status");
                    return;
                }

                string StrStoneNo = string.Empty;

                string StrAllStone_ID = string.Empty;

                foreach (DataRow DRow in DtStoneDetail.Rows)
                {
                    if (StrAllStone_ID.ToString().Trim().Equals(string.Empty))
                        StrAllStone_ID = Val.ToString(DRow["PARTYSTOCKNO"]);
                    else
                        StrAllStone_ID = StrAllStone_ID + "," + Val.ToString(DRow["PARTYSTOCKNO"]);

                    if (!(Val.ToString(DRow["STATUS"]) == "AVAILABLE" || Val.ToString(DRow["STATUS"]) == "OFFLINE"))
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["PARTYSTOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }

                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Available Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                DataTable DtInvDetail = new DataTable();
                LiveStockProperty Property = new LiveStockProperty();
                Property.STOCKNO = StrAllStone_ID;
                DataSet DS = ObjStock.GetLiveStockDataNew(Property, "All");

                if (DS.Tables.Count > 0)
                {
                    DtInvDetail = DS.Tables[0];
                }

                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.HOLD, DtInvDetail, "SINGLE");

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnLight_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Val.ToString(txtEditableStoneID.Text).Trim().Equals(string.Empty))
                //{

                if (!txtEditableStoneID.Text.Trim().Equals(string.Empty))
                {
                    ChkSelRej.Checked = false;
                    BtnSelectionRejection_Click(null, null);
                }

                List<string> listBoxTag = new List<string>();
                if (GrdFoundList.RowCount >= 0)
                {
                    for (int i = 0; i < GrdFoundList.RowCount; i++)
                    {
                        if (Val.ToBoolean(GrdFoundList.GetRowCellValue(i, "COLSELECTCHECKBOX")))
                        {
                            DataRow oDataRowView = GrdFoundList.GetDataRow(i);
                            // if (Val.ToBoolean(oDataRowView["ISFOUND"].ToString()) == true)
                            listBoxTag.Add(oDataRowView["RFIDTAGNO"].ToString());
                        }
                    }
                }
                ObjBORFIDCon.LedOnAll(RFIDCurrDevice, listBoxTag.Cast<string>().ToList());
                Global.Message("After disappear Of Message LED Will Stop"); // while user doesn't close the dialog, the led-lighting thread is still running
                RFIDCurrDevice.StopLightingLeds(); // stops lighting once user closed MessageBox
                //}


                //ArrayList aryLst = new ArrayList();
                //aryLst = ObjGridSelection.GetSelectedArrayList();
                //if (aryLst.Count > 0)
                //{
                //    IntCntScanTotal = 0;
                //    IntCntScanMatched = 0; IntCntScanUnMatched = 0;
                //    RFID_Device RFIDScanAndStartDevice = BOConfiguration.RFIDCurrDevice;
                //    if (IsEventInitilized == true)
                //    {
                //        RFIDCurrDevice.NotifyRFIDEvent -= (rfidDev_NotifyRFIDEvent);
                //        IsEventInitilized = false;
                //    }

                //    if (RFIDScanAndStartDevice != null)
                //    {
                //        if ((RFIDScanAndStartDevice.ConnectionStatus == ConnectionStatus.CS_Connected) && (RFIDScanAndStartDevice.DeviceStatus == DeviceStatus.DS_Ready))
                //        {
                //            RFIDScanAndStartDevice.ScanDevice(true, true);
                //            Thread.Sleep(10000);
                //            //lblMatched.Text = "0";
                //            //lblUnMatched.Text = "0";
                //            lblTotal.Text = "0";
                //            DataTable resultTable = new DataTable();
                //            List<string> listBoxTag = new List<string>();
                //            for (int i = 0; i < aryLst.Count; i++)
                //            {
                //                DataRowView oDataRowView = aryLst[i] as DataRowView;
                //                listBoxTag.Add(oDataRowView["RFIDTAGNO"].ToString());
                //            }

                //            string[] StrScanGIA = ObjBORFIDCon.LedOnAll(RFIDScanAndStartDevice, listBoxTag.Cast<string>().ToList()).Split(',');
                //            Global.Message("After Disappear Of Message LED Will Stop");
                //            RFIDScanAndStartDevice.StopLightingLeds();

                //            DataTable DtNotFound = new DataTable();
                //            DtNotFound.Columns.Add("PARTYSTOCKNO");
                //            DtNotFound.Columns.Add("GIACONTROLNO");
                //            for (int i = 0; i < aryLst.Count; i++)
                //            {
                //                DataRowView oDataRowView = aryLst[i] as DataRowView;
                //                int Intfound = 0;
                //                for (int j = 0; j < StrScanGIA.Count(); j++)
                //                {
                //                    if (oDataRowView["GIACONTROLNO"].ToString() == StrScanGIA[j].ToString())
                //                    {
                //                        Intfound = 1;
                //                        break;
                //                    }
                //                }
                //                if (Intfound == 1 || oDataRowView["GIACONTROLNO"].ToString() == "")
                //                {
                //                    DataRow Dr = DtNotFound.NewRow();
                //                    Dr["PARTYSTOCKNO"] = oDataRowView["PARTYSTOCKNO"].ToString();
                //                    Dr["GIACONTROLNO"] = oDataRowView["GIACONTROLNO"].ToString();
                //                    DtNotFound.Rows.Add(Dr);
                //                }
                //            }
                //            lblMatched.Text = Val.ToString(aryLst.Count - DtNotFound.Rows.Count);
                //            lblUnMatched.Text = Val.ToString(DtNotFound.Rows.Count);
                //            lblTotal.Text = Val.ToString(aryLst.Count);
                //            if (DtNotFound.Rows.Count > 0)
                //            {
                //                FrmPopupGrid FrmPopupGrid = new FrmPopupGrid();
                //                FrmPopupGrid.CountedColumn = "PARTYSTOCKNO";
                //                FrmPopupGrid.MainGrid.DataSource = DtNotFound;
                //                FrmPopupGrid.MainGrid.Dock = DockStyle.Fill;
                //                FrmPopupGrid.GrdDet.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
                //                FrmPopupGrid.GrdDet.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
                //                FrmPopupGrid.GrdDet.OptionsSelection.MultiSelect = true;
                //                FrmPopupGrid.GrdDet.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
                //                FrmPopupGrid.GrdDet.OptionsBehavior.CopyToClipboardWithColumnHeaders = false;
                //                FrmPopupGrid.GrdDet.OptionsBehavior.Editable = false;
                //                FrmPopupGrid.Text = "List Of Packets Not Exists On RFID Machine.";
                //                FrmPopupGrid.ISPostBack = true;
                //                this.Cursor = Cursors.Default;

                //                FrmPopupGrid.Width = 1000;
                //                FrmPopupGrid.GrdDet.OptionsBehavior.Editable = false;

                //                FrmPopupGrid.GrdDet.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                //                FrmPopupGrid.GrdDet.Columns["PARTYSTOCKNO"].Caption = "Stone No";
                //                FrmPopupGrid.GrdDet.Columns["GIACONTROLNO"].Caption = "GIA Control No";

                //                FrmPopupGrid.GrdDet.Columns["PARTYSTOCKNO"].Width = 100;
                //                FrmPopupGrid.GrdDet.Columns["GIACONTROLNO"].Width = 150;
                //                FrmPopupGrid.ShowDialog();
                //                FrmPopupGrid.Hide();
                //                FrmPopupGrid.Dispose();
                //                FrmPopupGrid = null;
                //                return;
                //            }
                //        }
                //    }
                //}
                //else
                //    Global.MessageError("Please Select Any Packet");

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnMemoIssue_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataTable DtStoneDetail = Global.GetSelectedRecordOfGrid(GrdFoundList, true, ObjGridSelection);

                if (DtStoneDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                string StrStoneNo = string.Empty;
                string StrAllStone_ID = string.Empty;
                foreach (DataRow DRow in DtStoneDetail.Rows)
                {
                    if (StrAllStone_ID.ToString().Trim().Equals(string.Empty))
                        StrAllStone_ID = Val.ToString(DRow["PARTYSTOCKNO"]);
                    else
                        StrAllStone_ID = StrAllStone_ID + "," + Val.ToString(DRow["PARTYSTOCKNO"]);

                    if (!(Val.ToString(DRow["STATUS"]) == "AVAILABLE" || Val.ToString(DRow["STATUS"]) == "OFFLINE"))
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["PARTYSTOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Available Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                DataTable DtInvDetail = new DataTable();
                LiveStockProperty Property = new LiveStockProperty();
                Property.STOCKNO = StrAllStone_ID;
                DataSet DS = ObjStock.GetLiveStockDataNew(Property, "All");

                if (DS.Tables.Count > 0)
                {
                    DtInvDetail = DS.Tables[0];
                }

                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                //FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.MEMOISSUE, DtInvDetail, "SINGLE");

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnSalesOrder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataTable DtStoneDetail = Global.GetSelectedRecordOfGrid(GrdFoundList, true, ObjGridSelection);
                if (DtStoneDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                //if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                //{
                //    this.Cursor = Cursors.Default;

                //    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                //    return;
                //}

                string StrStoneNo = string.Empty;
                string StrAllStone_ID = string.Empty;
                foreach (DataRow DRow in DtStoneDetail.Rows)
                {
                    if (StrAllStone_ID.ToString().Trim().Equals(string.Empty))
                        StrAllStone_ID = Val.ToString(DRow["PARTYSTOCKNO"]);
                    else
                        StrAllStone_ID = StrAllStone_ID + "," + Val.ToString(DRow["PARTYSTOCKNO"]);

                    if (!(
                        Val.ToString(DRow["STATUS"]) == "AVAILABLE" ||
                        Val.ToString(DRow["STATUS"]) == "MEMO" ||
                        Val.ToString(DRow["STATUS"]) == "CONSIGNMENT" ||
                        Val.ToString(DRow["STATUS"]) == "OFFLINE" ||
                        Val.ToString(DRow["STATUS"]) == "HOLD"
                        ))
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["PARTYSTOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select (Available / Memo / Offline / Hold) Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                DataTable DtInvDetail = new DataTable();
                LiveStockProperty Property = new LiveStockProperty();
                Property.STOCKNO = StrAllStone_ID;
                DataSet DS = ObjStock.GetLiveStockDataNew(Property, "All");
                if (DS.Tables.Count > 0)
                {
                    DtInvDetail = DS.Tables[0];
                }

                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                //FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.ORDERCONFIRM, DtInvDetail, "SINGLE");

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;

                Global.Message(ex.Message.ToString());
            }
        }

        private void txtboxMaster_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "BOXECODE,BOXNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BOX);
                    FrmSearch.mBoolISPostBack = true;
                    FrmSearch.mStrColumnsToHide = "BOX_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBoxName.Text = Val.ToString(FrmSearch.DRow["BOXNAME"]);
                        txtBoxName.Tag = Val.ToInt32(FrmSearch.DRow["BOX_ID"]);
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

        private DataTable GetPacketNoAndScanFromStockID(DataTable DTab) //hinal 18-01-2022
        {
            DataTable DtabMatchRFIDNO = new DataTable();
            DataTable DtabNotMatchRFIDNO = new DataTable();

            if (DTab.Rows.Count > 0)
            {
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }

                //Global.Message("StartFindStoneNo");
                DtabFoundList.Rows.Clear();
                DtabNotFoundList.Rows.Clear();

                DataSet DSStoneData = new DataSet();

                DTab.TableName = "Table1";

                string StrStockIDXml = "";
                using (StringWriter sw = new StringWriter())
                {
                    DTab.WriteXml(sw);
                    StrStockIDXml = sw.ToString();
                }
                //      MessageBox.Show(StrRFIDTagNoXml);

                DSStoneData = ObjStock.GetPartyStockNofromRFIDTagNo(StrStockIDXml, "SEARCHSTOCKIDWISE");
                DtabMatchRFIDNO = DSStoneData.Tables[0];
                DtabNotMatchRFIDNO = DSStoneData.Tables[1];

                if (DtabMatchRFIDNO.Rows.Count > 0)
                {
                    DtabFoundList = DSStoneData.Tables[0].Clone();
                }

                //if (DSStoneData.Tables[0].Rows.Count > 0)
                //{
                //    DtabMatchRFIDNO = (from r in DSStoneData.Tables[0].AsEnumerable()
                //                       where DtabRFIDNo.AsEnumerable().Any(r2 => r["RFIDTAGNO"].ToString().Trim().ToLower() == r2["RFIDTAGNO"].ToString().Trim().ToLower()
                //                      )
                //                       select r).CopyToDataTable();
                //}
                //if (DSStoneData.Tables[0].Rows.Count > 0)
                //{
                //    DtabNotMatchRFIDNO = (from r in DSStoneData.Tables[0].AsEnumerable()
                //                          where !DtabRFIDNo.AsEnumerable().Any(r2 => r["RFIDTAGNO"].ToString().Trim().ToLower() == r2["RFIDTAGNO"].ToString().Trim().ToLower()
                //                         )
                //                          select r).CopyToDataTable();
                //}

                //DtabFoundList = DSStoneData.Tables[0].Clone();
                //if (DtabMatchRFIDNO.Rows.Count > 0)
                //{
                //    foreach (DataRow Dr in DtabMatchRFIDNO.Rows)
                //    {
                //        if (Dr != null)
                //        {
                //            DataRow DRNew = DtabFoundList.NewRow();

                //            DRNew["STOCK_ID"] = Dr["STOCK_ID"];
                //            DRNew["PARTYSTOCKNO"] = Dr["PARTYSTOCKNO"];
                //            DRNew["RFIDTAGNO"] = Dr["RFIDTAGNO"];
                //            DRNew["STATUS"] = Dr["STATUS"];
                //            DRNew["STOCKTYPE"] = Dr["STOCKTYPE"];

                //            DtabFoundList.Rows.Add(DRNew);
                //            DtabFoundList.AcceptChanges();

                //        }
                //    }
                //}
                DtabFoundList = DSStoneData.Tables[0].Clone();
                if (DtabNotMatchRFIDNO.Rows.Count > 0)
                {
                    foreach (DataRow DrNotMatchRFID in DtabNotMatchRFIDNO.Rows)
                    {
                        if (DrNotMatchRFID != null)
                        {
                            DataRow DRNotNew = DtabNotFoundList.NewRow();

                            DRNotNew["NOTFOUNDNO"] = DrNotMatchRFID["NOTFOUNDNO"];
                            DRNotNew["BOXNAME"] = DrNotMatchRFID["BOXNAME"];

                            DtabNotFoundList.Rows.Add(DRNotNew);
                            DtabNotFoundList.AcceptChanges();
                        }
                    }
                }

                if (DtabNotFoundList.Rows.Count > 0)
                {
                    MainGrdNotFoundList.DataSource = DtabNotFoundList;
                    GrdNotFoundList.RefreshData();
                }

                MainGrdFoundList.DataSource = DtabFoundList;
                GrdFoundList.RefreshData();

                //if (DSStoneData.Tables.Count > 0)
                //{
                //    //DtabFoundList = DSStoneData.Tables[0];
                //    //DtabNotFoundList = DSStoneData.Tables[1];

                //    MainGrdFoundList.DataSource = DtabFoundList;
                //    GrdFoundList.RefreshData();

                //    MainGrdNotFoundList.DataSource = DtabNotFoundList;
                //    GrdNotFoundList.RefreshData();



                //    for (int IntI = 0; IntI < GrdFoundList.RowCount; IntI++)
                //    {
                //        GrdFoundList.SetRowCellValue(IntI, "COLSELECTCHECKBOX", true);
                //    }
                //}

            }
            return DtabMatchRFIDNO;
        }

        private void BtnScanAndLight_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEditableStoneID.Text.Trim().Equals(string.Empty))
                {
                    Global.Message("Please enter RFIDTagNo which You want to start light..");
                    txtEditableStoneID.Focus();
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                string[] Str = txtEditableStoneID.Text.Split(',');
                DataTable resultTable = new DataTable();
                List<string> listBoxTag = new List<string>();
                //hinal 18-01-2022
                resultTable.Columns.Add("PARTYSTOCKNO");
                foreach (string s in Str)
                {
                    resultTable.Rows.Add(s);
                }

                // MessageBox.Show("Step6 " + aryLst.Count.ToString());
                DataTable DTabDBFoundData = GetPacketNoAndScanFromStockID(resultTable);

                ////return;
                ////List<String> fixedLenghtList = new List<string>(Str);
                ////ArrayList aryLst = new ArrayList(fixedLenghtList);
                ////return;
                //ArrayList aryLst = new ArrayList();
                ////Global.Message("Start Array Count :" + resultTable.Rows.Count.ToString());
                //aryLst = ObjGridSelection.GetSelectedArrayList();
                ////Global.Message("End Array Count");
                ////string[] aryLst =
                //if (aryLst.Count > 0)

                //MessageBox.Show("Before TotalScanCount : " + DtabRFIDNo.Rows.Count.ToString() + "");

                if (DTabDBFoundData.Rows.Count > 0)
                {
                    IntCntScanTotal = 0;
                    IntCntScanMatched = 0; IntCntScanUnMatched = 0;
                    //RFID_Device RFIDScanAndStartDevice = BOConfiguration.RFIDCurrDevice;
                    if (IsEventInitilized == true)
                    {
                        //MessageBox.Show("Step -1");
                        RFIDCurrDevice.NotifyRFIDEvent -= (rfidDev_NotifyRFIDEvent);
                        IsEventInitilized = false;
                    }
                    //MessageBox.Show("Step1");
                    if (RFIDCurrDevice != null)
                    {
                        //MessageBox.Show("Step3 : " + RFIDCurrDevice.ConnectionStatus.ToString());
                        if ((RFIDCurrDevice.ConnectionStatus == ConnectionStatus.CS_Connected) && (RFIDCurrDevice.DeviceStatus == DeviceStatus.DS_Ready))
                        {
                            //MessageBox.Show("Step4");
                            RFIDCurrDevice.ScanDevice(true, true);
                            Thread.Sleep(10000);
                            //lblMatched.Text = "0";
                            //lblUnMatched.Text = "0";
                            lblTotal.Text = "0";

                            //MessageBox.Show("Step6 : DB Found Data : " + DTabDBFoundData.Rows.Count.ToString());

                            //for (int i = 0; i < DTabDBFoundData.Rows.Count; i++)
                            //{
                            //    DataRowView oDataRowView = DTabDBFoundData.DefaultView[DTabDBFoundData.Rows.[i]];
                            //    listBoxTag.Add(oDataRowView["RFIDTAGNO"].ToString());
                            //}

                            DataTable DTabDistinct = DTabDBFoundData.DefaultView.ToTable(true, "RFIDTAGNO");
                            listBoxTag = DTabDistinct.AsEnumerable()
                                           .Select(r => r.Field<string>("RFIDTAGNO"))
                                           .ToList();

                            //MessageBox.Show("listBoxTag " + listBoxTag.Cast<string>().ToList().Count.ToString() + "DTabDistinct : " + DTabDistinct.Rows.Count.ToString());
                            string[] StrScanGIA = ObjBORFIDCon.LedOnAll(RFIDCurrDevice, listBoxTag.Cast<string>().ToList()).Split(',');
                            //string[] StrScanGIA = { "1000554", "445345345" };
                            Global.Message("After Disappear Of Message LED Will Stop");
                            RFIDCurrDevice.StopLightingLeds();

                            //MessageBox.Show("Step7 : After Light ScanCount : " + StrScanGIA.Length.ToString() + " " + StrScanGIA[0].ToString());
                            //MessageBox.Show("Step8 : TotalScanCount : " + DtabRFIDNo.Rows.Count.ToString() + "");

                            //DataTable DtNotFound = new DataTable();
                            //DtNotFound.Columns.Add("PARTYSTOCKNO");
                            //DtNotFound.Columns.Add("RFIDTAGNO");
                            for (int i = 0; i < DTabDBFoundData.Rows.Count; i++)
                            {
                                DataRow DRow = DTabDBFoundData.Rows[i];
                                //int Intfound = 0;
                                //for (int j = 0; j < StrScanGIA.Count(); j++)
                                //{
                                //string foundedString = Array.FindAll(StrScanGIA, str => str.ToLower() == DRow["RFIDTAGNO"].ToString());
                                //if (DRow["RFIDTAGNO"].ToString() == StrScanGIA[j].ToString())
                                if (StrScanGIA.Any(str => str == DRow["RFIDTAGNO"].ToString()))
                                {
                                    //MessageBox.Show("Step8 : Found List ");
                                    DataRow DRNew = DtabFoundList.NewRow();
                                    DRNew["STOCK_ID"] = DRow["STOCK_ID"];
                                    DRNew["PARTYSTOCKNO"] = DRow["PARTYSTOCKNO"];
                                    DRNew["RFIDTAGNO"] = DRow["RFIDTAGNO"];
                                    DRNew["STATUS"] = DRow["STATUS"];
                                    DRNew["STOCKTYPE"] = DRow["STOCKTYPE"];
                                    DRNew["BOXNAME"] = DRow["BOXNAME"];
                                    DtabFoundList.Rows.Add(DRNew);
                                    DtabFoundList.AcceptChanges();
                                }
                                else
                                {
                                    //MessageBox.Show("Step8 : Not Found List ");
                                    DataRow DRNotNew = DtabNotFoundList.NewRow();
                                    DRNotNew["NOTFOUNDNO"] = DRow["PARTYSTOCKNO"];
                                    DRNotNew["BOXNAME"] = DRow["BOXNAME"];
                                    DtabNotFoundList.Rows.Add(DRNotNew);
                                    DtabNotFoundList.AcceptChanges();
                                }
                                //}
                                //if (Intfound == 1 || oDataRowView["RFIDTAGNO"].ToString() == "")
                                //{
                                //    DataRow Dr = DtNotFound.NewRow();
                                //    Dr["PARTYSTOCKNO"] = oDataRowView["PARTYSTOCKNO"].ToString();
                                //    Dr["RFIDTAGNO"] = oDataRowView["RFIDTAGNO"].ToString();
                                //    DtNotFound.Rows.Add(Dr);
                                //}
                            }
                            lblTotal.Text = Val.ToString(DTabDBFoundData.Rows.Count);
                            //if (DtNotFound.Rows.Count > 0)
                            //{
                            //    FrmPopupGrid FrmPopupGrid = new FrmPopupGrid();
                            //    FrmPopupGrid.CountedColumn = "PARTYSTOCKNO";
                            //    FrmPopupGrid.MainGrid.DataSource = DtNotFound;
                            //    FrmPopupGrid.MainGrid.Dock = DockStyle.Fill;
                            //    FrmPopupGrid.GrdDet.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
                            //    FrmPopupGrid.GrdDet.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
                            //    FrmPopupGrid.GrdDet.OptionsSelection.MultiSelect = true;
                            //    //FrmPopupGrid.GrdDet.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
                            //    FrmPopupGrid.GrdDet.OptionsBehavior.CopyToClipboardWithColumnHeaders = false;
                            //    FrmPopupGrid.GrdDet.OptionsBehavior.Editable = false;
                            //    FrmPopupGrid.Text = "List Of Packets Not Exists On RFID Machine.";
                            //    FrmPopupGrid.ISPostBack = true;
                            //    this.Cursor = Cursors.Default;

                            //    FrmPopupGrid.Width = 1000;
                            //    FrmPopupGrid.GrdDet.OptionsBehavior.Editable = false;

                            //    FrmPopupGrid.GrdDet.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                            //    FrmPopupGrid.GrdDet.Columns["PARTYSTOCKNO"].Caption = "Stone No";
                            //    FrmPopupGrid.GrdDet.Columns["GIACONTROLNO"].Caption = "GIA Control No";

                            //    FrmPopupGrid.GrdDet.Columns["PARTYSTOCKNO"].Width = 100;
                            //    FrmPopupGrid.GrdDet.Columns["GIACONTROLNO"].Width = 150;
                            //    FrmPopupGrid.ShowDialog();
                            //    FrmPopupGrid.Hide();
                            //    FrmPopupGrid.Dispose();
                            //    FrmPopupGrid = null;
                            //    return;
                            //}
                        }
                    }
                }
                else
                    Global.MessageError("Please Select Any Packet");

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtEditableStoneID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = "";
                if (txtEditableStoneID.Text.Trim().Contains("\t\n"))
                {
                    str1 = txtEditableStoneID.Text.Trim().Replace("\t\n", ",");
                }
                else
                {
                    str1 = txtEditableStoneID.Text.Trim().Replace("\n", ",");
                }

                txtEditableStoneID.Text = str1;
                txtEditableStoneID.Select(txtEditableStoneID.Text.Length, 0);
                lblTotalCount.Text = "(" + txtEditableStoneID.Text.Split(',').Length + ")";
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void FrmRFIDScanning_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeRFIDObject();
        }
    }
}
