using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Report
{
    public partial class FrmDashBoard : DevExpress.XtraEditors.XtraForm
    {
        System.Diagnostics.Stopwatch watch = null;
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Parameter ObjMast = new BOMST_Parameter();
        private bool isPasteAction = false;
        private const Keys PasteKeys = Keys.Control | Keys.V;

        public FrmDashBoard()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            lblEmployeeName.Text = BOConfiguration.gEmployeeProperty.LEDGERNAME;
            LblDate.Text = DateTime.Now.ToShortDateString();
            DataTable DTabLocation = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LOCATION);
            CmbLocation.DataSource = DTabLocation;
            CmbLocation.DisplayMember = "LOCATIONNAME";
            CmbLocation.ValueMember = "LOCATION_ID";
            Fill();
            this.Show();
        }
        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }
        public void Fill()
        {
            DataSet DSet = ObjMast.GetDataForDashBoard();
            MainGridOfferPrice.DataSource = DSet.Tables[0];
            GrdOfferPrice.BestFitColumns();

            MainGridNotification.DataSource = DSet.Tables[1];
            GrdNotifications.BestFitColumns();
        }

        private void BtnAddSendAndReceive_Click(object sender, EventArgs e)
        {
            try
            {
                GrpAddSendAndReceive.Visible = true;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void btnGrpClose_Click(object sender, EventArgs e)
        {
            try
            {
                GrpAddSendAndReceive.Visible = false;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                NotificationSendAndReceive Property = new NotificationSendAndReceive();

                Property.STONENO = Val.ToString(txtStoneNo.Text);
                Property.DISCRIPTION = Val.ToString(txtDescription.Text);
                Property.LOACTION_ID = Val.ToInt32(CmbLocation.SelectedValue);

                Property = ObjMast.SaveSendAndReceive(Property , "INSERT");
                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    btnGrpClose_Click(null,null);
                    txtDescription.Text = string.Empty;
                    txtStoneNo.Text = string.Empty;
                    Fill();                      
                }                
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtStoneNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                
                    String str1 = Val.ToString(txtStoneNo.Text);
                    string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                    if (result.EndsWith(",,"))
                    {
                        result = result.Remove(result.Length - 1);
                    }
                    txtStoneNo.Text = result;
                if (isPasteAction)
                {
                    isPasteAction = false;
                    txtStoneNo.Select(txtStoneNo.Text.Length, 0);
                }
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void timer30Second_Tick(object sender, EventArgs e)
        {
            try
            {
                Fill();
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
            }
        }

        private void GrdNotifications_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                if (e.Column.FieldName == "ISRECEIVE" || e.Column.FieldName == "ISCLEAR" || e.Column.FieldName == "ISSEND")
                {
                    NotificationSendAndReceive Property = new NotificationSendAndReceive();

                    Property.Notification_ID = Val.ToGuid(GrdNotifications.GetFocusedRowCellValue("NOTIFICATION_ID"));
                    Property.ISRECEIVE = Val.ToBoolean(GrdNotifications.GetFocusedRowCellValue("ISRECEIVE"));
                    Property.ISCLEAR = Val.ToBoolean(GrdNotifications.GetFocusedRowCellValue("ISCLEAR"));
                    Property.ISSEND = Val.ToBoolean(GrdNotifications.GetFocusedRowCellValue("ISSEND"));

                    Property = ObjMast.SaveSendAndReceive(Property, "");
                    Global.Message(Property.ReturnMessageDesc);

                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        Fill();
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void MainGridNotification_Click(object sender, EventArgs e)
        {

        }

        private void txtStoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                isPasteAction = true;
            }
        }
    }
}