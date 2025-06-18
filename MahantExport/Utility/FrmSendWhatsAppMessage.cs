using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Globalization;
using System.Collections;
using System.Net.Mail;
using BusLib.Configuration;
using BusLib.TableName;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;
using MahantExport.Utility;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using MahantExport.Class;
using System.Threading.Tasks;
using RestSharp;
using System.Net.Http;

namespace MahantExport
{
    public partial class FrmSendWhatsAppMessage : DevControlLib.cDevXtraForm
    {
        BODevGridSelection ObjGridSelection;

        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();

       
        public FrmSendWhatsAppMessage()
        {
            InitializeComponent();
        }

        public void ShowForm(string pStrWhatsappMessage)
        {
            htmlEditor.InnerText = pStrWhatsappMessage;
            AttachFormDefaultEvent();
            Val.FormGeneralSetting(this);
            this.Show();
        }

        private void AttachFormDefaultEvent()
        {
            Val.FormGeneralSetting(this);
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.FormKeyPress = false;

            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);

        }

       



        private async void BtnSend_Click(object sender, EventArgs e)
        {
            await WhatsAppSender.SendTemplateMessageAsync();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            htmlEditor.InnerText = string.Empty;
            htmlEditor.InnerHtml = string.Empty;
            txtAttachment.Text = string.Empty;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open = new OpenFileDialog();
            Open.Title = "Browse Text Files";
            if (Open.ShowDialog() == DialogResult.OK)
            {
                txtAttachment.Text = Open.FileName;
            }
            Open.Dispose();
            Open = null;

        }

        private void FrmEmailSend_Load(object sender, EventArgs e)
        {
            //MainGridMarty.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);

            DataTable dt = new DataTable();
            dt.Columns.Add("PARTYNAME", typeof(string));
            dt.Columns.Add("MOBILENO", typeof(string));

            // Example: Add rows
            dt.Rows.Add("Vipul Vankdiya", "8530067187");
            dt.Rows.Add("Dhara Vaghasiya", "9909320452");

            MainGridMarty.DataSource = dt;

            if (MainGridMarty.RepositoryItems.Count == 0)
            {
                ObjGridSelection = new BODevGridSelection();
                ObjGridSelection.View = GrdDetParty;
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                GrdDetParty.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
            }
            else
            {
                ObjGridSelection.ClearSelection();
            }

            GrdDetParty.BestFitColumns();

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

        private void lblOpenFile_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(txtAttachment.Text, "cmd");
        }
    }
}