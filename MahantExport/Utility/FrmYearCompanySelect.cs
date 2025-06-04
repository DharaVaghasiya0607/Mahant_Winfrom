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
using System.Data.SqlClient;

namespace MahantExport.Utility
{
    public partial class FrmYearCompanySelect : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        DataTable DtFinYear = new DataTable();
        DataTable DtCompany = new DataTable();

        #region Constructor

        public FrmYearCompanySelect()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            try
            {
                
                BOMST_Ledger ObjMast = new BOMST_Ledger();
                DtFinYear = ObjMast.FillFinYear(BOConfiguration.DEPTNAME);
                cmbFinincialYear.DataSource = DtFinYear;
                cmbFinincialYear.ValueMember = "YEAR_ID";
                cmbFinincialYear.DisplayMember = "YEARNAME";

                //Shiv Add 04-04-22 Account Person Login
                if (BOConfiguration.DEPTNAME == "ACCOUNT")
                {
                    if (DtFinYear.Rows.Count > 0)
                        cmbFinincialYear.SelectedItem = DtFinYear.Rows[0]["YEARNAME"].ToString();

                    DtCompany = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_ACC_COMPANY);
                    cmbCompany.DataSource = DtCompany;
                    cmbCompany.ValueMember = "PARTY_ID";
                    cmbCompany.DisplayMember = "PARTYNAME";

                    if (DtCompany.Rows.Count > 0)
                        cmbCompany.SelectedItem = DtCompany.Rows[0]["PARTYNAME"].ToString();
                }
                else
                {
                    DtCompany = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BRANCHCOMPANY);
                    cmbCompany.DataSource = DtCompany;
                    cmbCompany.ValueMember = "PARTY_ID";
                    cmbCompany.DisplayMember = "PARTYNAME";

                    if (DtCompany.Rows.Count > 0)
                        cmbCompany.SelectedItem = DtCompany.Rows[0]["PARTYNAME"].ToString();
                }


               
            }
            catch (Exception EX)
            {

            }

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

       
        #region Events

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                BusLib.Configuration.BOConfiguration.FINYEAR_ID = Val.ToInt(cmbFinincialYear.SelectedValue);
                DataRow[] FDRow = DtFinYear.Select("YEAR_ID = '" + BusLib.Configuration.BOConfiguration.FINYEAR_ID + "'");
                if (FDRow.Length != 0)
                {
                    BusLib.Configuration.BOConfiguration.FINYEARNAME = Val.ToString(FDRow[0]["YEARNAME"]);
                    BusLib.Configuration.BOConfiguration.FINYEARSHORTNAME = Val.ToString(FDRow[0]["YEARSHORTNAME"]);

                    this.Cursor = Cursors.WaitCursor;
                    DataTable DTab = new BOMST_Ledger().GetQMaster(BusLib.Configuration.BOConfiguration.FINYEAR_ID);
                    if (DTab.Rows.Count != 0)
                    {
                        SqlConnectionStringBuilder SqlBuid = new SqlConnectionStringBuilder();
                        SqlBuid.DataSource = Global.TextDecrypt(Val.ToString(DTab.Rows[0]["Q1"]));
                        SqlBuid.InitialCatalog = Global.TextDecrypt(Val.ToString(DTab.Rows[0]["Q2"]));
                        SqlBuid.UserID = Global.TextDecrypt(Val.ToString(DTab.Rows[0]["Q3"]));
                        SqlBuid.Password = Global.TextDecrypt(Val.ToString(DTab.Rows[0]["Q4"]));
                        SqlBuid.ConnectTimeout = Val.ToInt(DTab.Rows[0]["Q5"]);

                        BOConfiguration.ConnectionString = SqlBuid.ConnectionString;
                        BOConfiguration.ProviderName = "System.Data.SqlClient";

                    }
                    this.Cursor = Cursors.Default;
                }

                BusLib.Configuration.BOConfiguration.COMPANY_ID = Guid.Parse(Val.ToString(cmbCompany.SelectedValue));
                DataRow[] CDRow = DtCompany.Select("PARTY_ID = '" + BusLib.Configuration.BOConfiguration.COMPANY_ID + "'");
                if (CDRow.Length != 0)
                {
                    BusLib.Configuration.BOConfiguration.COMPANYNAME = Val.ToString(CDRow[0]["PARTYNAME"]);
                    BusLib.Configuration.BOConfiguration.COMPANYCODE = Val.ToString(CDRow[0]["PARTYCODE"]);

                    Global.gStrCompanyName = Val.ToString(CDRow[0]["PARTYNAME"]);

                }

                this.Hide();

                this.Close();

                try
                {
                    FrmFinalMDIRibbon FrmMDI = new FrmFinalMDIRibbon();//Gunjan:22/06/2023
                    Global.gMainRef = FrmMDI;
                    FrmMDI.ShowDialog();
                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
                return;              

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

        private void cmbLoginType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BOMST_Ledger ObjMast = new BOMST_Ledger();

                DtFinYear = ObjMast.FillFinYear("");
                cmbFinincialYear.DataSource = DtFinYear;
                cmbFinincialYear.ValueMember = "YEAR_ID";
                cmbFinincialYear.DisplayMember = "YEARNAME";

                if (DtFinYear.Rows.Count > 0)
                    cmbFinincialYear.SelectedItem = DtFinYear.Rows[0]["YEARNAME"].ToString();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
    }
}
