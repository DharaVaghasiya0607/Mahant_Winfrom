using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using MahantExport.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MahantExport.Masters
{
	public partial class FrmStoneDayMaster  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
		BOMST_StoneDay ObjStoneDay = new BOMST_StoneDay();
		DataTable DtabDay = new DataTable();
		AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public FrmStoneDayMaster()
		{
			InitializeComponent();
		}
        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            BtnAdd_Click(null, null);
            Fill();
            this.Show();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjStoneDay);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

		#region Validation

		private bool ValSave()
		{

			int IntCol = 0, IntRow = -1;
			foreach (DataRow dr in DtabDay.Rows)
			{
				//For Update Validation
                //if (Val.Val(dr["FROMDAY"]) == 0 && Val.Val(dr["TODAY"]) != 0)
                //{
                //    Global.Message("Please Enter FROM DAY");
                //    IntCol = 1;
                //    IntRow = dr.Table.Rows.IndexOf(dr);
                //    break;
                //}
				//end as


				if (Val.Val(dr["FROMDAY"]) == 0)
				{
					if (DtabDay.Rows.Count == 1)
					{
						Global.Message("Please Enter FROM DAY");
						IntCol = 1;
						IntRow = dr.Table.Rows.IndexOf(dr);
						break;

					}
					else
						continue;
				}
				if (Val.Val(dr["TODAY"]) == 0)
				{
				    Global.Message("Please Enter TO Day");
				    IntCol = 2;
				    IntRow = dr.Table.Rows.IndexOf(dr);
				    break;
				}
				if (Val.ToString(dr["DAYNAME"]).Trim().Equals(string.Empty))
				{
					Global.Message("Please Enter NAME");
					IntCol = 1;
					IntRow = dr.Table.Rows.IndexOf(dr);
					break;
				}

			}
			if (IntRow >= 0)
			{
				GrdStone.FocusedRowHandle = IntRow;
				GrdStone.FocusedColumn = GrdStone.VisibleColumns[IntCol];
				GrdStone.Focus();
				return true;
			}
			return false;
		}

		#endregion

		public void Fill()
		{
			DtabDay = ObjStoneDay.Fill();
			DtabDay.Rows.Add(DtabDay.NewRow());
			MainGrdStoneDay.DataSource = DtabDay;
			MainGrdStoneDay.Refresh();
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			try
			{
				if (ValSave())
				{
					return;
				}
				string ReturnMessageType = "";
				string ReturnMessageDesc = "";

				foreach (DataRow Dro in DtabDay.Rows)
				{

					StoneDayProperty Property = new StoneDayProperty();

					if (Val.Val(Dro["FROMDAY"]) == 0)
					continue;

					Property.STONEDAY_ID = Val.ToInt32(Dro["STONEDAY_ID"]);
					Property.FROMDAY = Val.ToInt32(Dro["FROMDAY"]);
					Property.TODAY = Val.ToInt32(Dro["TODAY"]);
					Property.DAYNAME = Val.ToString(Dro["DAYNAME"]);
					Property = ObjStoneDay.SAVE(Property);

					ReturnMessageType = Property.RETURNMESSAGETYPE;
					ReturnMessageDesc = Property.RETURNMESSAGEDESC;

					Property = null;
				}

				DtabDay.AcceptChanges();
				Global.Message(ReturnMessageDesc);
				if (ReturnMessageType == "SUCCESS")
				{
					Fill();

					if (GrdStone.RowCount > 1)
					{
						GrdStone.FocusedRowHandle = GrdStone.RowCount +1;
					}
				}


			}
			catch (Exception EX)
			{
				Global.Message(EX.Message);
			}
		}
		
		public bool CheckDuplicate(string ColName, string ColValue, int IndexRow, string StrMsg)
		{
			if (Val.ToString(ColValue).Trim().Equals(string.Empty))
				return false;

			var Result = from row in DtabDay.AsEnumerable()
						 where Val.ToString(ColName).ToUpper() == Val.ToString(ColValue).ToUpper() && row.Table.Rows.IndexOf(row) != IndexRow
						 select row;

			if (Result.Any())
			{
				Global.Message(StrMsg + "ALLREADY EXISTS");
				return true;
			}
			return false;
						 
		}
		

		private void FrmStoneDay_M_Load(object sender, EventArgs e)
		{
			Fill();
		}

		private void RepTxtRemk_KeyDown(object sender, KeyEventArgs e)
		{
				try
			{
				if (e.KeyCode == Keys.Enter)
				{
					DataRow DR = GrdStone.GetFocusedDataRow();
					if (Val.Val(DR["FROMDAY"]) != 0 && Val.Val(DR["TODAY"]) != 0 && GrdStone.IsLastRow)
					{
						DtabDay.Rows.Add(DtabDay.NewRow());
					}
					else if (GrdStone.IsLastRow)
					{
						BtnSave.Focus();
						e.Handled = true;
					}
				}
			}
			catch (Exception ex)
			{
				Global.Message(ex.Message);
			}
		}

		private void reptxtFromDay_Validating(object sender, CancelEventArgs e)
		{
            GrdStone.PostEditor();
            DataRow Dr = GrdStone.GetFocusedDataRow();
            if (CheckDuplicate("FROMDAY", Val.ToString(GrdStone.EditingValue), GrdStone.FocusedRowHandle, "FROMDAY"))
            {
                e.Cancel = true;
                return;
            }
            else
            {
                Dr["DAYNAME"] = Val.ToString(GrdStone.EditingValue) + "-" + Val.ToString(Dr["TOCARAT"]);
                DtabDay.AcceptChanges();

            }
            if (Val.ToDecimal(Dr["TODAY"]) != 0)
            {
                if (Val.ToDecimal(GrdStone.EditingValue) > Val.ToDecimal(Dr["TODAY"]))
                {
                    Global.Message("From Day must be Greter Than To Day");
                    e.Cancel = true;
                    return;
                }
            }

            var dValue = from row in DtabDay.AsEnumerable()
                         where Val.Val(row["FROMDAY"]) <= Val.Val(GrdStone.EditingValue) && Val.Val(row["TODAY"]) >= Val.Val(GrdStone.EditingValue) && row.Table.Rows.IndexOf(row) != GrdStone.FocusedRowHandle
                         select row;

            if (dValue.Any())
            {
                Global.Message("This Value Already Exist Between Some From Day and To Day Please Check.!");
                e.Cancel = true;
                return;
            }
		}

		private void reptxtName_Validating(object sender, CancelEventArgs e)
		{
            DataRow Dr = GrdStone.GetFocusedDataRow();
            if (CheckDuplicate("DAYNAME", Val.ToString(GrdStone.EditingValue), GrdStone.FocusedRowHandle, "DAYNAME"))
            {
                e.Cancel = true;
                return;
            }
            else if (Val.ToString(GrdStone.EditingValue).Trim().Equals(String.Empty))
            {
                GrdStone.EditingValue = Val.ToString(Dr["FROMDAY"]) + "-" + Val.ToString(Dr["TODAY"]);
            }
						 
		}

		private void reptxtToDay_Validating(object sender, CancelEventArgs e)
		{
            DataRow Dr = GrdStone.GetFocusedDataRow();
            if (CheckDuplicate("TODAY", Val.ToString(GrdStone.EditingValue), GrdStone.FocusedRowHandle, "TODAY"))
            {
                e.Cancel = true;
                return;
            }
            if (Val.ToDecimal(Dr["FROMDAY"]) > Val.ToDecimal(GrdStone.EditingValue))
            {
                Global.Message("To Day must be Greter Than To Day");
                e.Cancel = true;
                return;
            }
            else
            {
                Dr["DAYNAME"] = Val.ToString(Dr["FROMDAY"]) + "-" + Val.ToString(GrdStone.EditingValue);
                DtabDay.AcceptChanges();

            }

            var dValue = from row in DtabDay.AsEnumerable()
                         where Val.Val(row["FROMDAY"]) <= Val.Val(GrdStone.EditingValue) && Val.Val(row["TODAY"]) >= Val.Val(GrdStone.EditingValue) && row.Table.Rows.IndexOf(row) != GrdStone.FocusedRowHandle
                         select row;

            if (dValue.Any())
            {
                Global.Message("This Value Already Exist Between Some From Day and To Day Please Check.!");
                e.Cancel = true;
                return;
            }

		}

		private void BtnAdd_Click(object sender, EventArgs e)
		{
			DtabDay.Rows.Clear();
			DtabDay.Rows.Add(DtabDay.NewRow());
			Fill();
		}

		private void BtnBack_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void BtnExport_Click(object sender, EventArgs e)
		{
			Global.ExcelExport("Day List", GrdStone);
		}

		private void GrdStone_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				SendKeys.Send("{TAB}");
			}
		}

		private void RepBtnDelete_Click(object sender, EventArgs e)
		{
			if (Global.Confirm("Are You Want To Delete This Record.??") == System.Windows.Forms.DialogResult.No)
				return;
            FrmPassword FrmPassword = new FrmPassword();
            if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
            {
                StoneDayProperty Property = new StoneDayProperty();
                DataRow DR = GrdStone.GetDataRow(GrdStone.FocusedRowHandle);
                Property.STONEDAY_ID = Val.ToInt32(DR["STONEDAY_ID"]);

                Property = ObjStoneDay.Delete(Property);

                if (Property.RETURNMESSAGETYPE == "SUCCESS")
                {
                    Global.Message(Property.RETURNMESSAGEDESC);
                    DtabDay.Rows.RemoveAt(GrdStone.FocusedRowHandle);
                    DtabDay.AcceptChanges();
                    Fill();
                }
                else
                {
                    Global.Message("ERROR IN DELETE");
                }
            }
		}
	}
	}

