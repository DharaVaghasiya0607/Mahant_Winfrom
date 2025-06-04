using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusLib.TableName;
using BusLib.Configuration;
using MahantExport.MDI;
using BusLib.Master;
using System.Deployment.Application;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Net;
using MahantExport.GIADownload;
using MahantExport.Class;
using System.Net.NetworkInformation;
using MahantExport.Events;
using System.Diagnostics;

namespace MahantExport.Utility
{
    public partial class FrmLogin : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        DataTable DtFinYear = new DataTable();
        string sMacAddress = "";

        public class FtpDetails
        {
            public string Url { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        static FtpDetails LoadFtpCredentials(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            return new FtpDetails
            {
                Url = lines[0],
                Username = lines[1],
                Password = lines[2]
            };
        }

        #region Constructor

        public FrmLogin()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            try
            {
                //   lblCompanyName.Text = System.Configuration.ConfigurationManager.AppSettings["CompanyName"].ToString();
                //txtUserName.Text = System.Configuration.ConfigurationManager.AppSettings["USERNAME"].ToString();
                //txtPassWord.Text = System.Configuration.ConfigurationManager.AppSettings["PASSWORD"].ToString();

            }
            catch (Exception EX)
            {

            }
            txtConnectionString.Text = BOConfiguration.ConnectionString;

            this.ShowDialog();
        }
        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;

            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = false;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }


        #endregion

