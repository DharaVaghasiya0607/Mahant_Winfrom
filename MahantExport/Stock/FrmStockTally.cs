using BusLib;
using BusLib.Configuration;
using BusLib.Transaction;
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
using BusLib.TableName;
using MahantExport.Utility;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using BusLib.Master;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Utils;
using MahantExport.Report;
using SDK_SC_RFID_Devices;
using DataClass;
using MahantExport.Utility;

namespace MahantExport.Stock
{
    public partial class FrmStockTally : DevControlLib.cDevXtraForm
    {
        BODevGridSelection ObjGridSelection;
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();
        BOTRN_StockTally ObjTrn = new BOTRN_StockTally();

        DataTable DTabFound = new DataTable();
        DataTable DTabNotFound = new DataTable();
        DataTable DTabExtra = new DataTable();
        DataTable DTabBox = new DataTable();

        DataTable DTabScan = new DataTable(); //hinal 16-01-2022

        //Kuldeep 09112020 For RFID Work.
        BOTRN_RFIDConnection ObjBORFIDCon = new BOTRN_RFIDConnection();
        RFID_Device RFIDCurrDevice;
        int IntCntScanTotal = 0, IntCntScanMatched = 0, IntCntScanUnMatched = 0;

        #region Property Settings

        public FrmStockTally()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            RFIDCurrDevice = BOConfiguration.RFIDCurrDevice;
            if (RFIDCurrDevice != null)
            {
                RFIDCurrDevice.NotifyRFIDEvent += (rfidDev_NotifyRFIDEvent);
                lblDeviceName.Text = "Info : Device Connected";
                BtnScan.Enabled = true;
                BtnStop.Enabled = true;
                BtnLEDAllAtOnce.Enabled = true;
            }
            else
            {
                lblDeviceName.Text = "Info : No Device Detected";
                BtnScan.Enabled = false;
                BtnStop.Enabled = false;
                BtnLEDAllAtOnce.Enabled = false;
            }
            BtnShow_Click(null, null);
            txtStockBox.Focus();
            this.Show();

            DTPTransferDate.Text = DateTime.Now.ToShortDateString();
            DTPTransferDate_ValueChanged(null, null);
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = false;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjTrn);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion


        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string StockType = "";
                if(RbtNatural.Checked == true)
                {
                    StockType = RbtNatural.Text;
                }
                else
                {
                    StockType = RbtLabGrown.Text;
                }
                DataSet DS = ObjTrn.GetData(Val.SqlDate(DTPTransferDate.Value.ToShortDateString()), StockType);

                DTabNotFound.Rows.Clear();
                DTabFound.Rows.Clear();
                DTabExtra.Rows.Clear();
                DTabScan.Rows.Clear();
                DTabBox.Rows.Clear();


                DTabNotFound = DS.Tables[0];
                DTabFound = DS.Tables[1];
                DTabExtra = DS.Tables[2];
                DTabBox = DS.Tables[3];


                DTabScan = DTabFound.Clone();

                MainGridNotFound.DataSource = DTabNotFound;
                MainGridNotFound.Refresh();

                MainGridFound.DataSource = DTabFound;
                MainGridFound.Refresh();

                MainGridExtra.DataSource = DTabExtra;
                MainGridExtra.Refresh();

                MainGridScan.DataSource = DTabScan;
                MainGridScan.Refresh();

                MainGridSummary.DataSource = DTabBox;
                MainGridSummary.Refresh();

