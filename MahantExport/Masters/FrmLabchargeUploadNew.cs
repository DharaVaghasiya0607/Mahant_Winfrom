using BusLib.Configuration;
using BusLib.Pricing;
using DevExpress.XtraEditors;
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

namespace MahantExport.Masters
{
    public partial class FrmLabchargeUploadNew : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        DataTable DtabExcelData = new DataTable();
        DataTable DtabPara = new DataTable();
        BOFormPer ObjPer = new BOFormPer();
        DataTable DtabGetData = new DataTable();
        BOMST_LabChargesUpload Obj = new BOMST_LabChargesUpload();

        public FrmLabchargeUploadNew()
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

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFileName.Text == "" || txtFileName.Text.Trim().Equals(string.Empty))
                {
                    Global.Message("Please Select Excel File To Upload");
                    txtFileName.Focus();
                    return;
                }
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
    }
}