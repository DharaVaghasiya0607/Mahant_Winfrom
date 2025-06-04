
using MahantExport.Utility;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using Google.API.Translate;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace MahantExport.Pricing
{
    public partial class FrmPriceView : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_ParcelParameterDiscount ObjTrn = new BOTRN_ParcelParameterDiscount();
        BOFormPer ObjPer = new BOFormPer();
        BODevGridSelection ObjGridSelection;

        DataTable DtabPara = new DataTable();
        DataTable DtabPriceView = new DataTable();

        #region Property Settings

        public FrmPriceView()
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

            DtabPara = new BOMST_Parameter().GetParameterData();

            cmbShape.Properties.DataSource = DtabPara.Select("PARATYPE = 'SHAPE'").CopyToDataTable();
            cmbShape.Properties.ValueMember = "PARA_ID";
            cmbShape.Properties.DisplayMember = "SHORTNAME";

            cmbColor.Properties.DataSource = DtabPara.Select("PARATYPE = 'COLOR'").CopyToDataTable();
            cmbColor.Properties.ValueMember = "PARA_ID";
            cmbColor.Properties.DisplayMember = "SHORTNAME";

            cmbClarity.Properties.DataSource = DtabPara.Select("PARATYPE = 'CLARITY'").CopyToDataTable();
            cmbClarity.Properties.ValueMember = "PARA_ID";
            cmbClarity.Properties.DisplayMember = "SHORTNAME";

            CmbCut.Properties.DataSource = DtabPara.Select("PARATYPE = 'CUT'").CopyToDataTable();
            CmbCut.Properties.ValueMember = "PARA_ID";
            CmbCut.Properties.DisplayMember = "SHORTNAME";

            cmbPol.Properties.DataSource = DtabPara.Select("PARATYPE = 'POLISH'").CopyToDataTable();
            cmbPol.Properties.ValueMember = "PARA_ID";
            cmbPol.Properties.DisplayMember = "SHORTNAME";

            CmbSym.Properties.DataSource = DtabPara.Select("PARATYPE = 'SYMMETRY'").CopyToDataTable();
            CmbSym.Properties.ValueMember = "PARA_ID";
            CmbSym.Properties.DisplayMember = "SHORTNAME";

            CmbFL.Properties.DataSource = DtabPara.Select("PARATYPE = 'FLUORESCENCE'").CopyToDataTable();
            CmbFL.Properties.ValueMember = "PARA_ID";
            CmbFL.Properties.DisplayMember = "SHORTNAME";

            cmbLab.Properties.DataSource = DtabPara.Select("PARATYPE = 'LAB'").CopyToDataTable();
            cmbLab.Properties.ValueMember = "PARA_ID";
            cmbLab.Properties.DisplayMember = "SHORTNAME";

            DataTable DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PARAALL);
            DataRow[] DR = DTab.Select("PARATYPE='WEBSTATUS'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "PARANAME";
                CmbWebStatus.Properties.DataSource = DTTemp.DefaultView.ToTable();
                CmbWebStatus.Properties.ValueMember = "SHORTNAME";
                CmbWebStatus.Properties.DisplayMember = "SHORTNAME";
            }

            this.Show();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjTrn);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        #endregion

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                LiveStockProperty LStockProperty = new LiveStockProperty();

                LStockProperty.MULTYSHAPE_ID = cmbShape.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYCOLOR_ID = cmbColor.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYCLARITY_ID = cmbClarity.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYCUT_ID = CmbCut.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYPOL_ID = cmbPol.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYSYM_ID = CmbSym.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYFL_ID = CmbFL.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYLAB_ID = cmbLab.EditValue.ToString().Replace(" ", "");
                LStockProperty.WEBSTATUS = CmbWebStatus.EditValue.ToString().Replace(" ", null);

                LStockProperty.STOCKNO = Val.ToString(txtStoneNo.Text);
                LStockProperty.LABREPORTNO = Val.ToString(txtreportno.Text);

                if (DtpAvailbleFromDate.InvokeRequired)
                {
                    DtpAvailbleFromDate.Invoke(new MethodInvoker(() =>
                    {
                        if (DtpAvailbleFromDate.Checked == true)
                        {
                            LStockProperty.AVAILBLEFROMDATE = Val.SqlDate(DtpAvailbleFromDate.Value.ToShortDateString());
                        }
                    }));
                }
                else
                {
                    if (DtpAvailbleFromDate.Checked == true)
                    {
                        LStockProperty.AVAILBLEFROMDATE = Val.SqlDate(DtpAvailbleFromDate.Value.ToShortDateString());
                    }
                }

                if (DtpAvailbleToDate.InvokeRequired)
                {
                    DtpAvailbleToDate.Invoke(new MethodInvoker(() =>
                    {
                        if (DtpAvailbleToDate.Checked == true)
                        {
                            LStockProperty.AVAILBLETODATE = Val.SqlDate(DtpAvailbleToDate.Value.ToShortDateString());
                        }
                    }));
                }
                else
                {
                    if (DtpAvailbleToDate.Checked == true)
                    {
                        LStockProperty.AVAILBLETODATE = Val.SqlDate(DtpAvailbleToDate.Value.ToShortDateString());
                    }
                }

                if (DtpSalesFromDate.InvokeRequired)
                {
                    DtpSalesFromDate.Invoke(new MethodInvoker(() =>
                    {
                        if (DtpSalesFromDate.Checked == true)
                        {
                            LStockProperty.SALESFROMDATE = Val.SqlDate(DtpSalesFromDate.Value.ToShortDateString());
                        }
                    }));
                }
                else
                {
                    if (DtpSalesFromDate.Checked == true)
                    {
                        LStockProperty.SALESFROMDATE = Val.SqlDate(DtpSalesFromDate.Value.ToShortDateString());
                    }
                }


                if (DtpSalesToDate.InvokeRequired)
                {
                    DtpSalesToDate.Invoke(new MethodInvoker(() =>
                    {
                        if (DtpSalesToDate.Checked == true)
                        {
                            LStockProperty.SALESTODATE = Val.SqlDate(DtpSalesToDate.Value.ToShortDateString());
                        }
                    }));
                }
                else
                {
                    if (DtpSalesToDate.Checked == true)
                    {
                        LStockProperty.SALESTODATE = Val.SqlDate(DtpSalesToDate.Value.ToShortDateString());
                    }
                }

                DtabPriceView = ObjTrn.GetPricePriceViewGetData(LStockProperty);

                if (DtabPriceView.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                if (MainGrid.InvokeRequired)
                {
                    MainGrid.Invoke(new MethodInvoker(() =>
                    {
                        MainGrid.DataSource = DtabPriceView;
                        GrdDet.BestFitColumns();
                    }));
                }
                else
                {
                    MainGrid.DataSource = DtabPriceView;
                    GrdDet.BestFitColumns();
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }


        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressPanel1.Visible = false;
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.CancelAsync();
                }
                else
                {
                    progressPanel1.Visible = true;
                    backgroundWorker1.RunWorkerAsync();

                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtStoneNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = "";
                if (txtStoneNo.Text.Trim().Contains("\t\n"))
                {
                    str1 = txtStoneNo.Text.Trim().Replace("\t\n", ",");
                }
                else
                {
                    str1 = txtStoneNo.Text.Trim().Replace("\n", ",");
                    str1 = str1.Replace("\r", "");
                }

                txtStoneNo.Text = str1;
                txtStoneNo.Select(txtStoneNo.Text.Length, 0);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtreportno_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = "";
                if (txtreportno.Text.Trim().Contains("\t\n"))
                {
                    str1 = txtreportno.Text.Trim().Replace("\t\n", ",");
                }
                else
                {
                    str1 = txtreportno.Text.Trim().Replace("\n", ",");
                    str1 = str1.Replace("\r", "");
                }

                txtreportno.Text = str1;
                txtreportno.Select(txtreportno.Text.Length, 0);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
    }
}
