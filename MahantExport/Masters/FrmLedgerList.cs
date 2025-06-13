using MahantExport.Masters;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MahantExport.Master
{
    public partial class FrmLedgerList : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Ledger ObjLedger = new BOMST_Ledger();
        string StrMesg = "";


        #region Property Settings

        public FrmLedgerList()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            // txtPassword_TextChanged(null, null);
            this.Show();
        }

        public void ShowForm(string pStrTag)
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            
            FrmLedgerNew FrmLedgerNew = new FrmLedgerNew();
            FrmLedgerNew.MdiParent = Global.gMainRef;
            FrmLedgerNew.FormClosing += new FormClosingEventHandler(FrmLedger_FormClosing);
            FrmLedgerNew.ShowForm(pStrTag);
        }



        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjLedger);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion


        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {

                string StrStatus = "";

                if (RbtActive.Checked == true)
                    StrStatus = "ACTIVE";
                else if (RbtDeactive.Checked == true)
                    StrStatus = "DEACTIVE";
                else if (RbtWebPending.Checked == true)
                    StrStatus = "WEB PENDING";
                else
                    StrStatus = "ALL";

                this.Cursor = Cursors.WaitCursor;
                GrdDet.BeginUpdate();

                if (xtraTabActivity.SelectedTabPage.Tag == "PENDING KYC")
                {
                    DataSet Ds = ObjLedger.GetDataForLedgerListForPendingKYC(StrStatus);
                    MainGrd.DataSource = Ds.Tables[0];
                    MainGrd.Refresh();

                    if (Ds.Tables[1].Rows.Count != 0)
                    {
                        RbtAll.Text = "All ( " + Val.ToString(Ds.Tables[1].Rows[0]["ALL"]) + " )";

                        RbtActive.Text = "Active ( " + Val.ToString(Ds.Tables[1].Rows[0]["ACTIVE"]) + " )";

                        RbtDeactive.Text = "De-Active ( " + Val.ToString(Ds.Tables[1].Rows[0]["DEACTIVE"]) + " )";

                        RbtWebPending.Text = "Web Pending ( " + Val.ToString(Ds.Tables[1].Rows[0]["WEBPENDING"]) + " )";
                    }
                }
                else
                {
                    DataSet DS = ObjLedger.GetDataForLedgerList(StrStatus, Val.ToString(xtraTabActivity.SelectedTabPage.Tag));
                    MainGrd.DataSource = DS.Tables[0];
                    MainGrd.Refresh();

                    if (DS.Tables[1].Rows.Count != 0)
                    {
                        RbtAll.Text = "All ( " + Val.ToString(DS.Tables[1].Rows[0]["ALL"]) + " )";

                        RbtActive.Text = "Active ( " + Val.ToString(DS.Tables[1].Rows[0]["ACTIVE"]) + " )";

                        RbtDeactive.Text = "De-Active ( " + Val.ToString(DS.Tables[1].Rows[0]["DEACTIVE"]) + " )";

                        RbtWebPending.Text = "Web Pending ( " + Val.ToString(DS.Tables[1].Rows[0]["WEBPENDING"]) + " )";
                    }
                }

                GrdDet.EndUpdate();
                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnAddParty_Click(object sender, EventArgs e)
        {
            try
            {
                FrmLedgerNew FrmLedgerNew = new FrmLedgerNew();
                FrmLedgerNew.MdiParent = Global.gMainRef;
                FrmLedgerNew.FormClosing += new FormClosingEventHandler(FrmLedger_FormClosing);
                FrmLedgerNew.ShowForm(Val.ToString(xtraTabActivity.SelectedTabPage.Tag));
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }


        private void GrdDet_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0 || e.Clicks < 2)
                    return;

                this.Cursor = Cursors.WaitCursor;

                DataRow Dr = GrdDet.GetFocusedDataRow();
                string StrPartyCode = "", StrPartyType = "";

                StrPartyCode = Val.ToString(Dr["LEDGERCODE"]);
                StrPartyType = Val.ToString(Dr["LEDGERTYPE"]);

                if ((GrdDet.FocusedColumn != GrdDet.Columns["EMAILGROUP"]))
                {
                    FrmLedgerNew FrmLedgerNew = new FrmLedgerNew();
                    FrmLedgerNew.MdiParent = Global.gMainRef;
                    FrmLedgerNew.FormClosing += new FormClosingEventHandler(FrmLedger_FormClosing);
                    FrmLedgerNew.ShowForm(StrPartyType, Val.ToString(Dr["LEDGER_ID"]));
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }
        }

        private void GrdDet_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle < 0)
                    return;

                if (e.KeyCode == Keys.Enter)
                {
                    this.Cursor = Cursors.WaitCursor;

                    DataRow Dr = GrdDet.GetFocusedDataRow();

                    if (Dr == null)
                        return;

                    string StrPartyCode = "", StrPartyType = "";

                    StrPartyCode = Val.ToString(Dr["LEDGERCODE"]);
                    StrPartyType = Val.ToString(Dr["LEDGERTYPE"]);

                    if ((GrdDet.FocusedColumn != GrdDet.Columns["EMAILGROUP"]))
                    {
                        FrmLedgerNew FrmLedgerNew = new FrmLedgerNew();
                        FrmLedgerNew.MdiParent = Global.gMainRef;
                        FrmLedgerNew.ShowForm(StrPartyType, Val.ToString(Dr["LEDGER_ID"]));
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }

        }



        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            try
            {
                GrdDet.BestFitColumns();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.RowCount <= 0)
                    return;

                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = "Party List";
                svDialog.Filter = "Excel files 97-2003 (*.xls)|*.xls|Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {

                    PrintableComponentLinkBase link = new PrintableComponentLinkBase()
                    {
                        PrintingSystemBase = new PrintingSystemBase(),
                        Component = MainGrd,
                        Landscape = true,
                        PaperKind = PaperKind.A4,
                        Margins = new System.Drawing.Printing.Margins(20, 20, 20, 20)
                    };

                    link.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
                    link.ExportToXls(svDialog.FileName);

                    if (Global.Confirm("Do You Want To Open [" + "Party List" + ".xlsx] ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(svDialog.FileName, "CMD");
                    }
                }
                svDialog.Dispose();
                svDialog = null;

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        public void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            // ' For Report Title

            //TextBrick BrickTitle = e.Graph.DrawString("SELECTION LIST", System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 35), DevExpress.XtraPrinting.BorderSide.None);
            //BrickTitle.Font = new Font("verdana", 12, FontStyle.Bold);
            //BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            //BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;

            // ' For Group 
            TextBrick BrickTitleseller = e.Graph.DrawString("Party/Ledger List", System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitleseller.Font = new Font("verdana", 12, FontStyle.Bold);
            BrickTitleseller.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitleseller.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitleseller.ForeColor = Color.Black;

            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 400, 0));
            TextBrick BrickTitledate = e.Graph.DrawString("Print Date :- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), System.Drawing.Color.Navy, new RectangleF(IntX, 20, 400, 20), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitledate.Font = new Font("verdana", 8, FontStyle.Bold);
            BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitledate.ForeColor = Color.Black;

        }

        private void GrdDet_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                    return;

                string StrStatus = Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, "STATUS")).ToUpper();

                if (StrStatus == "DEACTIVE")
                {
                    e.Appearance.BackColor = lblDeactive.BackColor;
                    e.Appearance.BackColor2 = lblDeactive.BackColor;
                }
                else if (StrStatus == "WEB PENDING")
                {
                    e.Appearance.BackColor = lblWebPending.BackColor;
                    e.Appearance.BackColor2 = lblWebPending.BackColor;
                }
                else
                {
                    e.Appearance.BackColor = Color.Transparent;
                    e.Appearance.BackColor2 = Color.Transparent;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());

            }
        }

        private void FrmLedger_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnShow_Click(null, null);
        }

        private void FrmLedgerList_Load(object sender, EventArgs e)
        {
            DataTable DTabType = new DataTable();
            DTabType.Columns.Add(new DataColumn("TYPE"));
            DTabType.Rows.Add("SALE");
            DTabType.Rows.Add("PURCHASE");
            DTabType.Rows.Add("EMPLOYEE");
            DTabType.Rows.Add("BRANCH COMPANY");
            DTabType.Rows.Add("BROKER");
            DTabType.Rows.Add("CASH");
            DTabType.Rows.Add("BANK");
            DTabType.Rows.Add("COMMISSION");
            DTabType.Rows.Add("AIRFREIGHT");
            DTabType.Rows.Add("COURIER");
            DTabType.Rows.Add("EXPENSE");
            DTabType.Rows.Add("OTHER");
            DTabType.Rows.Add("PARTNER'S/DIRECTOR'S DETAILS");
            DTabType.Rows.Add("PENDING KYC");

            xtraTabActivity.TabPages.Clear();
            foreach (DataRow DRow in DTabType.Rows)
            {
                DevExpress.XtraTab.XtraTabPage Page = new DevExpress.XtraTab.XtraTabPage();
                Page.Text = Val.ToString(DRow["TYPE"]);
                Page.Tag = Val.ToString(DRow["TYPE"]);

                xtraTabActivity.TabPages.Add(Page);
            }
            xtraTabActivity.SelectedTabPageIndex = 0;

        }


        private void xtraTabActivity_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabActivity.TabPages.Count != 0)
            {
                BtnShow_Click(null, null);
                this.Text = Val.ToString(xtraTabActivity.SelectedTabPage.Tag) + " LIST";
            }

        }


        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(txtPassword.Tag) != "" && Val.ToString(txtPassword.Tag).ToUpper() == txtPassword.Text.ToUpper())
                {
                    GrdDet.Columns["EMAILGROUP"].OptionsColumn.AllowEdit = true;
                }
                else
                {
                    GrdDet.Columns["EMAILGROUP"].OptionsColumn.AllowEdit = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void repTxtEmailGroup_Enter(object sender, EventArgs e)
        {
            //try
            //{
            //    if (Val.ToString(GrdDet.GetFocusedRowCellValue("EMAILGROUP")) != "")
            //    {

            //        if (Global.Confirm("Do You Want To Update Process This Email Group Entry ? ") == System.Windows.Forms.DialogResult.No)
            //        {
            //            return;
            //        }

            //        LedgerListProperty property = new LedgerListProperty();

            //        property.LEDGER_ID = Val.ToGuid(Val.ToString(GrdDet.GetFocusedRowCellValue("LEDGER_ID")));
            //        property.EMAILGROUP = Val.ToString(Val.ToString(GrdDet.GetFocusedRowCellValue("EMAILGROUP")));

            //        property = ObjLedger.UpdateTransaction(property);

            //        Global.Message(property.ReturnMessageDesc);

            //        if (property.ReturnMessageType == "SUCCESS")
            //        {
            //            GrdDet.RefreshData();
            //        }
            //    }
            //}
            //catch(Exception ex)
            //{
            //    Global.Message(ex.Message);
            //}
        }

        private void GrdDet_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (GrdDet.FocusedColumn == GrdDet.Columns["EMAILGROUP"])
                {
                    if (Global.Confirm("Do You Want To Update Email Group For This Party ? ") == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }

                    LedgerMasterProperty property = new LedgerMasterProperty();
                    property.LEDGER_ID = Val.ToGuid(Val.ToString(GrdDet.GetFocusedRowCellValue("LEDGER_ID")));
                    property.EMAILGROUP = Val.ToString(Val.ToString(GrdDet.GetFocusedRowCellValue("EMAILGROUP")));

                    property = ObjLedger.UpdateLedgerEmailGroup(property);

                    Global.Message(property.ReturnMessageDesc);

                    if (property.ReturnMessageType == "SUCCESS")
                    {
                        GrdDet.RefreshData();
                    }

                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
    }
}
