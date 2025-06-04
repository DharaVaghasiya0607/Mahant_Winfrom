using BusLib.Configuration;
using BusLib.CRM;
using BusLib.TableName;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.CRM
{
	public partial class FrmTargetCreateMaster  : DevControlLib.cDevXtraForm
	{
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
		DataTable DtabTarget = new DataTable();
		BOCRM_TargetCreateMaster ObjTarg = new BOCRM_TargetCreateMaster();
		BOFormPer ObjPer = new BOFormPer();

		bool IsNextImage = true;

		public FrmTargetCreateMaster()
		{
			InitializeComponent();
		}

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
			ObjPer.GetFormPermission(Val.ToString(this.Tag));
			if (ObjPer.ISVIEW == false)
			{
				Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
				return;
			}

			this.Show();

            txtYear.Text = DateTime.Now.Year.ToString();
            CmbMonth.SelectedItem = DateTime.Now.AddMonths(0).ToString("MMM");

            txtCopyToYear.Text = DateTime.Now.Year.ToString();
            CmbCopyToMonth.SelectedItem = DateTime.Now.AddMonths(0).ToString("MMM");

            //CmbMonth.DataSource = CultureInfo.InvariantCulture.DateTimeFormat.MonthNames.Take(12).ToList();
            //CmbMonth.SelectedItem = CultureInfo.InvariantCulture.DateTimeFormat.MonthNames[DateTime.Now.AddMonths(-1).Month];
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
            ObjFormEvent.ObjToDisposeList.Add(ObjTarg);
        }

		
        public void Fill()
		{
			if (Val.ToInt32(txtYear.Text) == 0)
			{
				Global.Message("Year Is Required");
				txtYear.Focus();
				return;
			}
			if (Val.ToString(CmbMonth.Text) == "")
			{
				Global.Message("Month Is Required");
				CmbMonth.Focus();
				return;
			}

			DtabTarget.Rows.Clear();

			int StrMonth = 0;
            StrMonth = Val.ToInt32(CmbMonth.SelectedIndex + 1);

			DtabTarget = ObjTarg.Fill(Val.ToInt32(txtYear.Text), StrMonth);
			MainGrid.DataSource = DtabTarget;
			MainGrid.Refresh();

			GrdDet.FocusedColumn = GrdDet.VisibleColumns[0];
			GrdDet.Focus();
			GrdDet.ShowEditor();

		}

		private void BtnShow_Click(object sender, EventArgs e)
		{
			Fill();
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			if (Val.ToInt32(txtYear.Text) == 0)
			{
				Global.Message("Year Is Required");
				txtYear.Focus();
				return;
			}
			
			string ReturnMessageValue = "";
			string ReturnMessageDesc = "";
			string ReturnMessageType = "";

			//txtYear.Tag = Val.ToString("EMPLOYEE_ID") == "" ? Guid.Empty : Guid.Parse(Val.ToString("EMPLOYEE_ID"));

			foreach (DataRow Dr in DtabTarget.Rows)
			{
			
				if (Val.ToDecimal(Dr["SALETARGETDOLLAR"]) == 0 || Val.ToInt32(Dr["NOOFCUSTOMER"]) == 0 || Val.ToInt32(Dr["NOOFNEWCUSTOMER"]) == 0)
					continue;
					
				TargetCreateMaster Property = new TargetCreateMaster();

				Property.FROMYEAR = Val.ToInt(txtYear.Text);
                Property.FROMMONTH = Val.ToInt32(CmbMonth.SelectedIndex + 1);
				Property.EMPLOYEE_ID = Val.ToString(Dr["EMPLOYEE_ID"]) == ""? Guid.Empty : Guid.Parse(Val.ToString(Dr["EMPLOYEE_ID"]));
				Property.TARGET_ID = Val.ToString(Dr["TARGET_ID"]) == "" ? Guid.Empty : Guid.Parse(Val.ToString(Dr["TARGET_ID"]));
				Property.SALETARGETDOLLAR = Val.ToDecimal(Dr["SALETARGETDOLLAR"]);
				Property.NOOFCUST = Val.ToInt32(Dr["NOOFCUSTOMER"]);
				Property.NOOFNEWCUST = Val.ToInt32(Dr["NOOFNEWCUSTOMER"]);

				Property = ObjTarg.Save(Property);

				ReturnMessageValue = Property.RETURNVALUE;
				ReturnMessageDesc = Property.RETURNMESSAGEDESC;
				ReturnMessageType = Property.RETURNMESSAGETYPE;

				Property = null;
			}
			DtabTarget.AcceptChanges();

			Global.Message(ReturnMessageDesc);

			if (ReturnMessageType == "SUCCESS")
			{
				Fill();

				if (GrdDet.RowCount > 1)
				{
					GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
				}
			}

		}

		private void BtnLeft_Click(object sender, EventArgs e)
		{
			if (IsNextImage)
			{
				BtnLeft.Image = MahantExport.Properties.Resources.A1;
				PnlCopyPaste.Visible = false;
				IsNextImage = false;
			}
			else
			{
				BtnLeft.Image = MahantExport.Properties.Resources.A2;
				PnlCopyPaste.Visible = true;
				IsNextImage = true;
				txtCopyToYear.Focus();
			}
		}

		private void BtnCopy_Click(object sender, EventArgs e)
		{

			if (Val.ToInt32(txtYear.Text) == 0)
			{
				Global.Message("Year Is Required");
				txtYear.Focus();
				return;
			}
			if (Val.ToString(CmbMonth.Text).Trim().Equals(string.Empty))
			{
				Global.Message("Month Is Required");
				CmbMonth.Focus();
				return;
			}

			if (Val.ToInt32(txtCopyToYear.Text) == 0)
			{
				Global.Message("Copy To Year Is Required");
				txtCopyToYear.Focus();
				return;
			}
			if (Val.ToString(CmbCopyToMonth .Text).Trim().Equals(string.Empty))
			{
				Global.Message("Copy To Month Is Required");
				CmbCopyToMonth.Focus();
				return;
			}

			if (Global.Confirm("Are You Sure You Want To Copy Data?") == System.Windows.Forms.DialogResult.No)
			{
				return;
			}
			this.Cursor = Cursors.WaitCursor;

			int IntRes = ObjTarg.CopyPastTarget(Val.ToInt(txtYear.Text), Val.ToInt32(CmbMonth.SelectedIndex + 1), Val.ToInt(txtCopyToYear.Text), Val.ToInt32(CmbCopyToMonth.SelectedIndex+1));
			this.Cursor = Cursors.Default;

			if (IntRes != -1)
			{
				Global.Message("Data Copied Successfully");
				txtCopyToYear.Text = string.Empty;
				CmbCopyToMonth.SelectedText = string.Empty;
				return;
			}

		}

		private void BtnAdd_Click(object sender, EventArgs e)
		{
            txtYear.Text = DateTime.Now.Year.ToString();
            CmbMonth.SelectedItem = DateTime.Now.AddMonths(0).ToString("MMM");
			DtabTarget.Rows.Clear();
            txtYear.Focus();

		}

		private void BtnExport_Click(object sender, EventArgs e)
		{
			Global.ExcelExport("TargetMaster", GrdDet);
		}

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteSelectedAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY") == System.Windows.Forms.DialogResult.Yes)
                    {
                        TargetCreateMaster Property = new TargetCreateMaster();
                        DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                        Property.TARGET_ID = Guid.Parse(Val.ToString(Drow["TARGET_ID"]));
                        Property = ObjTarg.Delete(Property);

                        if (Property.RETURNMESSAGETYPE == "SUCCESS")
                        {
                            Global.Message("ENTRY DELETED SUCCESSFULLY");
                            DtabTarget.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                            DtabTarget.AcceptChanges();
                            Fill();
                        }
                        else
                        {
                            Global.Message("ERROR IN DELETE ENTRY");
                        }

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
                