                txtStockBox.Text = string.Empty;
                txtStockNo.Text = string.Empty;
                txtStockNo.Tag = string.Empty;

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnDeleteStockTally_Click(object sender, EventArgs e)
        {
            if (Global.Confirm("Are You Sure For Delete ?") == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            FrmPassword FrmPassword = new FrmPassword();

            if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string StockType = "";
            if (RbtNatural.Checked == true)
            {
                StockType = RbtNatural.Text;
            }
            else
            {
                StockType = RbtLabGrown.Text;
            }


            this.Cursor = Cursors.WaitCursor;
            TrnStockTallyProperty Property = new TrnStockTallyProperty();
            Property.STOCKTALLYDATE = Val.SqlDate(DTPTransferDate.Value.ToShortDateString());
            Property.MAINCATAGORY = StockType;

            Property.FOUNDSTATUS = "FOUND";
            Property = ObjTrn.Delete(Property);
            this.Cursor = Cursors.Default;
            Global.Message(Property.ReturnMessageDesc);
            if (Property.ReturnMessageType == "SUCCESS")
            {
                DTabExtra.Rows.Clear();
                DTabFound.Rows.Clear();
                DTabNotFound.Rows.Clear();
                DTabScan.Rows.Clear();
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
        }

        private void BtnFindAndConnect_Click(object sender, EventArgs e)
        {
            DisposeRFIDObject();
            //Kuldeep 09112020 For RFID Work.
            lblDeviceName.Text = ObjBORFIDCon.FindDevice();
            if (lblDeviceName.Text != "Info : No device detected")
            {
                RFIDCurrDevice = ObjBORFIDCon.ConnectDevice();
                if (RFIDCurrDevice != null)
                    RFIDCurrDevice.NotifyRFIDEvent += (rfidDev_NotifyRFIDEvent);
                else
                    lblDeviceName.Text = "Device Not Found";
            }
            lblRFIDMessgae.Text = "";
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
                        BtnFindAndConnect.Enabled = true;
                    });
                    break;
                // Event when release the object
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_Disconnected:
                    Invoke((MethodInvoker)delegate
                    {
                        BtnScan.Enabled = false;
                        BtnStop.Enabled = false;
                        BtnLEDAllAtOnce.Enabled = false;
                    });
                    break;

                //Event when device is connected
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_Connected:
                    UpdateStatusRFID("Info : Device connected");
                    Invoke((MethodInvoker)delegate
                    {
                        BtnFindAndConnect.Enabled = false;
                        BtnScan.Enabled = true;
                        BtnLEDAllAtOnce.Enabled = true;
                    });
                    break;

                // Event when scan started
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ScanStarted:
                    UpdateStatusRFID("Info : Scan started");
                    Invoke((MethodInvoker)delegate
                    {
                        BtnScan.Enabled = false;
                        BtnStop.Enabled = true;
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
                        IntCntScanTotal++;
                        if (IntCntScanTotal == 1)
                            txtGIAControlNo.Text = args.Message;
                        else
                            txtGIAControlNo.Text += "," + args.Message;
                        lblTotal.Text = Val.ToString(IntCntScanTotal);

                    });
                    Application.DoEvents();
                    break;

                // Event when scan completed
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ScanCompleted:
                    Invoke((MethodInvoker)delegate
                    {
                        BtnLEDAllAtOnce.Enabled = true;
                        BtnScan.Enabled = true;
                        BtnStop.Enabled = false;
                    });
                    UpdateStatusRFID("Info : Scan completed");
                    GetPacketNoAndScanFromRFID();
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

        private void DisposeRFIDObject()
        {
            if (RFIDCurrDevice == null) return;

            if (RFIDCurrDevice.ConnectionStatus == ConnectionStatus.CS_Connected)
                RFIDCurrDevice.ReleaseDevice();
        }

        private void BtnScan_Click(object sender, EventArgs e)
        {
            txtGIAControlNo.Invoke((MethodInvoker)delegate
            {
                txtGIAControlNo.Text = "";
            });
            IntCntScanTotal = 0;
            IntCntScanMatched = 0; IntCntScanUnMatched = 0;
            if (RFIDCurrDevice != null)
            {
                if (!ObjBORFIDCon.StartScan(RFIDCurrDevice, true))
                    MessageBox.Show("Device is not ready or not connected");
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            ObjBORFIDCon.StopScan(RFIDCurrDevice);
        }

        private void BtnLEDAllAtOnce_Click(object sender, EventArgs e)
        {
            List<string> listBoxTag = new List<string>();
            if (GrdDetFound.RowCount >= 0)
            {
                for (int i = 0; i < GrdDetFound.RowCount; i++)
                {
                    DataRow oDataRowView = GrdDetFound.GetDataRow(i);
                    if (Val.ToBoolean(oDataRowView["ISFOUND"].ToString()) == true)
                        listBoxTag.Add(oDataRowView["RFIDTAGNO"].ToString());
                }
            }
            if (GrdDetExtra.RowCount >= 0)
            {
                for (int i = 0; i < GrdDetExtra.RowCount; i++)
                {
                    DataRow oDataRowView = GrdDetExtra.GetDataRow(i);
                    if (Val.ToBoolean(oDataRowView["ISFOUND"].ToString()) == true)
                        listBoxTag.Add(oDataRowView["RFIDTAGNO"].ToString());
                }
            }
            ObjBORFIDCon.LedOnAll(RFIDCurrDevice, listBoxTag.Cast<string>().ToList());
            Global.Message("After disappear Of Message LED Will Stop"); // while user doesn't close the dialog, the led-lighting thread is still running
            RFIDCurrDevice.StopLightingLeds(); // stops lighting once user closed MessageBox
        }

        private void UpdateStatusRFID(string message)
        {
            Invoke((MethodInvoker)delegate { lblRFIDMessgae.Text = message; });
        }

        private void GetPacketNoAndScanFromRFID()
        {
            if (txtGIAControlNo.Text != "")
            {
                //DataTable DtPartyStockNo = ObjStock.GetPartyStockNofromGIAControlNo(txtGIAControlNo.Text);
                Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;

                        DTabScan.Rows.Clear();

                        //MessageBox.Show("Scan Comepleted : Start Data Fill In Grid.." + DTabNotFound.Rows.Count.ToString());
                        string[] StrRFIDTagNo = txtGIAControlNo.Text.Split(',');
                        for (int i = 0; i < StrRFIDTagNo.Count(); i++)
                        {
                            DataRow DRowNotFound = null;
                            int IntI = 0;
                            for (IntI = 0; IntI < DTabNotFound.Rows.Count; IntI++)
                            {
                                DataRow DR = DTabNotFound.Rows[IntI];
                                if (StrRFIDTagNo[i].ToString().Trim() == Val.ToString(DR["RFIDTAGNO"]).Trim())
                                {
                                    DRowNotFound = DR;
                                    break;
                                }
                            }

                            bool ISExists = false;

                            foreach (DataRow DRowFound in DTabFound.Rows)
                            {
                                if (StrRFIDTagNo[i].ToString().Trim() == Val.ToString(DRowFound["RFIDTAGNO"]).Trim())
                                {
                                    ISExists = true;

                                    DataRow DRSNew = DTabScan.NewRow();
                                    DRSNew["ISFOUND"] = true;
                                    DRSNew["PACKET_ID"] = DRowFound["PACKET_ID"];
                                    DRSNew["PARTYSTOCKNO"] = DRowFound["PARTYSTOCKNO"];
                                    DRSNew["STOCK_ID"] = DRowFound["STOCK_ID"];
                                    DRSNew["GIACONTROLNO"] = DRowFound["GIACONTROLNO"];
                                    DRSNew["RFIDTAGNO"] = DRowFound["RFIDTAGNO"];
                                    DRSNew["PROCESS_ID"] = DRowFound["PROCESS_ID"];
                                    DRSNew["BOX_ID"] = DRowFound["BOX_ID"];
                                    DRSNew["BOXNAME"] = DRowFound["BOXNAME"];
                                    DRSNew["CARAT"] = DRowFound["CARAT"];
                                    DRSNew["PROCESSNAME"] = DRowFound["PROCESSNAME"];
                                    DRSNew["SERIALNO"] = DRowFound["SERIALNO"];
                                    DTabScan.Rows.Add(DRSNew);
                                    DTabScan.AcceptChanges();
                                }
                            }

                            if (DRowNotFound != null && ISExists == false)
                            {
                                //Global.Message("Start Insert Data Into Stock Tally..");
                                TrnStockTallyProperty Property = new TrnStockTallyProperty();
                                Property.STOCKTALLYDATE = Val.SqlDate(DTPTransferDate.Value.ToShortDateString());
                                Property.PACKET_ID = Guid.Parse(Val.ToString(DRowNotFound["PACKET_ID"]));
                                Property.PARTYSTOCKNO = Val.ToString(DRowNotFound["PARTYSTOCKNO"]);
                                Property.STOCK_ID = Guid.Parse(Val.ToString(DRowNotFound["STOCK_ID"]));
                                Property.RFIDTAGNO = Val.ToString(DRowNotFound["RFIDTAGNO"]);
                                Property.BOX_ID = Val.ToInt32(DRowNotFound["BOX_ID"]);
                                Property.CARAT = Val.Val(DRowNotFound["CARAT"]);
                                Property.FOUNDSTATUS = "FOUND";
                                Property.STATUS = Val.ToString(DRowNotFound["PROCESSNAME"]);
                                Property.SERIALNO = Val.ToInt32(DRowNotFound["SERIALNO"]);
                                Property = ObjTrn.Save(Property);
                                if (Property.ReturnMessageType == "SUCCESS")
                                {
                                    DataRow DRNew = DTabFound.NewRow();

                                    DRNew["ISFOUND"] = false;
                                    DRNew["PACKET_ID"] = DRowNotFound["PACKET_ID"];
                                    DRNew["PARTYSTOCKNO"] = DRowNotFound["PARTYSTOCKNO"];
                                    DRNew["STOCK_ID"] = DRowNotFound["STOCK_ID"];
                                    DRNew["RFIDTAGNO"] = DRowNotFound["RFIDTAGNO"];
                                    DRNew["BOX_ID"] = DRowNotFound["BOX_ID"];
                                    DRNew["BOXNAME"] = DRowNotFound["BOXNAME"];
                                    DRNew["PROCESS_ID"] = DRowNotFound["PROCESS_ID"];
                                    DRNew["CARAT"] = DRowNotFound["CARAT"];
                                    DRNew["PROCESSNAME"] = DRowNotFound["PROCESSNAME"];
                                    DRNew["SERIALNO"] = DRowNotFound["SERIALNO"];
                                    DTabFound.Rows.Add(DRNew);

                                    DTabFound.AcceptChanges();

                                    //hinal 07-02-2022
                                    DataRow DRSNew = DTabScan.NewRow();

                                    DRSNew["ISFOUND"] = true;
                                    DRSNew["PACKET_ID"] = DRowNotFound["PACKET_ID"];
                                    DRSNew["PARTYSTOCKNO"] = DRowNotFound["PARTYSTOCKNO"];
                                    DRSNew["STOCK_ID"] = DRowNotFound["STOCK_ID"];
                                    DRSNew["GIACONTROLNO"] = DRowNotFound["GIACONTROLNO"];
                                    DRSNew["RFIDTAGNO"] = DRowNotFound["RFIDTAGNO"];
                                    DRSNew["PROCESS_ID"] = DRowNotFound["PROCESS_ID"];
                                    DRSNew["BOX_ID"] = DRowNotFound["BOX_ID"];
                                    DRSNew["BOXNAME"] = DRowNotFound["BOXNAME"];
                                    DRSNew["CARAT"] = DRowNotFound["CARAT"];
                                    DRSNew["PROCESSNAME"] = DRowNotFound["PROCESSNAME"];
                                    DRSNew["SERIALNO"] = DRowNotFound["SERIALNO"];
                                    DTabScan.Rows.Add(DRSNew);

                                    DTabNotFound.Rows[IntI].Delete();
                                    DTabNotFound.AcceptChanges();
                                    DTabScan.AcceptChanges();
                                }

                            }
                            else if (ISExists == true)
                            {
                                // Global.Message("Exists is true..");
                            }
                            else if (DRowNotFound == null)
                            {
                                //Global.Message("DRowNotFound is Null..");

                                bool Matched = false;
                                foreach (DataRow DrExtra in DTabExtra.Rows)
                                {
                                    if (StrRFIDTagNo[i].ToString().Trim() == Val.ToString(DrExtra["RFIDTAGNO"]).Trim())
                                    {
                                        Matched = true;
                                        break;
                                    }
                                }
                                if (Matched == false)
                                {
                                    //DataTable DtPartyStockNo = ObjTrn.GetPartyStockNofromRFIDTagNo(StrRFIDTagNo[i].ToString().Trim()); //#P : 25-04-2022
                                    DataTable DtPartyStockNo = ObjTrn.GetPartyStockNofromStockNo("", StrRFIDTagNo[i].ToString().Trim());

                                    TrnStockTallyProperty Property = new TrnStockTallyProperty();
                                    Property.STOCKTALLYDATE = Val.SqlDate(DTPTransferDate.Value.ToShortDateString());
                                    if (DtPartyStockNo.Rows.Count > 0)
                                    {
                                        Property.PARTYSTOCKNO = DtPartyStockNo.Rows[0]["PARTYSTOCKNO"].ToString();
                                        Property.STATUS = DtPartyStockNo.Rows[0]["PROCESSNAME"].ToString();
                                        Property.PROCESS_ID = Val.ToInt32(DtPartyStockNo.Rows[0]["PROCESS_ID"].ToString());
                                        Property.BOX_ID = Val.ToInt32(DtPartyStockNo.Rows[0]["BOX_ID"]);
                                        Property.BOXNAME = Val.ToString(DtPartyStockNo.Rows[0]["BOXNAME"]);
                                        Property.STOCK_ID = Guid.Parse(Val.ToString(DtPartyStockNo.Rows[0]["STOCK_ID"]));
                                        Property.PACKET_ID = Guid.Parse(Val.ToString(DtPartyStockNo.Rows[0]["PACKET_ID"]));
                                        Property.CARAT = Val.ToDouble(DtPartyStockNo.Rows[0]["CARAT"]);
                                        Property.SERIALNO = Val.ToInt32(DtPartyStockNo.Rows[0]["SERIALNO"]);
                                        Property.RFIDTAGNO = Val.ToString(DtPartyStockNo.Rows[0]["RFIDTAGNO"]);
                                    }
                                    else
                                    {
                                        Property.PARTYSTOCKNO = "";
                                        Property.STATUS = "";
                                        Property.STOCK_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                                        Property.PACKET_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                                        Property.CARAT = 0.00;
                                        Property.PROCESS_ID = 0;
                                        Property.SERIALNO = 0;
                                        Property.BOX_ID = 0;
                                        Property.BOXNAME = "";
                                        Property.GIACONTROLNO = "";
                                        Property.RFIDTAGNO = "";
                                    }

                                    Property.RFIDTAGNO = Val.ToString(StrRFIDTagNo[i].ToString().Trim());
                                    Property.FOUNDSTATUS = "EXTRA";

                                    Property = ObjTrn.Save(Property);
                                    if (Property.ReturnMessageType == "SUCCESS")
                                    {
                                        DataRow DRNew = DTabExtra.NewRow();
                                        DRNew["ISFOUND"] = false;
                                        DRNew["PARTYSTOCKNO"] = Property.PARTYSTOCKNO;
                                        DRNew["RFIDTAGNO"] = StrRFIDTagNo[i].ToString().Trim();
                                        DRNew["PROCESSNAME"] = Property.STATUS;
                                        DRNew["PROCESS_ID"] = Property.PROCESS_ID;
                                        DRNew["SERIALNO"] = Property.SERIALNO;
                                        DRNew["CARAT"] = Property.CARAT;
                                        DRNew["BOX_ID"] = Property.BOX_ID;
                                        DRNew["BOXNAME"] = Property.BOXNAME;
                                        DTabExtra.Rows.Add(DRNew);
                                        DTabExtra.AcceptChanges();

                                        DataRow DRSNew = DTabScan.NewRow();
                                        DRSNew["ISFOUND"] = true;
                                        //DRSNew["PACKET_ID"] = DRowNotFound["PACKET_ID"];
                                        DRSNew["PARTYSTOCKNO"] = Property.PARTYSTOCKNO;
                                        DRSNew["STOCK_ID"] = Property.STOCK_ID;
                                        //DRSNew["GIACONTROLNO"] = DRowNotFound["GIACONTROLNO"];
                                        DRSNew["RFIDTAGNO"] = StrRFIDTagNo[i].ToString().Trim();
                                        DRSNew["PROCESS_ID"] = Property.PROCESS_ID;
                                        DRSNew["BOX_ID"] = Property.BOX_ID;
                                        DRSNew["BOXNAME"] = Property.BOXNAME;
                                        DRSNew["CARAT"] = Property.CARAT;
                                        DRSNew["PROCESSNAME"] = Property.STATUS;
                                        DRSNew["SERIALNO"] = Property.SERIALNO;
                                        DTabScan.Rows.Add(DRSNew);
                                        DTabScan.AcceptChanges();
                                    }
                                }
                            }

                            if (GrdDetFound.RowCount > 1)
                            {
                                GrdDetFound.FocusedRowHandle = GrdDetFound.RowCount - 1;
                            }
                            if (GrdDetExtra.RowCount > 1)
                            {
                                GrdDetExtra.FocusedRowHandle = GrdDetExtra.RowCount - 1;
                            }

                        }
                        this.Cursor = Cursors.Default;
                    }
                    catch (Exception ex)
                    {
                        Global.Message(ex.Message.ToString());
                        this.Cursor = Cursors.Default;
                    }
                });

            }
        }

        private void FrmStockTally_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeRFIDObject();
        }

        private void txtTag_Validating(object sender, CancelEventArgs e)
        {
            if (txtKapanName.Text.Trim().Length == 0)
            {
                return;
            }
            if (Val.ToInt(txtPacketNo.Text) == 0)
            {
                txtKapanName.Focus();
                return;
            }
            if (txtTag.Text.Trim().Length == 0)
            {
                txtKapanName.Focus();
                return;
            }

            if (Val.ISNumeric(txtTag.Text) == true)
            {
                Char c = (Char)(64 + Val.ToInt(txtTag.Text));
                txtTag.Text = c.ToString();
            }
            string StrPARTYSTOCKNO = txtKapanName.Text + '-' + txtPacketNo.Text + txtTag.Text;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataRow DRowNotFound = null;
                int IntI = 0;
                for (IntI = 0; IntI < DTabNotFound.Rows.Count; IntI++)
                {
                    DataRow DR = DTabNotFound.Rows[IntI];
                    if (StrPARTYSTOCKNO == Val.ToString(DR["PARTYSTOCKNO"]).Trim())
                    {
                        DRowNotFound = DR;
                        break;
                    }
                }

                bool ISExists = false;

                foreach (DataRow DRowFound in DTabFound.Rows)
                {
                    if (StrPARTYSTOCKNO == Val.ToString(DRowFound["PARTYSTOCKNO"]).Trim())
                    {
                        ISExists = true;
                    }
                }

                if (DRowNotFound != null && ISExists == false)
                {
                    TrnStockTallyProperty Property = new TrnStockTallyProperty();
                    Property.STOCKTALLYDATE = Val.SqlDate(DTPTransferDate.Value.ToShortDateString());
                    string PacketId = Val.ToString(DRowNotFound["PACKET_ID"]);
                    Property.PACKET_ID = Guid.Parse(Val.ToString(PacketId == "" ? "00000000-0000-0000-0000-000000000000" : DRowNotFound["PACKET_ID"]));
                    Property.PARTYSTOCKNO = Val.ToString(DRowNotFound["PARTYSTOCKNO"]);
                    Property.STOCK_ID = Guid.Parse(Val.ToString(DRowNotFound["STOCK_ID"]));
                    Property.GIACONTROLNO = Val.ToString(DRowNotFound["GIACONTROLNO"]);
                    Property.CARAT = Val.Val(DRowNotFound["CARAT"]);
                    Property.FOUNDSTATUS = "FOUND";
                    Property.STATUS = Val.ToString(DRowNotFound["PROCESSNAME"]);
                    Property.SERIALNO = Val.ToInt32(DRowNotFound["SERIALNO"]);
                    Property = ObjTrn.Save(Property);
                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        DataRow DRNew = DTabFound.NewRow();

                        DRNew["ISFOUND"] = true;
                        DRNew["PACKET_ID"] = DRowNotFound["PACKET_ID"];
                        DRNew["PARTYSTOCKNO"] = DRowNotFound["PARTYSTOCKNO"];
                        DRNew["STOCK_ID"] = DRowNotFound["STOCK_ID"];
                        DRNew["GIACONTROLNO"] = DRowNotFound["GIACONTROLNO"];
                        DRNew["PROCESS_ID"] = DRowNotFound["PROCESS_ID"];
                        DRNew["CARAT"] = DRowNotFound["CARAT"];
                        DRNew["PROCESSNAME"] = DRowNotFound["PROCESSNAME"];
                        DRNew["SERIALNO"] = DRowNotFound["SERIALNO"];
                        DTabFound.Rows.Add(DRNew);

                        DTabNotFound.Rows[IntI].Delete();
                        DTabNotFound.AcceptChanges();
                        DTabFound.AcceptChanges();
                    }

                }
                else if (DRowNotFound == null)
                {
                    bool Matched = false;
                    foreach (DataRow DrExtra in DTabExtra.Rows)
                    {
                        if (StrPARTYSTOCKNO == Val.ToString(DrExtra["PARTYSTOCKNO"]).Trim())
                        {
                            Matched = true;
                            break;
                        }
                    }
                    if (Matched == false)
                    {
                        DataTable DtPartyStockNo = ObjTrn.GetPartyStockNofromStockNo(StrPARTYSTOCKNO, "");

                        TrnStockTallyProperty Property = new TrnStockTallyProperty();
                        Property.STOCKTALLYDATE = Val.SqlDate(DTPTransferDate.Value.ToShortDateString());
                        if (DtPartyStockNo.Rows.Count > 0)
                        {
                            Property.PARTYSTOCKNO = DtPartyStockNo.Rows[0]["PARTYSTOCKNO"].ToString();
                            Property.STATUS = DtPartyStockNo.Rows[0]["PROCESSNAME"].ToString();
                            Property.PROCESS_ID = Val.ToInt32(DtPartyStockNo.Rows[0]["PROCESS_ID"].ToString());
                            Property.STOCK_ID = Guid.Parse(Val.ToString(DtPartyStockNo.Rows[0]["STOCK_ID"]));
                            Property.PACKET_ID = Guid.Parse(Val.ToString(DtPartyStockNo.Rows[0]["PACKET_ID"]));
                            Property.CARAT = Val.ToDouble(DtPartyStockNo.Rows[0]["CARAT"]);
                            Property.GIACONTROLNO = Val.ToString(DtPartyStockNo.Rows[0]["GIACONTROLNO"]);
                            Property.SERIALNO = Val.ToInt32(DtPartyStockNo.Rows[0]["SERIALNO"]);
                        }
                        else
                        {
                            Property.PARTYSTOCKNO = "";
                            Property.STATUS = "";
                            Property.STOCK_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                            Property.PACKET_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                            Property.CARAT = 0.00;
                            Property.PROCESS_ID = 0;
                            Property.GIACONTROLNO = "";
                            Property.SERIALNO = 0;
                        }

                        Property.FOUNDSTATUS = "EXTRA";
                        Property = ObjTrn.Save(Property);

                        if (Property.ReturnMessageType == "SUCCESS")
                        {
                            DataRow DRNew = DTabExtra.NewRow();

                            DRNew["ISFOUND"] = false;
                            DRNew["PARTYSTOCKNO"] = Property.PARTYSTOCKNO;
                            DRNew["GIACONTROLNO"] = Property.GIACONTROLNO;
                            DRNew["PROCESSNAME"] = Property.STATUS;
                            DRNew["PROCESS_ID"] = Property.PROCESS_ID;
                            DRNew["SERIALNO"] = Property.SERIALNO;
                            DRNew["CARAT"] = Property.CARAT;


                            DTabExtra.Rows.Add(DRNew);
                            DTabExtra.AcceptChanges();
                        }
                    }
                }

                if (GrdDetFound.RowCount > 1)
                {
                    GrdDetFound.FocusedRowHandle = GrdDetFound.RowCount - 1;
                }
                if (GrdDetExtra.RowCount > 1)
                {
                    GrdDetExtra.FocusedRowHandle = GrdDetExtra.RowCount - 1;
                }
                txtKapanName.Text = string.Empty;
                txtPacketNo.Text = string.Empty;
                txtTag.Text = string.Empty;
                txtKapanName.Focus();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void txtTag_Leave(object sender, EventArgs e)
        {
            txtKapanName.Focus();
        }

        private void ChkBxFound_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < GrdDetFound.RowCount; i++)
                GrdDetFound.SetRowCellValue(i, "ISFOUND", ChkBxFound.Checked);
        }

        private void ChkBxExtra_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < GrdDetExtra.RowCount; i++)
                GrdDetExtra.SetRowCellValue(i, "ISFOUND", ChkBxExtra.Checked);
        }

        private void GrdDetExtra_RowStyle(object sender, RowStyleEventArgs e)
        {

            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                string StrCol = Val.ToString(GrdDetExtra.GetRowCellValue(e.RowHandle, "PROCESSNAME"));
                if (StrCol.ToUpper() == "NONE")
                {
                    e.Appearance.BackColor = lblNone.BackColor;
                    e.Appearance.BackColor2 = lblNone.BackColor;
                }
                else if (StrCol.ToUpper() == "MEMO")
                {
                    e.Appearance.BackColor = lblMemo.BackColor;
                    e.Appearance.BackColor2 = lblMemo.BackColor;
                }
                if (StrCol.ToUpper() == "SOLD")
                {
                    e.Appearance.BackColor = lblSold.BackColor;
                    e.Appearance.BackColor2 = lblSold.BackColor;
                }
                else if (StrCol.ToUpper() == "DELIVERY")
                {
                    e.Appearance.BackColor = lblInvoice.BackColor;
                    e.Appearance.BackColor2 = lblInvoice.BackColor;
                }
                else if (StrCol.ToUpper() == "PURCHASE-RETURN")
                {
                    e.Appearance.BackColor = lblPurchaseReturn.BackColor;
                    e.Appearance.BackColor2 = lblPurchaseReturn.BackColor;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtStockNo_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (txtStockNo.Text.Trim().Length == 0)
                {
                    //txtStockNo.Focus();
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                string StrPacketNo = "";

                StrPacketNo = Val.ToString(txtStockNo.Text);
                if(StrPacketNo.Contains("/"))
                {
                    StrPacketNo = StrPacketNo.Replace("/","");
                }
                if (StrPacketNo.Contains("\""))
                {
                    StrPacketNo = StrPacketNo.Replace("\"", "");
                }
                if (StrPacketNo.Contains("\\"))
                {
                    StrPacketNo = StrPacketNo.Replace("\\", "");
                }

                DataRow DRowNotFound = null;
                int IntI = 0;
                for (IntI = 0; IntI < DTabNotFound.Rows.Count; IntI++)
                {
                    DataRow DR = DTabNotFound.Rows[IntI];
                    DR["BOXNAME"] = Val.ToString(txtStockBox.Text);
                    if (StrPacketNo == Val.ToString(DR["PARTYSTOCKNO"]).Trim())
                    {
                        DRowNotFound = DR;
                        break;
                    }
                }
                DTabNotFound.AcceptChanges();

                bool ISExists = false;

                foreach (DataRow DRowFound in DTabFound.Rows)
                {
                    if (StrPacketNo == Val.ToString(DRowFound["PARTYSTOCKNO"]).Trim())
                    {
                        ISExists = true;
                        txtStockNo.Text = "";
                        txtStockNo.Focus();
                    }
                }

                if (DRowNotFound != null && ISExists == false)
                {
                    TrnStockTallyProperty Property = new TrnStockTallyProperty();
                    Property.STOCKTALLYDATE = Val.SqlDate(DTPTransferDate.Value.ToShortDateString());
                    //string PacketId = Val.ToString(DRowNotFound["PACKET_ID"]);
                    //Property.PACKET_ID = Guid.Parse(Val.ToString(PacketId == "" ? "00000000-0000-0000-0000-000000000000" : DRowNotFound["PACKET_ID"]));
                    Property.PARTYSTOCKNO = Val.ToString(DRowNotFound["PARTYSTOCKNO"]);
                    Property.STOCK_ID = Guid.Parse(Val.ToString(DRowNotFound["STOCK_ID"]));
                    Property.GIACONTROLNO = Val.ToString(DRowNotFound["GIACONTROLNO"]);
                    Property.RFIDTAGNO = Val.ToString(DRowNotFound["RFIDTAGNO"]);
                    Property.BOXNAME = Val.ToString(DRowNotFound["BOXNAME"]);
                    Property.CARAT = Val.Val(DRowNotFound["CARAT"]);
                    Property.FOUNDSTATUS = "FOUND";
                    Property.STATUS = Val.ToString(DRowNotFound["PROCESSNAME"]);
                    Property.SERIALNO = Val.ToInt32(DRowNotFound["SERIALNO"]);
                    Property.MAINCATAGORY = Val.ToString(DRowNotFound["MAINCATAGORY"]);

                    Property = ObjTrn.Save(Property);
                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        DataRow DRNew = DTabFound.NewRow();

                        DRNew["ISFOUND"] = true;
                        DRNew["PACKET_ID"] = DRowNotFound["PACKET_ID"];
                        DRNew["PARTYSTOCKNO"] = DRowNotFound["PARTYSTOCKNO"];
                        DRNew["STOCK_ID"] = DRowNotFound["STOCK_ID"];
                        DRNew["GIACONTROLNO"] = DRowNotFound["GIACONTROLNO"];
                        DRNew["RFIDTAGNO"] = DRowNotFound["RFIDTAGNO"];
                        DRNew["PROCESS_ID"] = DRowNotFound["PROCESS_ID"];
                        DRNew["BOXNAME"] = DRowNotFound["BOXNAME"];
                        DRNew["CARAT"] = DRowNotFound["CARAT"];
                        DRNew["PROCESSNAME"] = DRowNotFound["PROCESSNAME"];
                        DRNew["SERIALNO"] = DRowNotFound["SERIALNO"];
                        DRNew["MAINCATAGORY"] = DRowNotFound["MAINCATAGORY"];

                        DTabFound.Rows.Add(DRNew);

                        //DTabNotFound.Rows[IntI].Delete();
                        //DTabNotFound.AcceptChanges();
                        DTabFound.AcceptChanges();

                        //hinal 16-01-2022
                        DataRow DRSNew = DTabScan.NewRow();

                        DRSNew["ISFOUND"] = true;
                        DRSNew["PACKET_ID"] = DRowNotFound["PACKET_ID"];
                        DRSNew["PARTYSTOCKNO"] = DRowNotFound["PARTYSTOCKNO"];
                        DRSNew["STOCK_ID"] = DRowNotFound["STOCK_ID"];
                        DRSNew["GIACONTROLNO"] = DRowNotFound["GIACONTROLNO"];
                        DRSNew["RFIDTAGNO"] = DRowNotFound["RFIDTAGNO"];
                        DRSNew["PROCESS_ID"] = DRowNotFound["PROCESS_ID"];
                        DRSNew["BOXNAME"] = DRowNotFound["BOXNAME"];
                        DRSNew["CARAT"] = DRowNotFound["CARAT"];
                        DRSNew["PROCESSNAME"] = DRowNotFound["PROCESSNAME"];
                        DRSNew["SERIALNO"] = DRowNotFound["SERIALNO"];
                        DRNew["MAINCATAGORY"] = DRowNotFound["MAINCATAGORY"];

                        DTabScan.Rows.Add(DRSNew);

                        DTabNotFound.Rows[IntI].Delete();
                        DTabNotFound.AcceptChanges();
                        DTabScan.AcceptChanges();
                    }

                }
                else if (DRowNotFound == null)
                {
                    bool Matched = false;
                    foreach (DataRow DrExtra in DTabExtra.Rows)
                    {
                        if (txtStockNo.Text == Val.ToString(DrExtra["PARTYSTOCKNO"]).Trim())
                        {
                            Matched = true;
                            break;
                        }
                    }
                    if (Matched == false)
                    {
                        DataTable DtPartyStockNo = ObjTrn.GetPartyStockNofromStockNo(txtStockNo.Text, "");

                        TrnStockTallyProperty Property = new TrnStockTallyProperty();
                        Property.STOCKTALLYDATE = Val.SqlDate(DTPTransferDate.Value.ToShortDateString());
                        if (DtPartyStockNo.Rows.Count > 0)
                        {
                            Property.PARTYSTOCKNO = DtPartyStockNo.Rows[0]["PARTYSTOCKNO"].ToString();
                            Property.STATUS = DtPartyStockNo.Rows[0]["PROCESSNAME"].ToString();
                            Property.PROCESS_ID = Val.ToInt32(DtPartyStockNo.Rows[0]["PROCESS_ID"].ToString());
                            Property.STOCK_ID = Guid.Parse(Val.ToString(DtPartyStockNo.Rows[0]["STOCK_ID"]));
                            Property.PACKET_ID = Guid.Parse(Val.ToString(DtPartyStockNo.Rows[0]["PACKET_ID"]));
                            Property.BOX_ID = Val.ToInt32(DtPartyStockNo.Rows[0]["BOX_ID"]);
                            Property.BOXNAME = Val.ToString(DtPartyStockNo.Rows[0]["BOXNAME"]);
                            Property.CARAT = Val.ToDouble(DtPartyStockNo.Rows[0]["CARAT"]);
                            Property.GIACONTROLNO = Val.ToString(DtPartyStockNo.Rows[0]["GIACONTROLNO"]);
                            Property.RFIDTAGNO = Val.ToString(DtPartyStockNo.Rows[0]["RFIDTAGNO"]);
                            Property.SERIALNO = Val.ToInt32(DtPartyStockNo.Rows[0]["SERIALNO"]);
                        }
                        else
                        {
                            Property.PARTYSTOCKNO = txtStockNo.Text;
                            Property.STATUS = "";
                            Property.STOCK_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                            Property.PACKET_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                            Property.CARAT = 0.00;
                            Property.PROCESS_ID = 0;
                            Property.BOX_ID = 0;
                            Property.BOXNAME = "";
                            Property.GIACONTROLNO = "";
                            Property.RFIDTAGNO = "";
                            Property.SERIALNO = 0;
                        }

                        Property.FOUNDSTATUS = "EXTRA";
                        Property = ObjTrn.Save(Property);

                        if (Property.ReturnMessageType == "SUCCESS")
                        {
                            DataRow DRNew = DTabExtra.NewRow();

                            DRNew["PACKET_ID"] = Property.PACKET_ID;
                            DRNew["STOCK_ID"] = Property.STOCK_ID;
                            DRNew["ISFOUND"] = false;
                            DRNew["PARTYSTOCKNO"] = Property.PARTYSTOCKNO;
                            DRNew["GIACONTROLNO"] = Property.GIACONTROLNO;
                            DRNew["RFIDTAGNO"] = Property.RFIDTAGNO;
                            DRNew["BOX_ID"] = Property.BOX_ID;
                            DRNew["BOXNAME"] = Property.BOXNAME;
                            DRNew["PROCESSNAME"] = Property.STATUS;
                            DRNew["PROCESS_ID"] = Property.PROCESS_ID;
                            DRNew["SERIALNO"] = Property.SERIALNO;
                            DRNew["CARAT"] = Property.CARAT;


                            DTabExtra.Rows.Add(DRNew);
                            DTabExtra.AcceptChanges();
                        }
                    }
                }

                if (GrdDetFound.RowCount > 1)
                {
                    GrdDetFound.FocusedRowHandle = GrdDetFound.RowCount - 1;
                }
                if (GrdDetExtra.RowCount > 1)
                {
                    GrdDetExtra.FocusedRowHandle = GrdDetExtra.RowCount - 1;
                }
                txtStockNo.Text = string.Empty;
                txtStockNo.Focus();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void txtBoxID_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "BOXCODE,BOXNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BOX);
                    FrmSearch.mBoolISPostBack = true;
                    FrmSearch.mStrISPostBackColumn = "BOXNAME";
                    FrmSearch.mStrColumnsToHide = "BOX_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBoxID.Text = Val.ToString(FrmSearch.DRow["BOXNAME"]);
                        txtBoxID.Tag = Val.ToString(FrmSearch.DRow["BOX_ID"]);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }

            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }

        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = DTabScan.Clone();
                if (RbtScan.Checked == true)
                {
                    foreach (DataRow dr in DTabScan.Rows)
                    {
                        if (Val.ToBoolean(dr["ISFOUND"].ToString()) == true)
                            DTab.Rows.Add(dr.ItemArray);
                    }
                }
                //if (RbtAllStock.Checked == true)
                //{
                //    foreach (DataRow dr in DTabFound.Rows)
                //    {
                //        if (Val.ToBoolean(dr["ISFOUND"].ToString()) == true)
                //            DTab.Rows.Add(dr.ItemArray);
                //    }
                //}
                if (RbtNotFoundStock.Checked == true)
                {
                    foreach (DataRow dr in DTabExtra.Rows)
                    {
                        if (Val.ToBoolean(dr["ISFOUND"].ToString()) == true)
                            DTab.Rows.Add(dr.ItemArray);
                    }
                }

                if (DTab.Rows.Count <= 0)
                {
                    return;
                }
                string ParameterUpdateXml;

                using (StringWriter sw = new StringWriter())
                {
                    DTab.WriteXml(sw);
                    ParameterUpdateXml = sw.ToString();
                }

                string str = ObjTrn.UpdateBoxID(ParameterUpdateXml, Val.ToInt32(txtBoxID.Tag), txtBoxID.Text, Val.SqlDate(DTPTransferDate.Value.ToShortDateString()));

                //Global.Message(str);
                if (str != "SUCCESS")
                {
                    Global.Message("Something Goes Wrong...");
                }
                else
                {
                    BtnShow_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            DTabScan.Rows.Clear();
            foreach (DataRow dr in DTabFound.Rows)
            {
                if (Val.ToBoolean(dr["ISFOUND"].ToString()) == true)
                    dr["ISFOUND"] = false;
            }
            DTabFound.AcceptChanges();
            txtBoxID.Text = string.Empty;
            txtBoxID.Tag = string.Empty;

            txtStockNo.Text = string.Empty;
            txtStockNo.Tag = string.Empty;
        }

        private void ChkAllFoundStock_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < GrdDetFound.RowCount; i++)
                GrdDetFound.SetRowCellValue(i, "ISFOUND", ChkAllFoundStock.Checked);
        }

        private void ChkAllScanStock_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < GrdScan.RowCount; i++)
                GrdScan.SetRowCellValue(i, "ISFOUND", ChkAllScanStock.Checked);
        }

        private void txtStockBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               
            }
        }

        private void txtStockBox_Validating(object sender, CancelEventArgs e)
        {
            txtStockNo.Focus();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnRefreshStock_Click(object sender, EventArgs e)
        {

            if (Global.Confirm("Are You Sure For Delete Current StockTally And Refresh with New?") == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            string StockType = "";
            if (RbtNatural.Checked == true)
            {
                StockType = RbtNatural.Text;
            }
            else
            {
                StockType = RbtLabGrown.Text;
            }
            TrnStockTallyProperty Property = new TrnStockTallyProperty();
            Property.STOCKTALLYDATE = Val.SqlDate(DTPTransferDate.Value.ToShortDateString());
            Property.MAINCATAGORY = StockType;

            Property.FOUNDSTATUS = "FOUND";
            Property = ObjTrn.Delete(Property);
            this.Cursor = Cursors.Default;
            //Global.Message(Property.ReturnMessageDesc);
            if (Property.ReturnMessageType == "SUCCESS")
            {
                DTabExtra.Rows.Clear();
                DTabFound.Rows.Clear();
                DTabNotFound.Rows.Clear();
                DTabScan.Rows.Clear();

                BtnShow_Click(null, null);
            }
        }

        private void DTPTransferDate_ValueChanged(object sender, EventArgs e)
        {
            if ((DTPTransferDate.Text) == DateTime.Now.ToShortDateString())
            {
                BtnRefreshStock.Enabled = true;
            }
            else
            {
                BtnRefreshStock.Enabled = false;
            }
            //Global.Message(DateTime.Now.ToShortDateString());
        }


    }
}