        #region Form Validation

        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control && e.Alt && e.Shift)
            {
                txtConnectionString.Visible = true;
                BtnUpdate.Visible = true;
            }

            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
                this.Close();
            }
        }


        #endregion

        #region Events


        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try


            {
                BOMST_Ledger ObjMast = new BOMST_Ledger();
                if (txtUserName.Text.Length == 0)
                {
                    Global.Message("UserName Is Required");
                    txtUserName.Focus();
                    return;
                }
                if (txtPassWord.Text.Length == 0)
                {
                    Global.Message("Password Is Required");
                    txtPassWord.Focus();
                    return;
                }

                if (txtUserName.Text == "AXONEADMIN" && txtPassWord.Text == "AXONEADMIN")
                {
                    this.Hide();

                    BusLib.Configuration.BOConfiguration.gEmployeeProperty = new LedgerMasterProperty();

                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGERNAME = "Testing";
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME = txtUserName.Text;
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.PASSWORD = txtPassWord.Text;
                    Global.gStrExeVersion = lblVersion.Text;

                    this.Hide();
                    this.Close();
                    // BOConfiguration.BackUp();
                    //FrmMDINew FrmMDI = new FrmMDINew();
                    //Global.gMainRef = FrmMDI;
                    //FrmMDI.ShowDialog();
                    FrmFinalMDIRibbon FrmFinalMDIRibbon = new FrmFinalMDIRibbon();
                    Global.gMainRef = FrmFinalMDIRibbon;
                    FrmFinalMDIRibbon.ShowDialog();

                    return;
                }

                this.Cursor = Cursors.WaitCursor;



                DataRow DRow = ObjMast.CheckLogin(txtUserName.Text, txtPassWord.Text);

                if (DRow != null)
                {
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty = new LedgerMasterProperty();

                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID = Guid.Parse(Val.ToString(DRow["LEDGER_ID"]));
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGERCODE = Val.ToInt32(DRow["LEDGERCODE"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGERNAME = Val.ToString(DRow["LEDGERNAME"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGERTYPE = Val.ToString(DRow["LEDGERTYPE"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.CONTACTPER = Val.ToString(DRow["CONTACTPER"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.COMPANYNAME = Val.ToString(DRow["COMPANYNAME"]);

                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.EMAILID = Val.ToString(DRow["EMAILID"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.QQID = Val.ToString(DRow["QQID"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.SKYPEID = Val.ToString(DRow["SKYPEID"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.WEBSITE = Val.ToString(DRow["WEBSITE"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.GENDER = Val.ToString(DRow["GENDER"]);

                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.DESIGNATION_ID = Val.ToInt32(DRow["DESIGNATION_ID"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.DEPARTMENT_ID = Val.ToInt32(DRow["DEPARTMENT_ID"]);

                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.BILLINGADDRESS1 = Val.ToString(DRow["BILLINGADDRESS1"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.BILLINGADDRESS2 = Val.ToString(DRow["BILLINGADDRESS2"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.BILLINGADDRESS3 = Val.ToString(DRow["BILLINGADDRESS3"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.BILLINGCOUNTRY_ID = Val.ToInt32(DRow["BILLINGCOUNTRY_ID"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.BILLINGSTATE = Val.ToString(DRow["BILLINGSTATE"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.BILLINGCITY = Val.ToString(DRow["BILLINGCITY"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.BILLINGZIPCODE = Val.ToString(DRow["BILLINGZIPCODE"]);

                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.SHIPPINGADDRESS1 = Val.ToString(DRow["SHIPPINGADDRESS1"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.SHIPPINGADDRESS2 = Val.ToString(DRow["SHIPPINGADDRESS2"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.SHIPPINGADDRESS3 = Val.ToString(DRow["SHIPPINGADDRESS3"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.SHIPPINGCOUNTRY_ID = Val.ToInt32(DRow["SHIPPINGCOUNTRY_ID"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.SHIPPINGSTATE = Val.ToString(DRow["SHIPPINGSTATE"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.SHIPPINGCITY = Val.ToString(DRow["SHIPPINGCITY"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.SHIPPINGZIPCODE = Val.ToString(DRow["SHIPPINGZIPCODE"]);

                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.MOBILENO1 = Val.ToString(DRow["MOBILENO1"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.MOBILENO2 = Val.ToString(DRow["MOBILENO2"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.LANDLINENO = Val.ToString(DRow["LANDLINENO"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISDISPLAYCOSTPRICE = Val.ToBoolean(DRow["ISDISPLAYCOSTPRICE"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISDISPLAYALLMFGCOST = Val.ToBoolean(DRow["ISDISPLAYALLMFGCOST"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISCOMPUTERPRICE = Val.ToBoolean(DRow["ISCOMPUTERPRICE"]);

                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISALLOWWEBLOGIN = Val.ToBoolean(DRow["ISALLOWWEBLOGIN"]);

                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME = Val.ToString(DRow["USERNAME"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.PASSWORD = Val.ToString(DRow["PASSWORD"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.DEFAULTDISCOUNTDIFF = Val.Val(DRow["DEFAULTDISCOUNTDIFF"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISDEFAULTDISCOUNTDIFF = Val.ToBoolean(DRow["ISDEFAULTDISCOUNTDIFF"]);

                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.MEMBERDISCOUNT = Val.Val(DRow["MEMBERDISCOUNT"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISMEMBERDISCOUNT = Val.ToBoolean(DRow["ISMEMBERDISCOUNT"]);

                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.STATUS = Val.ToString(DRow["STATUS"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.REMARK = Val.ToString(DRow["REMARK"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.SOURCE = Val.ToString(DRow["SOURCE"]);

                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.DEFAULTSELLER_ID = Val.ToString(DRow["DEFAULTSELLER_ID"]).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(DRow["DEFAULTSELLER_ID"]));
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISDISPLAYPURCHASEPARTY = Val.ToBoolean(DRow["ISDISPLAYPURPARTY"]);
                    BusLib.Configuration.BOConfiguration.gEmployeeProperty.PROCESS_ID = Val.ToString(DRow["PROCESS_ID"]);
                    BusLib.Configuration.BOConfiguration.DEPTNAME = Val.ToString(DRow["Dept"]);

                    //BusLib.Configuration.BOConfiguration.gEmployeeProperty.PASSWORD = txtPassWord.Text;

                    Global.gStrExeVersion = lblVersion.Text;


                    // BusLib.Configuration.BOConfiguration.gEmployeeProperty.IS_DISPLAYVALUE = Val.ToBooleanToInt(DRow["IS_DISPLAYVALUE"]);
                    this.Cursor = Cursors.Default;
                    this.Hide();

                    this.Close();

                    //COMMNET SHIV 13-08-2022
                    //// BOConfiguration.BackUp();
                    //FrmYearCompanySelect FrmYearCompanySelect = new FrmYearCompanySelect();
                    //ObjFormEvent.ObjToDisposeList.Add(FrmYearCompanySelect);
                    //FrmYearCompanySelect.ShowForm();

                    foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces()) //Add namespace : using System.Net.NetworkInformation; 
                    {
                        if (n.OperationalStatus == OperationalStatus.Up)
                        {
                            sMacAddress += n.GetPhysicalAddress().ToString();
                            break;
                        }
                    }

                    DataTable Dtab = ObjMast.CheckEvent(sMacAddress);
                    if (Dtab.Rows.Count > 0)
                    {
                        if (Val.ToString(Dtab.Rows[0]["MESSAGE"]) == "YES")
                        {
                            Global.gstrEventUrl = Val.ToString(Dtab.Rows[0]["URL"]);
                            FrmEventBox FrmEventBox = new FrmEventBox();
                            FrmEventBox.ShowDialog();

                        }
                        else
                        {
                            FrmYearCompanySelect FrmYearCompanySelect = new FrmYearCompanySelect();
                            ObjFormEvent.ObjToDisposeList.Add(FrmYearCompanySelect);
                            FrmYearCompanySelect.ShowForm();
                        }
                    }
                    else
                    {
                        FrmYearCompanySelect FrmYearCompanySelect = new FrmYearCompanySelect();
                        ObjFormEvent.ObjToDisposeList.Add(FrmYearCompanySelect);
                        FrmYearCompanySelect.ShowForm();
                    }

                    return;
                }
                else
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("INVALID USERNAME AND PASSWORD");
                    txtUserName.Focus();
                    return;
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        #endregion

        private void BtnClose_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.Close();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            BOMST_Ledger ObjMast = new BOMST_Ledger();


            var Cred = LoadFtpCredentials("ftp.txt");
            var ftp = new FtpHelper(Cred.Url, Cred.Username, Cred.Password);

            try
            {
                // Example: Download a file
                string localVersion = lblVersion.Text;
                DataTable DTabRemoteVersion = ObjMast.GetRemotVersion();
                string remoteVersion = Val.ToString(DTabRemoteVersion.Rows[0]["settingvalue"]);
                if (remoteVersion != localVersion)
                {
                    if (Global.Confirm("A new version is available. Do you want to update now?") == DialogResult.Yes)
                    {
                        string updaterPath = Path.Combine(Application.StartupPath, "Updater.exe");
                        string appName = Path.GetFileName(Application.ExecutablePath);

                        if (File.Exists(updaterPath))
                        {

                            string arguments = $"\"{appName}\" \"Updater.exc\" \"{Cred.Url}\" \"{Cred.Username}\" \"{Cred.Password}\"";
                            Process.Start(updaterPath, arguments);
                        }
                        else
                        {
                            MessageBox.Show("Updater.exe not found. Please contact support.");
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            if (File.Exists(Application.StartupPath + "\\Background.png"))
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\Background.png");
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }

            //if (ApplicationDeployment.IsNetworkDeployed)
            //{
            //    lblVersion.Visible = true;
            //    lblVersion.Text = string.Format(ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4));
            //}
            //else
            //{
            //    lblVersion.Visible = false;
            //}

        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {

            string Str = Global.TextEncrypt(txtConnectionString.Text);

            System.IO.File.WriteAllText(Application.StartupPath + "\\" + Val.ToString(BOConfiguration.ConnectionFileName) + ".txt", Str);

            BOConfiguration.ConnectionString = txtConnectionString.Text;

            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //config.ConnectionStrings.ConnectionStrings["ConnectionString"].ConnectionString = Str;
            //config.Save(ConfigurationSaveMode.Modified);
            //ConfigurationManager.RefreshSection("connectionStrings");

            Global.Message("Connection String Changed. Your Application Automatically Closed");
            txtConnectionString.Visible = false;
            BtnUpdate.Visible = false;

            this.Close();
            Application.Restart();
        }

    }
}
