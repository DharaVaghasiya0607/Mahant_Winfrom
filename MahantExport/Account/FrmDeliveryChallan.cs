using BusLib.Master;
using BusLib.TableName;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace MahantExport.Account
{
    public partial class FrmDeliveryChallan  : DevControlLib.cDevXtraForm
    {

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();

        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_DelliveryChallan ObjMast = new BOMST_DelliveryChallan();
        DataTable DTabDelivery = new DataTable();
        int IntDELIVERY_ID = 0;
        public FrmDeliveryChallan()
        {
            InitializeComponent();
        }

        #region Property Settings


        public void ShowForm()
        {
            AttachFormEvents();
            this.Show();
            Clear();
        }

        public void AttachFormEvents()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            //ObjFormEvent.ObjToDisposeList.Add(ObjLanguage);
        }

        #endregion

        #region VALIDATION
        private bool ValSave()
        {
            if (txtChallanNo.Text.Trim().Length == 0)
            {
                Global.Message("ChallanNo Name Is Required");
                DATE.Focus();
                return false;
            }
            return true;
        }

        #endregion

        private void BtnSave_Click(object sender, EventArgs e)
        {

            if (ValSave() == false)
            {
                return;
            }


            if (Global.Confirm("Are Your Sure To Save" + " The Record ?") == System.Windows.Forms.DialogResult.No)
                return;

            DeliveryChallanProperty Property = new DeliveryChallanProperty();

            Property.DATE = Val.SqlDate(DTPDate.Text);
            Property.CARAT = txtCarat.Text;
            Property.CHALLANNO = txtChallanNo.Text;

            Property = ObjMast.Save(Property);
            Global.Message(Property.ReturnMessageDesc);

            if (Property.ReturnMessageType == "SUCCESS")
            {
                Fill();
                BtnPrint_Click(null, null);
                if (GridData.RowCount > 1)
                {
                    GridData.FocusedRowHandle = GridData.RowCount - 1;
                }
                DTPDate.Checked = false;
                DTPDate.Value = DateTime.Now;
                txtChallanNo.Text = string.Empty;
                txtCarat.Text = string.Empty;
            }
            else
            {
                return;
            }
            Property = null;
        }
        public void Fill()
        {
            DataTable DTab = ObjMast.Fill();
            MainGrid.DataSource = DTab;
            MainGrid.Refresh();
        }
        public void Clear()
        {
            DTabDelivery.Rows.Clear();
            DTabDelivery.Rows.Add(DTabDelivery.NewRow());
        }


        private void BtnAdd_Click(object sender, EventArgs e)
        {
            DTPDate.Checked = false;
            DTPDate.Value = DateTime.Now;
            txtChallanNo.Text = string.Empty;
            txtCarat.Text = string.Empty;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (Global.Confirm("Are You Sure For Print Entry") == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DTab = ObjMast.Print(Val.ToString(txtChallanNo.Text));
                if (DTab.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("There Is No Data Found For Print");
                    return;
                }

                DataSet DS = new DataSet();
                DTab.TableName = "Table";
                DS.Tables.Add(DTab);
                DataTable DTabDuplicate = DTab.Copy();
                DTabDuplicate.TableName = "Table1";
                DTabDuplicate.AcceptChanges();
                DS.Tables.Add(DTabDuplicate);

                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowFormInvoicePrint("Rpt_AccDeliveryChallan", DTabDuplicate);
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DeliveryChallanProperty Property = new DeliveryChallanProperty();
            try
            {

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;


                Property.DELIVERY_ID = Val.ToInt32(IntDELIVERY_ID);
                Property = ObjMast.Delete(Property);
                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    BtnAdd_Click(null, null);
                    Fill();
                }
                else
                {
                    CHALLANNO.Focus();
                }

            }
            catch (System.Exception ex)
            {
                Global.MessageToster(ex.Message);
            }
            Property = null;

        }

        private void txtCarat_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtChallanNo.Focus();
                }
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message.ToString());

            }
        }

        private void txtChallanNo_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    BtnSave.Focus();
                }
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message.ToString());
            }
        }

        private void GridData_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }

            if (e.Clicks == 2)
            {
                DataRow DR = GridData.GetDataRow(e.RowHandle);
                FetchValue(DR);
                DR = null;
                BtnDelete.Visible = true;
            }
        }

        private void FrmDeliveryChallan_Load(object sender, EventArgs e)
        {
            Fill();
            DataTable dt = new DataTable();
            dt = ObjMast.FindVoucherNoNew();
            if (dt.Rows.Count > 0)
            {
                txtChallanNo.Text = Val.ToString(dt.Rows[0]["CHALLANNO"]);
            }
        }

        public void FetchValue(DataRow DR)
        {
            txtCarat.Text = Val.ToString(DR["CARAT"]);
            txtChallanNo.Text = Val.ToString(DR["CHALLANNO"]);
            //public void FetchValue(DataRow DR)
            //{
            //txtParaType.Text = Val.ToString(DR["DELIVERY_ID"]);
            //}
        }
    }
}     
