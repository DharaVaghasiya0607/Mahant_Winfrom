using BusLib.Configuration;
using BusLib.Pricing;
using BusLib.TableName;
using DevExpress.XtraGrid.Views.Base;
using MahantExport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Pricing
{
	public partial class FrmLabChargesUpload  : DevControlLib.cDevXtraForm
	{
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
		DataTable DtabExcelData = new DataTable();
		DataTable DtabPara = new DataTable();
		BOFormPer ObjPer = new BOFormPer();
		DataTable DtabGetData = new DataTable();
		BOMST_LabChargesUpload Obj = new BOMST_LabChargesUpload();

		public FrmLabChargesUpload()
		{
			InitializeComponent();
		}
		public void ShowForm()
		{
            Val.FormGeneralSetting(this);
			AttachFormEvents();
			ObjPer.GetFormPermission(Val.ToString(this.Tag));
			if (ObjPer.ISVIEW == false)
			{
				Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
				return;
			}



			DtabPara = Obj.GetParameterData();

			DtabExcelData = Obj.GetTableHeader();
			DtabExcelData.Rows.Add(DtabExcelData.NewRow());

			MainGrid.DataSource = DtabExcelData;
			MainGrid.RefreshDataSource();

			this.Show();
		}

		public void AttachFormEvents()
		{
            ObjFormEvent.mForm = this;
			//objBOFormEvents.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;

            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(Obj);
		}

		private void BtnBrowse_Click(object sender, EventArgs e)
		{
			try
			{
				OpenFileDialog OpenFileDialog = new OpenFileDialog();
				OpenFileDialog.Filter = "Excel Files (*.xls,*.xlsx)|*.xls;*.xlsx;";
				if (OpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					txtFileName.Text = OpenFileDialog.FileName;

					string extension = Path.GetExtension(txtFileName.Text.ToString());
					string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(txtFileName.Text);
					destinationPath = destinationPath.Replace(extension, ".xlsx");
					if (File.Exists(destinationPath))
					{
						File.Delete(destinationPath);
					}
					File.Copy(txtFileName.Text, destinationPath);

					if (File.Exists(destinationPath))
					{
						File.Delete(destinationPath);
					}


				}
				OpenFileDialog.Dispose();
				OpenFileDialog = null;
			}
			catch (Exception ex)
			{
				Global.Message(ex.ToString() + "InValid File Name");
			}
		}

		private void lblSampleExcelFile_Click(object sender, EventArgs e)
		{
			try
			{
				string StrFilePathDestination = "";
				StrFilePathDestination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\LabChargesUpload" + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.Day.ToString() + ".xlsx";
				if (File.Exists(StrFilePathDestination))
				{
					File.Delete(StrFilePathDestination);
				}
				File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\Format\\LabChargesUpload.xlsx", StrFilePathDestination);

				System.Diagnostics.Process.Start(StrFilePathDestination, "CMD");
			}
			catch (Exception ex)
			{
				Global.Message(ex.Message);
			}
		}

		public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
		{
			using (var pck = new OfficeOpenXml.ExcelPackage())
			{
				using (var stream = File.OpenRead(path))
				{
					pck.Load(stream);
				}
				var ws = pck.Workbook.Worksheets.First();
				DataTable tbl = new DataTable();
				foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
				{
					if (Convert.ToString(firstRowCell.Text).Equals(string.Empty))
						continue;

					tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
				}
				var startRow = hasHeader ? 2 : 1;
				for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
				{
					var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
					DataRow row = tbl.Rows.Add();
					foreach (var cell in wsRow)
					{
						if (Convert.ToString(cell.Text).Equals(string.Empty))
							continue;

						row[cell.Start.Column - 1] = cell.Text;
					}
				}
				return tbl;
			}
		}

		private void BtnCalculate_Click(object sender, EventArgs e)
		{
			try
			{
				if (txtFileName.Text == "" || txtFileName.Text.Trim().Equals(string.Empty))
				{
					Global.Message("Please Select Excel File To Upload");
					txtFileName.Focus();
					return;
				}
				txtAddless1.Text = Val.ToString(0);
				txtAddLess2.Text = Val.ToString(0);

				this.Cursor = Cursors.WaitCursor;
				BtnBrowse.Enabled = false;

				DtabExcelData.Rows.Clear();
				string extension = Path.GetExtension(txtFileName.Text.ToString());
				string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(txtFileName.Text);
				destinationPath = destinationPath.Replace(extension, ".xlsx");
				if (File.Exists(destinationPath))
				{
					File.Delete(destinationPath);
				}
				File.Copy(txtFileName.Text, destinationPath);

				DtabExcelData = GetDataTableFromExcel(destinationPath);

				if (File.Exists(destinationPath))
				{
					File.Delete(destinationPath);
				}

				DtabExcelData.Columns.Add("LABCHARGEUPLOAD_ID", typeof(Guid));
				DtabExcelData.Columns.Add("FROMCOLOR_ID", typeof(int));
				DtabExcelData.Columns.Add("TOCOLOR_ID", typeof(int));
				DtabExcelData.Columns.Add("SERVICETYPE_ID", typeof(int));

				MainGrid.DataSource = DtabExcelData;

				this.Cursor = Cursors.Default;
				BtnBrowse.Enabled = true;
				BtnVerify.Enabled = true;
			}
			catch (Exception ex)
			{
				Global.Message(ex.Message);
				BtnBrowse.Enabled = true;
				BtnVerify.Enabled = false;
			}
		}

		private void ReptxtAmt_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				GrdDet.PostEditor();
				DataRow DR = GrdDet.GetFocusedDataRow();

				if (Val.ToString(DR["LABCHARGEUPLOAD_ID"]) == "" && GrdDet.RowCount < 1 || GrdDet.RowCount == 1)
				{

						DateTime Date1 = new DateTime(2021, 01, 01);
						DateTime Date2 = new DateTime(2021, 01, 01);

						Date1 = Convert.ToDateTime(DTPFromAppDate.Text);
						Date2 = Convert.ToDateTime(DTPToAppDate.Text);


						int Res = DateTime.Compare(Date1, Date2);

						if (Res > 0)
						{
							Global.Message("FROM-DATE MUST BE LESS THEN TO-DATE. PLEASE CHEKE APPLICABLE DATE..");
							DtpFromDate.Focus();
							this.Cursor = Cursors.Default;

							return;
						}

						DataTable DTABVALIDATION = Obj.GetDataForValidation(Val.SqlDate(DTPFromAppDate.Text), Val.SqlDate(DTPToAppDate.Text));

						if (DTABVALIDATION.Rows.Count != 0)
						{
							Global.Message("FROM-APPLICABLEDATE AND TO-PPLICABLEDATE IS ALREADY EXIST PLEASE CHECK..");
							DtpFromDate.Focus();
							this.Cursor = Cursors.Default;

							return;
						}
				}

				if (Val.Val(DR["FROMCARAT"]) != 0 && Val.Val(DR["TOCARAT"]) != 0 && Val.Val(DR["AMOUNT"]) != 0)
				{
					LabChargesUploadProperty Property = new LabChargesUploadProperty();

					string Str_IDs = "";

					Property.LABCHARGEUPLOAD_ID = Val.ToString(DR["LABCHARGEUPLOAD_ID"]).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(DR["LABCHARGEUPLOAD_ID"])); ;
					Property.FROMCARAT = Val.Val(DR["FROMCARAT"]);
					Property.TOCARAT = Val.Val(DR["TOCARAT"]);
					Property.FROMCOLOR = Val.ToString(DR["FROMCOLOR"]);
					Property.FROMCOLOR_ID = Val.ToInt(DR["FROMCOLOR_ID"]);
					Property.TOCOLOR = Val.ToString(DR["TOCOLOR"]);
					Property.TOCOLOR_ID = Val.ToInt(DR["TOCOLOR_ID"]);
					Property.SERVICETYPE = Val.ToString(DR["SERVICETYPE"]);
					Property.SERVICETYPE_ID = Val.ToInt(DR["SERVICETYPE_ID"]);
					Property.AMOUNT = Val.Val(DR["AMOUNT"]);
					Property.CALCTYPE = Val.ToString(DR["CALCTYPE"]);
					Property.DIAMONDTYPE = Val.ToString(CmbDiamondType.Text);
					Property.LAB = Val.ToString(cmbLab.Text);


					Property.APPLICABLEDATE = Val.SqlDate(DTPFromAppDate.Text);
					Property.TOAPPLICABLEDATE = Val.SqlDate(DTPToAppDate.Text);

					if (Val.Val(txtAddless1.Text) == 0)
					{
						DataTable Dtab = Obj.FindAddLessAmt(Val.SqlDate(DTPFromAppDate.Text),Val.SqlDate(DTPToAppDate.Text));
						if (Dtab.Rows.Count == 0)
						{
							Property.ADDLESS1 = 0;
						}
						else
						{
							DataRow DRs = Dtab.Rows[0];

							Property.ADDLESS1 = Val.Val(DRs["ADDLESS1"]);
						}
					}
					else
					{
						Property.ADDLESS1 = Val.Val(txtAddless1.Text);
					}
					if (Val.Val(txtAddLess2.Text) == 0)
					{
						DataTable Dtab = Obj.FindAddLessAmt(Val.SqlDate(DTPFromAppDate.Text), Val.SqlDate(DTPToAppDate.Text));
						if (Dtab.Rows.Count == 0)
						{
							Property.ADDLESS2 = 0;
						}
						else
						{
							DataRow DRs = Dtab.Rows[0];

							Property.ADDLESS2 = Val.Val(DRs["ADDLESS2"]);
						}
					}
					else
					{
						Property.ADDLESS2 = Val.Val(txtAddLess2.Text);
					}

					Str_IDs = Val.ToString(Property.LABCHARGEUPLOAD_ID);
					Obj.Save(Property);

					DR["LABCHARGEUPLOAD_ID"] = (Val.ToString(Str_IDs));

					DtabExcelData.AcceptChanges();
					
				}

				if (Val.Val(DR["FROMCARAT"]) != 0 && Val.Val(DR["TOCARAT"]) != 0 && Val.ToString(DR["FROMCOLOR"]) != "" && Val.ToString(DR["TOCOLOR"]) != "" && Val.Val(DR["AMOUNT"]) != 0 && GrdDet.IsLastRow)
				{
					DtabExcelData.Rows.Add(DtabExcelData.NewRow());
				}
				else if (GrdDet.IsLastRow)
				{
					e.Handled = true;
				}
			}
		}

		private void ReptxtFromColor_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				DataRow DR = GrdDet.GetFocusedDataRow();

				if (Global.OnKeyPressEveToPopup(e))
				{
					FrmSearchPopupBox FrmSearchPopupBox = new FrmSearchPopupBox();
					FrmSearchPopupBox.mStrSearchField = "COLORNAME,COLORCODE";
					FrmSearchPopupBox.mStrSearchText = e.KeyChar.ToString();
					this.Cursor = Cursors.WaitCursor;
					FrmSearchPopupBox.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLOR);

					FrmSearchPopupBox.mStrColumnsToHide = "COLOR_ID";
					//FrmSearchPopupBox.mColumnsToHide = "SEQUENCENO";

					this.Cursor = Cursors.Default;
					FrmSearchPopupBox.ShowDialog();
					e.Handled = true;
					if (FrmSearchPopupBox.DRow != null)
					{
						DR["FROMCOLOR"] = Val.ToString(FrmSearchPopupBox.DRow["COLORNAME"]);
						DR["FROMCOLOR_ID"] = Val.ToString(FrmSearchPopupBox.DRow["COLOR_ID"]);
					}
					else
					{
						DR["FROMCOLOR"] = Val.ToString(DBNull.Value);
						DR["FROM_ID"] = Val.ToString(DBNull.Value);
					}
					FrmSearchPopupBox.Hide();
					FrmSearchPopupBox.Dispose();
					FrmSearchPopupBox = null;
				}
				LabChargesUploadProperty Property = new LabChargesUploadProperty();

				if (Val.ToString(DR["LABCHARGEUPLOAD_ID"]) == "")
				{
					return;
				}
				else
				{
					if (Val.Val(DR["FROMCARAT"]) > Val.Val(DR["TOCARAT"]))
					{
						Global.Message("TOCARAT MUST BE GREATER THEN FROM CARAT.PLEASE CHECK..");
						return;
					}
					if (Val.Val(DR["AMOUNT"]) == 0)
					{
						Global.Message("PLEASE ENTER AMOUNT..");
						GrdDet.FocusedColumn = GrdDet.Columns["AMOUNT"];
						return;
					}

					Property.LABCHARGEUPLOAD_ID = Guid.Parse(Val.ToString(DR["LABCHARGEUPLOAD_ID"]));//.Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(DRow["LABCHARGEUPLOAD_ID"])); ;
					Property.FROMCARAT = Val.Val(DR["FROMCARAT"]);
					Property.TOCARAT = Val.Val(DR["TOCARAT"]);
					Property.FROMCOLOR = Val.ToString(DR["FROMCOLOR"]);
					Property.FROMCOLOR_ID = Val.ToInt(DR["FROMCOLOR_ID"]);
					Property.TOCOLOR = Val.ToString(DR["TOCOLOR"]);
					Property.TOCOLOR_ID = Val.ToInt(DR["TOCOLOR_ID"]);
					Property.SERVICETYPE = Val.ToString(DR["SERVICETYPE"]);
					Property.SERVICETYPE_ID = Val.ToInt(DR["SERVICETYPE_ID"]);
					Property.AMOUNT = Val.Val(DR["AMOUNT"]);

					Property.APPLICABLEDATE = Val.SqlDate(DTPFromAppDate.Text);
					Property.TOAPPLICABLEDATE = Val.SqlDate(DTPToAppDate.Text);
					Property.ADDLESS1 = Val.Val(txtAddless1.Text);
					Property.ADDLESS2 = Val.Val(txtAddLess2.Text);

					Obj.Save(Property);

					DtabExcelData.AcceptChanges();
				}
			}
			catch (Exception ex)
			{
				Global.Message(ex.Message);
			}
		}

		private void RepTxtToColor_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				DataRow DR = GrdDet.GetFocusedDataRow();

				if (Global.OnKeyPressEveToPopup(e))
				{
                    FrmSearchPopupBox FrmSearchPopupBox = new FrmSearchPopupBox();
					FrmSearchPopupBox.mStrSearchField = "COLORNAME,COLORCODE";
					FrmSearchPopupBox.mStrSearchText = e.KeyChar.ToString();
					this.Cursor = Cursors.WaitCursor;
					FrmSearchPopupBox.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLOR);

					FrmSearchPopupBox.mStrColumnsToHide = "COLOR_ID";
					//FrmSearchPopupBox.mColumnsToHide = "SEQUENCENO";

					this.Cursor = Cursors.Default;
					FrmSearchPopupBox.ShowDialog();
					e.Handled = true;
					if (FrmSearchPopupBox.DRow != null)
					{
						DR["TOCOLOR"] = Val.ToString(FrmSearchPopupBox.DRow["COLORNAME"]);
						DR["TOCOLOR_ID"] = Val.ToString(FrmSearchPopupBox.DRow["COLOR_ID"]);
					}
					else
					{
						DR["TOCOLOR"] = Val.ToString(DBNull.Value);
						DR["TOCOLOR_ID"] = Val.ToString(DBNull.Value);
					}
					FrmSearchPopupBox.Hide();
					FrmSearchPopupBox.Dispose();
					FrmSearchPopupBox = null;
				}

				LabChargesUploadProperty Property = new LabChargesUploadProperty();

				if (Val.ToString(DR["LABCHARGEUPLOAD_ID"]) == "")
				{
					return;
				}
				else
				{
					if (Val.Val(DR["FROMCARAT"]) > Val.Val(DR["TOCARAT"]))
					{
						Global.Message("TOCARAT MUST BE GREATER THEN FROM CARAT.PLEASE CHECK..");
						return;
					}
					if (Val.Val(DR["AMOUNT"]) == 0)
					{
						Global.Message("PLEASE ENTER AMOUNT..");
						GrdDet.FocusedColumn = GrdDet.Columns["AMOUNT"];
						return;
					}

					Property.LABCHARGEUPLOAD_ID = Guid.Parse(Val.ToString(DR["LABCHARGEUPLOAD_ID"]));//.Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(DRow["LABCHARGEUPLOAD_ID"])); ;
					Property.FROMCARAT = Val.Val(DR["FROMCARAT"]);
					Property.TOCARAT = Val.Val(DR["TOCARAT"]);
					Property.FROMCOLOR = Val.ToString(DR["FROMCOLOR"]);
					Property.FROMCOLOR_ID = Val.ToInt(DR["FROMCOLOR_ID"]);
					Property.TOCOLOR = Val.ToString(DR["TOCOLOR"]);
					Property.TOCOLOR_ID = Val.ToInt(DR["TOCOLOR_ID"]);
					Property.SERVICETYPE = Val.ToString(DR["SERVICETYPE"]);
					Property.SERVICETYPE_ID = Val.ToInt(DR["SERVICETYPE_ID"]);
					Property.AMOUNT = Val.Val(DR["AMOUNT"]);
					Property.CALCTYPE = Val.ToString(DR["CALCTYPE"]);
					Property.DIAMONDTYPE = Val.ToString(CmbDiamondType.SelectedItem);
					Property.LAB = Val.ToString(cmbLab.SelectedItem);

					Property.APPLICABLEDATE = Val.SqlDate(DTPFromAppDate.Text);
					Property.TOAPPLICABLEDATE = Val.SqlDate(DTPToAppDate.Text);
					Property.ADDLESS1 = Val.Val(txtAddless1.Text);
					Property.ADDLESS2 = Val.Val(txtAddLess2.Text);

					Obj.Save(Property);

					DtabExcelData.AcceptChanges();
				}
			}
			catch (Exception ex)
			{
				Global.Message(ex.Message);
			}
		}

		private void ReptxtServiceType_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				DataRow DR = GrdDet.GetFocusedDataRow();

				if (Global.OnKeyPressEveToPopup(e))
				{
					FrmSearchPopupBox FrmSearchPopupBox = new FrmSearchPopupBox();
					FrmSearchPopupBox.mStrSearchField = "LABSERVICECODENAME,LABSERVICECODE";
					FrmSearchPopupBox.mStrSearchText = e.KeyChar.ToString();
					this.Cursor = Cursors.WaitCursor;
					FrmSearchPopupBox.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LABSERVICECODE);

					FrmSearchPopupBox.mStrColumnsToHide = "LABSERVICECODE_ID";
					//FrmSearchPopupBox.mColumnsToHide = "SEQUENCENO";

					this.Cursor = Cursors.Default;
					FrmSearchPopupBox.ShowDialog();
					e.Handled = true;
					if (FrmSearchPopupBox.DRow != null)
					{
						DR["SERVICETYPE"] = Val.ToString(FrmSearchPopupBox.DRow["LABSERVICECODENAME"]);
						DR["SERVICETYPE_ID"] = Val.ToString(FrmSearchPopupBox.DRow["LABSERVICECODE_ID"]);
					}
					else
					{
						DR["SERVICETYPE"] = Val.ToString(DBNull.Value);
						DR["SERVICETYPE_ID"] = Val.ToString(DBNull.Value);
					}
					FrmSearchPopupBox.Hide();
					FrmSearchPopupBox.Dispose();
					FrmSearchPopupBox = null;
				}
				LabChargesUploadProperty Property = new LabChargesUploadProperty();

				if (Val.ToString(DR["LABCHARGEUPLOAD_ID"]) == "")
				{
					return;
				}
				else
				{
					if (Val.Val(DR["FROMCARAT"]) > Val.Val(DR["TOCARAT"]))
					{
						Global.Message("TOCARAT MUST BE GREATER THEN FROM CARAT.PLEASE CHECK..");
						return;
					}
					if (Val.Val(DR["AMOUNT"]) == 0)
					{
						Global.Message("PLEASE ENTER AMOUNT..");
						GrdDet.FocusedColumn = GrdDet.Columns["AMOUNT"];
						return;
					}

					Property.LABCHARGEUPLOAD_ID = Guid.Parse(Val.ToString(DR["LABCHARGEUPLOAD_ID"]));//.Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(DRow["LABCHARGEUPLOAD_ID"])); ;
					Property.FROMCARAT = Val.Val(DR["FROMCARAT"]);
					Property.TOCARAT = Val.Val(DR["TOCARAT"]);
					Property.FROMCOLOR = Val.ToString(DR["FROMCOLOR"]);
					Property.FROMCOLOR_ID = Val.ToInt(DR["FROMCOLOR_ID"]);
					Property.TOCOLOR = Val.ToString(DR["TOCOLOR"]);
					Property.TOCOLOR_ID = Val.ToInt(DR["TOCOLOR_ID"]);
					Property.SERVICETYPE = Val.ToString(DR["SERVICETYPE"]);
					Property.SERVICETYPE_ID = Val.ToInt(DR["SERVICETYPE_ID"]);
					Property.AMOUNT = Val.Val(DR["AMOUNT"]);
					Property.CALCTYPE = Val.ToString(DR["CALCTYPE"]);
					Property.DIAMONDTYPE = Val.ToString(CmbDiamondType.SelectedItem);
					Property.LAB = Val.ToString(cmbLab.SelectedItem);

					Property.APPLICABLEDATE = Val.SqlDate(DTPFromAppDate.Text);
					Property.TOAPPLICABLEDATE = Val.SqlDate(DTPToAppDate.Text);
					Property.ADDLESS1 = Val.Val(txtAddless1.Text);
					Property.ADDLESS2 = Val.Val(txtAddLess2.Text);

					Obj.Save(Property);

					DtabExcelData.AcceptChanges();
				}
			}
			catch (Exception ex)
			{
				Global.Message(ex.Message);
			}
		}

		private void BtnVerify_Click(object sender, EventArgs e)
		{
			foreach (DataRow DRow in DtabExcelData.Rows)
			{
				this.Cursor = Cursors.WaitCursor;

				double FROMCARAT = 0;
				double TOCARAT = 0;

				FROMCARAT = Val.Val(DRow["FROMCARAT"]);
				TOCARAT = Val.Val(DRow["TOCARAT"]);

				if (FROMCARAT > TOCARAT)
				{
					this.Cursor = Cursors.Default;
					Global.Message("TO CARAT MUST BE GREATER THEN FROM CARAT..PLEASE CHECK.");
					this.Cursor = Cursors.Default;
					return;
				}

				DateTime Date1 = new DateTime(2021,01,01);
				DateTime Date2 = new DateTime(2021, 01, 01);

				Date1 = Convert.ToDateTime(DTPFromAppDate.Text);
				Date2 = Convert.ToDateTime(DTPToAppDate.Text);


				int Res = DateTime.Compare(Date1, Date2);

				if (Res > 0)
				{
					Global.Message("FROM-DATE MUST BE LESS THEN TO-DATE. PLEASE CHEKE APPLICABLE DATE..");
					DtpFromDate.Focus();
					this.Cursor = Cursors.Default;
					
					return;
				}

				//DataTable DTABVALIDATION = Obj.GetDataForValidation(Val.SqlDate(DTPFromAppDate.Text), Val.SqlDate(DTPToAppDate.Text));

				//if (DTABVALIDATION.Rows.Count != 0)
				//{
				//	Global.Message("FROM-APPLICABLEDATE AND TO-PPLICABLEDATE IS ALREADY EXIST PLEASE CHECK..");
				//	DtpFromDate.Focus();
				//	this.Cursor = Cursors.Default;
					
				//	return;
				//}


				if (Val.ToString(DRow["FROMCOLOR"]).Length != 0)
				{
					if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'COLOR'").CopyToDataTable(), "PARANAME", Val.ToString(DRow["FROMCOLOR"]).ToUpper(), "PARA_ID", true)) == 0)
					{
						this.Cursor = Cursors.Default;
						Global.Message("From Color -> '" + Val.ToString(DRow["FROMCOLOR"]) + "' Is Not Valid In Record.");
						GrdDet.FocusedColumn = GrdDet.Columns["FROMCOLOR"];

						return;
					}
				}
				if (Val.ToString(DRow["TOCOLOR"]).Length != 0)
				{
					if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'COLOR'").CopyToDataTable(), "PARANAME", Val.ToString(DRow["TOCOLOR"]).ToUpper(), "PARA_ID", true)) == 0)
					{
						this.Cursor = Cursors.Default;
						Global.Message("To Color -> '" + Val.ToString(DRow["TOCOLOR"]) + "' Is Not Valid In Record.");
						GrdDet.FocusedColumn = GrdDet.Columns["TOCOLOR"];

						return;
					}
				}
				if (Val.ToString(DRow["SERVICETYPE"]).Length != 0)
				{
					if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'LABSERVICECODE'").CopyToDataTable(), "PARANAME", Val.ToString(DRow["SERVICETYPE"]).ToUpper(), "PARA_ID", true)) == 0)
					{
						this.Cursor = Cursors.Default;
						Global.Message("SERVICE TYPE -> '" + Val.ToString(DRow["SERVICETYPE"]) + "' Is Not Valid In Record.");
						GrdDet.FocusedColumn = GrdDet.Columns["SERVICETYPE"];

						return;
					}
				}
				this.Cursor = Cursors.Default;
			}

			this.Cursor = Cursors.WaitCursor;

			string StrRes = "";
			double Addless1 = 0;
			double Addless2 = 0;
			DataTable Dtab = Obj.FindAddLessAmt(Val.SqlDate(DTPFromAppDate.Text),Val.SqlDate(DTPToAppDate.Text));
			if (Dtab.Rows.Count == 0)
			{
				Addless1 = 0;
				Addless2 = 0;
			}
			else
			{
				DataRow DRs = Dtab.Rows[0];

				Addless1 = Val.Val(DRs["ADDLESS1"]);
				Addless2 = Val.Val(DRs["ADDLESS2"]);
			}

			for (int i = 0; i < DtabExcelData.Rows.Count; i++)
			{
				DataRow DR = DtabExcelData.Rows[i];

				LabChargesUploadProperty Property = new LabChargesUploadProperty();
				Property.LABCHARGEUPLOAD_ID = Guid.NewGuid();
				Property.FROMCARAT = Val.Val(DR["FROMCARAT"]);
				Property.TOCARAT = Val.Val(DR["TOCARAT"]);
				Property.FROMCOLOR = Val.ToString(DR["FROMCOLOR"]);
				Property.FROMCOLOR_ID = Val.ToInt(DR["FROMCOLOR_ID"]);
				Property.TOCOLOR = Val.ToString(DR["TOCOLOR"]);
				Property.TOCOLOR_ID = Val.ToInt(DR["TOCOLOR_ID"]);
				Property.SERVICETYPE = Val.ToString(DR["SERVICETYPE"]);
				Property.SERVICETYPE_ID = Val.ToInt(DR["SERVICETYPE_ID"]);
				Property.AMOUNT = Val.Val(DR["AMOUNT"]);
				

				Property.APPLICABLEDATE = Val.SqlDate(DTPFromAppDate.Text);
				Property.TOAPPLICABLEDATE = Val.SqlDate(DTPToAppDate.Text);

				Property.CALCTYPE = Val.ToString(DR["CALCTYPE"]);
				Property.DIAMONDTYPE = Val.ToString(CmbDiamondType.SelectedItem);
				Property.LAB = Val.ToString(cmbLab.SelectedItem);

				if (Val.Val(txtAddless1.Text) == 0)
				{
					Property.ADDLESS1 = Addless1;
				}
				else
				{
					Property.ADDLESS1 = Val.Val(txtAddless1.Text);
				}
				if (Val.Val(txtAddLess2.Text) == 0)
				{
					Property.ADDLESS2 = Addless2;
				}
				else
				{
					Property.ADDLESS2 = Val.Val(txtAddLess2.Text);
				}

				 StrRes = Obj.Save(Property);
				
			}

			if (StrRes == "SUCCESS")
			{
				Global.Message("UPLOAD DATA SAVE SUCCESFULLY");
			}
			else
			{
				Global.Message("ERROR IN SAVE PLEASE CHECK DATA PROPERLY..");
				this.Cursor = Cursors.Default;
				return;
			}

			DtabExcelData.Rows.Clear();
			DtabExcelData.Rows.Add(DtabExcelData.NewRow());

			MainGrid.DataSource = DtabExcelData;
			MainGrid.RefreshDataSource();
			BtnClear.PerformClick();

			BtnVerify.Enabled = true;
			this.Cursor = Cursors.Default;
		}

		private void GrdDet_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
		{
			GrdDet.PostEditor();

			DataRow DR = GrdDet.GetFocusedDataRow();

			double FROMCARAT = 0;
			double TOCARAT = 0;

			if (GrdDet.FocusedColumn.FieldName == "TOCARAT")
			{
				FROMCARAT = Val.Val(DR["FROMCARAT"]);
				TOCARAT = Val.Val(e.Value);

				if (Val.Val(DR["FROMCARAT"]) != 0 && Val.Val(e.Value) != 0)
				{
					if (FROMCARAT > TOCARAT)
					{
						e.ErrorText = "TOCARAT MUST BE GREATER THEN FROMCARAT..";
						e.Valid = false;
						return;
					}

				}
			}
			if (GrdDet.FocusedColumn.FieldName == "FROMCARAT")
			{
				FROMCARAT = Val.Val(e.Value);
				TOCARAT = Val.Val(DR["TOCARAT"]);

				if (Val.Val(e.Value) != 0 && Val.Val(DR["TOCARAT"]) != 0)
				{
					if (FROMCARAT > TOCARAT)
					{
						e.ErrorText = "FROMCARAT MUST BE LESS THEN TOCARAT..";
						e.Valid = false;
						return;
					}
				}
			}
		}

		private void BtnClear_Click(object sender, EventArgs e)
		{
			DTPFromAppDate.Value = DateTime.Now;
			DTPToAppDate.Value = DateTime.Now;
			txtAddless1.Text = Val.ToString(0);
			txtAddLess2.Text = Val.ToString(0);
			txtFileName.Text = "";
			BtnVerify.Enabled = true;
			BtnCalculate.Enabled = true;

			DTPFromAppDate.Enabled = true;
			DTPToAppDate.Enabled = true;

			DtabExcelData.Rows.Clear();
			DtabExcelData.Rows.Add(DtabExcelData.NewRow());

			MainGrid.DataSource = DtabExcelData;
			MainGrid.RefreshDataSource();
		}

		private void BtnExit_Click(object sender, EventArgs e)
		{
			if (Global.Confirm("ARE YOU WANT TO CLOSE THIS FORM.?") == System.Windows.Forms.DialogResult.No)
			{
				return;
			}
			else
			{
				this.Close();
			}
		}

		private void GrdDet_KeyPress(object sender, KeyPressEventArgs e)
		{
			//try
			//{
			//	GrdDet.PostEditor();
			//	DataRow DRow = GrdDet.GetFocusedDataRow();

			//	LabChargesUploadProperty Property = new LabChargesUploadProperty();

			//	if (Val.ToString(DRow["LABCHARGEUPLOAD_ID"]) == "")
			//	{
			//		return;
			//	}
			//	else
			//	{
			//		if (Val.Val(DRow["FROMCARAT"]) > Val.Val(DRow["TOCARAT"]))
			//		{
			//			Global.Message("TOCARAT MUST BE GREATER THEN FROM CARAT.PLEASE CHECK..");
			//			return;
			//		}
			//		if (Val.Val(DRow["AMOUNT"]) == 0)
			//		{
			//			Global.Message("PLEASE ENTER AMOUNT..");
			//			GrdDet.FocusedColumn = GrdDet.Columns["AMOUNT"];
			//			return;
			//		}

			//		Property.LABCHARGEUPLOAD_ID = Guid.Parse(Val.ToString(DRow["LABCHARGEUPLOAD_ID"]));//.Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(DRow["LABCHARGEUPLOAD_ID"])); ;
			//		Property.FROMCARAT = Val.Val(DRow["FROMCARAT"]);
			//		Property.TOCARAT = Val.Val(DRow["TOCARAT"]);
			//		Property.FROMCOLOR = Val.ToString(DRow["FROMCOLOR"]);
			//		Property.FROMCOLOR_ID = Val.ToInt(DRow["FROMCOLOR_ID"]);
			//		Property.TOCOLOR = Val.ToString(DRow["TOCOLOR"]);
			//		Property.TOCOLOR_ID = Val.ToInt(DRow["TOCOLOR_ID"]);
			//		Property.SERVICETYPE = Val.ToString(DRow["SERVICETYPE"]);
			//		Property.SERVICETYPE_ID = Val.ToInt(DRow["SERVICETYPE_ID"]);
			//		Property.AMOUNT = Val.Val(DRow["AMOUNT"]);

			//		Property.APPLICABLEDATE = Val.SqlDate(DTPAppDate.Text);
			//		Property.ADDLESS1 = Val.Val(txtAddless1.Text);
			//		Property.ADDLESS2 = Val.Val(txtAddLess2.Text);

			//		Obj.Save(Property);

			//		DtabExcelData.AcceptChanges();
			//	}
			//}
			//catch (Exception ex)
			//{
			//	Global.Message(ex.Message);
			//}
		}

		private void txtAddLess2_Leave(object sender, EventArgs e)
		{
			for (int i = 0; i < DtabExcelData.Rows.Count; i++)
			{
				DataRow DR = DtabExcelData.Rows[i];

				if (Val.ToString(DR["LABCHARGEUPLOAD_ID"]) == "")
				{
					return;
				}

				LabChargesUploadProperty Property = new LabChargesUploadProperty();
				Property.LABCHARGEUPLOAD_ID = Guid.NewGuid();
				Property.FROMCARAT = Val.Val(DR["FROMCARAT"]);
				Property.TOCARAT = Val.Val(DR["TOCARAT"]);
				Property.FROMCOLOR = Val.ToString(DR["FROMCOLOR"]);
				Property.FROMCOLOR_ID = Val.ToInt(DR["FROMCOLOR_ID"]);
				Property.TOCOLOR = Val.ToString(DR["TOCOLOR"]);
				Property.TOCOLOR_ID = Val.ToInt(DR["TOCOLOR_ID"]);
				Property.SERVICETYPE = Val.ToString(DR["SERVICETYPE"]);
				Property.SERVICETYPE_ID = Val.ToInt(DR["SERVICETYPE_ID"]);
				Property.AMOUNT = Val.Val(DR["AMOUNT"]);
				Property.CALCTYPE = Val.ToString(DR["CALCTYPE"]);
				Property.DIAMONDTYPE = Val.ToString(CmbDiamondType.SelectedItem);
				Property.LAB = Val.ToString(cmbLab.SelectedItem);

				Property.APPLICABLEDATE = Val.SqlDate(DTPFromAppDate.Text);
				Property.TOAPPLICABLEDATE = Val.SqlDate(DTPToAppDate.Text);
				Property.ADDLESS1 = Val.Val(txtAddless1.Text);
				Property.ADDLESS2 = Val.Val(txtAddLess2.Text);

				string StrRes = Obj.Save(Property);
			}
		}

		private void DTPAppDate_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				SendKeys.Send("{TAB}");
			}
		}

		private void txtAddless1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				SendKeys.Send("{TAB}");
			}
		}

		private void txtAddLess2_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				SendKeys.Send("{TAB}");
			}
		}

		private void txtFileName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				SendKeys.Send("{TAB}");
			}
		}

		private void deleteSelectedAmountToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (GrdDet.FocusedRowHandle >= 0)
				{
					if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY") == System.Windows.Forms.DialogResult.Yes)
					{
						DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);

						Guid ID = Guid.Parse(Val.ToString(Drow["LABCHARGEUPLOAD_ID"]));

						string StrRes = Obj.Delete(ID);

						if (StrRes == "SUCCESS")
						{
							Global.Message("ENTRY DELETED SUCCESSFULLY");

							BtnClear.PerformClick();
							MainGrid.RefreshDataSource();
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

		private void txtAddless1_Validating(object sender, CancelEventArgs e)
		{
			if (txtAddless1.Text == Val.ToString(0))
			{
				return;
			}

			string STRDATA = Obj.UpdateAddlessValue(Val.SqlDate(DTPFromAppDate.Text), Val.Val(txtAddless1.Text), Val.Val(txtAddLess2.Text));
		}

		private void txtAddLess2_Validating(object sender, CancelEventArgs e)
		{
			if (txtAddLess2.Text == Val.ToString(0))
			{
				return;
			}
			string STRDATA = Obj.UpdateAddlessValue(Val.SqlDate(DTPFromAppDate.Text), Val.Val(txtAddless1.Text), Val.Val(txtAddLess2.Text));
		}

		private void BtnClose_Click(object sender, EventArgs e)
		{
			if (Global.Confirm("ARE YOU WANT TO CLOSE THIS FORM..?") == System.Windows.Forms.DialogResult.No)
			{
				return;
			}
			else
			{
				this.Close();
			}
		}

		private void BtnSearch_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			DtabGetData = Obj.GetData(Val.SqlDate(DtpFromDate.Text), Val.SqlDate(DtpToDate.Text), Val.ToString(CmbDiamondType.SelectedItem), Val.ToString(cmbLab.SelectedItem));

			MainGrdDet.DataSource = DtabGetData;
			MainGrdDet.RefreshDataSource();

			GrdDetDetail.BestFitColumns();
			this.Cursor = Cursors.Default;
		}

		private void GrdDetDetail_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
		{
			try
			{
				if (e.Clicks == 2)
				{
					this.Cursor = Cursors.WaitCursor;

					string StrDate = "",StrToDate;
					DataRow DR = GrdDetDetail.GetFocusedDataRow();

					StrDate = Val.SqlDate(Val.ToString(DR["APPLICABLEDATE"]));
					StrToDate = Val.SqlDate(Val.ToString(DR["APPLICABLETODATE"]));
					CmbDiamondType.Text = Val.ToString(DR["DIAMONDTYPE"]);
					cmbLab.Text = Val.ToString(DR["LAB"]);

					DtabExcelData = Obj.GetShowDatafETCH(StrDate,StrToDate,Val.ToString(CmbDiamondType.SelectedItem), Val.ToString(cmbLab.SelectedItem));

					DataRow dro = DtabExcelData.Rows[0];
					DTPFromAppDate.Text = Val.ToString(dro["APPLICABLEDATE"]);
					DTPToAppDate.Text = Val.ToString(dro["APPLICABLETODATE"]);
					txtAddless1.Text = Val.ToString(dro["ADDLESS1"]);
					txtAddLess2.Text = Val.ToString(dro["ADDLESS2"]);
					DtabExcelData.Rows.Add(DtabExcelData.NewRow());

					MainGrid.DataSource = DtabExcelData;
					MainGrdDet.RefreshDataSource();

					xtraTabControl1.SelectedTabPageIndex = 0;
					DTPFromAppDate.Enabled = false;
					DTPToAppDate.Enabled = false;
					this.Cursor = Cursors.Default;


				}
			}
			catch (Exception ex)
			{
				Global.Message(ex.Message);
			}
		}

		private void BtnShow_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			DtabExcelData = Obj.GetShowData(Val.SqlDate(DTPFromAppDate.Text), Val.SqlDate(DTPToAppDate.Text), Val.ToString(CmbDiamondType.SelectedItem), Val.ToString(cmbLab.SelectedItem));

			if (DtabExcelData.Rows.Count == 0)
			{
				txtAddless1.Text = Val.ToString(0);
				txtAddLess2.Text = Val.ToString(0);
				//Global.Message("DATA NOT FOUND BETWEEN FROMAPPLICABLE DATE AND TOAPPLICABLE DATE");
				//DtpFromDate.Focus();
				//return;
			}
			else
			{
				DataRow dro = DtabExcelData.Rows[0];
				DTPFromAppDate.Text = Val.ToString(dro["APPLICABLEDATE"]);
				DTPToAppDate.Text = Val.ToString(dro["APPLICABLETODATE"]);
				txtAddless1.Text = Val.ToString(dro["ADDLESS1"]);
				txtAddLess2.Text = Val.ToString(dro["ADDLESS2"]);
				CmbDiamondType.Text = Val.ToString(dro["DIAMONDTYPE"]);
				cmbLab.Text = Val.ToString(dro["LAB"]);

				//DTPFromAppDate.Enabled = false;
				//DTPToAppDate.Enabled = false;
			}
			DtabExcelData.Rows.Add(DtabExcelData.NewRow());


			MainGrid.DataSource = DtabExcelData;
			MainGrid.RefreshDataSource();

			this.Cursor = Cursors.Default;
		}

		private void DtpFromDate_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				SendKeys.Send("{TAB}");
			}
		}

		private void DtpToDate_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				SendKeys.Send("{TAB}");
			}
		}

		private void GrdDet_CellValueChanged(object sender, CellValueChangedEventArgs e)
		{
			try
			{
				GrdDet.PostEditor();
				DataRow DRow = GrdDet.GetFocusedDataRow();

				LabChargesUploadProperty Property = new LabChargesUploadProperty();
				string sTR = "";
				if (Val.ToString(DRow["LABCHARGEUPLOAD_ID"]) == "")
				{
					return;
				}
				else
				{
					if (Val.Val(DRow["FROMCARAT"]) > Val.Val(DRow["TOCARAT"]))
					{
						Global.Message("TOCARAT MUST BE GREATER THEN FROM CARAT.PLEASE CHECK..");
						return;
					}
					if (Val.Val(DRow["AMOUNT"]) == 0)
					{
						Global.Message("PLEASE ENTER AMOUNT..");
						GrdDet.FocusedColumn = GrdDet.Columns["AMOUNT"];
						return;
					}



					Property.LABCHARGEUPLOAD_ID = Guid.Parse(Val.ToString(DRow["LABCHARGEUPLOAD_ID"]));//.Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(DRow["LABCHARGEUPLOAD_ID"])); ;
					Property.FROMCARAT = Val.Val(DRow["FROMCARAT"]);
					Property.TOCARAT = Val.Val(DRow["TOCARAT"]);
					Property.FROMCOLOR = Val.ToString(DRow["FROMCOLOR"]);
					Property.FROMCOLOR_ID = Val.ToInt(DRow["FROMCOLOR_ID"]);
					Property.TOCOLOR = Val.ToString(DRow["TOCOLOR"]);
					Property.TOCOLOR_ID = Val.ToInt(DRow["TOCOLOR_ID"]);
					Property.SERVICETYPE = Val.ToString(DRow["SERVICETYPE"]);
					Property.SERVICETYPE_ID = Val.ToInt(DRow["SERVICETYPE_ID"]);
					Property.AMOUNT = Val.Val(DRow["AMOUNT"]);

					Property.APPLICABLEDATE = Val.SqlDate(DTPFromAppDate.Text);
					Property.TOAPPLICABLEDATE = Val.SqlDate(DTPToAppDate.Text);
					Property.ADDLESS1 = Val.Val(txtAddless1.Text);
					Property.ADDLESS2 = Val.Val(txtAddLess2.Text);
					Property.CALCTYPE = Val.ToString(DRow["CALCTYPE"]);
					Property.DIAMONDTYPE = Val.ToString(CmbDiamondType.SelectedItem);
					Property.LAB = Val.ToString(cmbLab.SelectedItem);

					sTR = Obj.Save(Property);

					DtabExcelData.AcceptChanges();
				}
				if(sTR == "SUCCESS")
                {
					Global.Message("RECODE INSERTED SUCESSFULLY..");
                }
			}
			catch (Exception ex)
			{
				Global.Message(ex.Message);
			}

		}

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
