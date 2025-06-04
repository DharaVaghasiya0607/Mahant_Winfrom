using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDK_SC_RFID_Devices;
using TcpIP_class;
using DataClass;
using System.Threading;
using System.Windows.Forms;
namespace BusLib.Transaction
{
    public class BOTRN_RFIDConnection_Old
    {
        private rfidPluggedInfo[] _arrayOfPluggedDevice;
        //private RFID_Device _currentUSBDevice; // (USB) local device instance
        private int _selectedDevice; // (USB) devices comboBox index
        private DeviceInfo _currentEthernetDevice; // (Ethernet) remote device information
        private bool _isEthernetDeviceSPCE2; // (Ethernet) state compatible (or not) with SPCE2 tags

        TcpIpClient tcpClient = new TcpIpClient();

        public string FindDevice()
        {
            MessageBox.Show("Start Find Device"); //Step:1
            _arrayOfPluggedDevice = null;

            MessageBox.Show("Step - 2"); //Step:1

            RFID_Device tmp = new RFID_Device();

            MessageBox.Show("Step - 3"); //Step:1

            _arrayOfPluggedDevice = tmp.getRFIDpluggedDevice(true);

            MessageBox.Show("Step - 4"); //Step:1

            MessageBox.Show("Step - 4.1" + _arrayOfPluggedDevice[0].SerialRFID.ToString() + "");

            tmp.ReleaseDevice();

            string StrDeviceName = "Info : No device detected";
            MessageBox.Show("Step : 5 " + StrDeviceName); //Step:2

            if (_arrayOfPluggedDevice != null)
            {
                MessageBox.Show("Step : 6 " + StrDeviceName); //Step:2

                if (_arrayOfPluggedDevice.Length == 0)
                    return StrDeviceName;

                MessageBox.Show("Step : 7 " + StrDeviceName); //Step:2

                int i = 0;
                foreach (rfidPluggedInfo dev in _arrayOfPluggedDevice)
                {
                    if (i == 0)
                    {
                        StrDeviceName = "Device Name : " + dev.SerialRFID;
                        MessageBox.Show("Step : 3 " + StrDeviceName); //Step:3
                        break;
                    }
                }
            }
            else
            {
                StrDeviceName = "Info : No device detected";
                MessageBox.Show("Step : 8 " + StrDeviceName); //Step:4
            }
            return StrDeviceName;
        }
        public RFID_Device ConnectDevice()
        {
            
            try
            {
                //if (_currentUSBDevice != null)
                //{
                //    if (_currentUSBDevice.ConnectionStatus != ConnectionStatus.CS_Connected)
                //        _currentUSBDevice.ReleaseDevice(); // Release previous object if not connected
                //}

                RFID_Device _currentUSBDevice = new RFID_Device(); // Create a new object 


                // Let's create appropriate smartboard device
                // The task is under a threadpool because scanning all ports (if you decide to do so) makes the UI freeze.
                ThreadPool.QueueUserWorkItem(
                    delegate
                    {
                        //  Give the COM port as second parameter to avoid looking for all com ports again => faster connection.
                        _currentUSBDevice.Create_NoFP_Device(_arrayOfPluggedDevice[_selectedDevice].SerialRFID, _arrayOfPluggedDevice[_selectedDevice].portCom);
                        // Remove the second parameter if you want to force the program to look for all COM ports again
                        if (_currentUSBDevice.get_RFID_Device.FirmwareVersion.StartsWith("1"))
                        {
                            _currentUSBDevice.ReleaseDevice();
                        }
                    });
                return _currentUSBDevice;
            }
            catch
            {
                return null;
            }
            
        }
        public bool StartScan(RFID_Device localDevice, bool bUnlockAll)
        {
            if (localDevice == null) return false;

            if ((localDevice.ConnectionStatus == ConnectionStatus.CS_Connected) &&
                (localDevice.DeviceStatus == DeviceStatus.DS_Ready))
            {
                localDevice.ScanDevice(true, bUnlockAll);
                return true;
            }

            return false;
        }
        public void StopScan(RFID_Device localDevice)
        {
            if (localDevice == null) return;
            if ((localDevice.ConnectionStatus == ConnectionStatus.CS_Connected) &&
                  (localDevice.DeviceStatus == DeviceStatus.DS_InScan))
                localDevice.StopScan();
        }
        public string LedOnAll(RFID_Device localDevice, List<string> tagsList)
        {
            int nbTagToLight = tagsList.Count; // initial number of tags
            localDevice.get_RFID_Device.DeviceBoard.setBridgeState(false, 167, 167);
            localDevice.TestLighting(tagsList);

            string message = String.Format("{0} tags to find : {1} have been found.", nbTagToLight,
                nbTagToLight - tagsList.Count);

            if (tagsList.Count > 0) // some tag UIDs are still in the list : they've not been found
            {
                message += "\nMissing tags ID :";
                message = "";
                foreach (string missingTag in tagsList)
                    //message = String.Format("{0}\n{1}", message, missingTag);
                    if (missingTag != "")
                        message += missingTag + ",";
                if (message.Length > 0)
                    message = message.Remove(message.Length - 1, 1);
            }
            return message;
        }

    }
}
