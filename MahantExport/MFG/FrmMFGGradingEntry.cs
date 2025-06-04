using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OfficeOpenXml;
using Spire.Xls;
using DevExpress.Data;
using DevExpress.XtraPrintingLinks;
using System.Drawing.Printing;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using BusLib.Transaction;
using System.Xml;
using BusLib.Rapaport;
using BusLib.Report;
using DevExpress.XtraGrid.Views.Grid;
using MahantExport.Utility;

namespace MahantExport.Grading
{
    public partial class FrmMFGGradingEntry : DevControlLib.cDevXtraForm
    {
        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOFindRap ObjRap = new BOFindRap();
        DataTable DTabGrdData = new DataTable();
        DataTable DTabPara = new DataTable();

        DataTable DtabSum = new DataTable();
        BOFormPer ObjPer = new BOFormPer();

        #region Property Settings

        public FrmMFGGradingEntry()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;
            deleteSelectedAmountToolStripMenuItem.Enabled = ObjPer.ISDELETE;

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            DTabPara = new BOMST_Parameter().GetParameterData();

            DTPGradingDate.Value = DateTime.Now;
            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;



            string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDet.Name);

            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDet.RestoreLayoutFromStream(stream);
            }



            this.Show();
        }

        public void FillListControls()
        {
            DataTable DTab = new DataTable();
            DTab = DTabPara.Select("PARATYPE = 'TABLEINC'").CopyToDataTable();
            repCmbTableInc.DataSource = DTab;
            repCmbTableInc.DisplayMember = "SHORTNAME";
            repCmbTableInc.ValueMember = "PARA_ID";

            DTab = DTabPara.Select("PARATYPE = 'SIDETABLEINC'").CopyToDataTable();
            RepCmbSideTable.DataSource = DTab;
            RepCmbSideTable.DisplayMember = "SHORTNAME";
            RepCmbSideTable.ValueMember = "PARA_ID";
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = false;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            GrdDet.BestFitColumns();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Trn_SinglePrdProperty Property = new Trn_SinglePrdProperty();
                Property.MFGGradingNo = Val.ToInt64(txtGradingNo.Text);
                DTabGrdData = ObjStock.MFGGradingGetDetail(Property);

                BtnAddNewRow_Click(null, null);
                FillListControls();



                MainGrd.DataSource = DTabGrdData;
                GrdDet.RefreshData();
                GrdDet.BestFitMaxRowCount = 500;
                GrdDet.BestFitColumns();

                GrdDet.Columns["REMARK"].Width = 250;


                calculate(); //hinal 02-01-2022


                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void repTextPopup_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle < 0)
                    return;

                DataTable DTab = new DataTable();

                if (GrdDet.FocusedColumn.FieldName.ToUpper() == "SHAPENAME")
                    DTab = DTabPara.Select("PARATYPE = 'SHAPE'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "COLORNAME" || GrdDet.FocusedColumn.FieldName.ToUpper() == "MFGCOLORNAME")
                    DTab = DTabPara.Select("PARATYPE = 'COLOR'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "CLARITYNAME" || GrdDet.FocusedColumn.FieldName.ToUpper() == "MFGCLARITYNAME")
                    DTab = DTabPara.Select("PARATYPE = 'CLARITY'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "CUTNAME" || GrdDet.FocusedColumn.FieldName.ToUpper() == "MFGCUT")
                    DTab = DTabPara.Select("PARATYPE = 'CUT'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "POLNAME" || GrdDet.FocusedColumn.FieldName.ToUpper() == "MFGPOLISH")
                    DTab = DTabPara.Select("PARATYPE = 'POLISH'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "SYMNAME" || GrdDet.FocusedColumn.FieldName.ToUpper() == "MFGSYM")
                    DTab = DTabPara.Select("PARATYPE = 'SYMMETRY'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "FLNAME" || GrdDet.FocusedColumn.FieldName.ToUpper() == "MFGFL")
                    DTab = DTabPara.Select("PARATYPE = 'FLUORESCENCE'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "LOCATIONNAME")
                    DTab = DTabPara.Select("PARATYPE = 'LOCATION'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "SIZENAME")
                    DTab = DTabPara.Select("PARATYPE = 'SIZE'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "LABNAME")
                    DTab = DTabPara.Select("PARATYPE = 'LAB'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "COLORSHADENAME")
                    DTab = DTabPara.Select("PARATYPE = 'COLORSHADE'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "MILKYNAME")
                    DTab = DTabPara.Select("PARATYPE = 'MILKY'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "EYECLEANNAME")
                    DTab = DTabPara.Select("PARATYPE = 'EYECLEAN'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "LUSTERNAME")
                    DTab = DTabPara.Select("PARATYPE = 'LUSTER'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "HANAME")
                    DTab = DTabPara.Select("PARATYPE = 'HEARTANDARROW'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "CULETNAME")
                    DTab = DTabPara.Select("PARATYPE = 'CULET'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "GIRDLENAME")
                    DTab = DTabPara.Select("PARATYPE = 'GIRDLE'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "FROMGIRDLENAME")
                    DTab = DTabPara.Select("PARATYPE = 'GIRDLE'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "TOGIRDLENAME")
                    DTab = DTabPara.Select("PARATYPE = 'GIRDLE'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "TABLEINCNAME")
                    DTab = DTabPara.Select("PARATYPE = 'TABLEINC'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "TABLEOPENNAME")
                    DTab = DTabPara.Select("PARATYPE = 'TABLEOPENINC'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "SIDETABLENAME")
                    DTab = DTabPara.Select("PARATYPE = 'SIDETABLEINC'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "SIDEOPENNAME")
                    DTab = DTabPara.Select("PARATYPE = 'SIDEOPENINC'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "TABLEBLACKNAME")
                    DTab = DTabPara.Select("PARATYPE = 'TABLEBLACKINC'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "SIDEBLACKNAME")
                    DTab = DTabPara.Select("PARATYPE = 'SIDEBLACKINC'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "REDSPORTNAME")
                    DTab = DTabPara.Select("PARATYPE = 'REDSPORTINC'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "GIRDLEINCNAME")
                    DTab = DTabPara.Select("PARATYPE = 'GIRDLEINC'").CopyToDataTable();

                DTab.DefaultView.Sort = "SEQUENCENO";
                DTab = DTab.DefaultView.ToTable();

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearchPopupBox = new FrmSearchPopupBox();
                    FrmSearchPopupBox.mStrSearchField = "PARACODE,PARANAME,SHORTNAME";
                    FrmSearchPopupBox.mStrSearchText = e.KeyChar.ToString();
                    FrmSearchPopupBox.mBoolSearchWithoutLike = true;
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearchPopupBox.mDTab = DTab;
                    FrmSearchPopupBox.mStrColumnsToHide = "PARA_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearchPopupBox.ShowDialog();
                    e.Handled = true;
                    if (FrmSearchPopupBox.DRow != null)
                    {
                        GrdDet.SetFocusedRowCellValue(GrdDet.FocusedColumn.FieldName, (Val.ToString(FrmSearchPopupBox.DRow["SHORTNAME"])));
                    }
                    FrmSearchPopupBox.Hide();
                    FrmSearchPopupBox.Dispose();
                    FrmSearchPopupBox = null;
                }

                if (GrdDet.FocusedColumn.FieldName.ToUpper() == "LABNAME")
                {
                    BtnSave_Click(null, null);
                    if (DTabGrdData.Rows[0]["RETURNTYPE"].ToString() == "SUCCESS")
                    {
                        if (GrdDet.IsLastRow || Val.ToString(GrdDet.GetFocusedRowCellValue("LABNAME")) != "")
                        {
                            BtnAddNewRow_Click(null, null);
                            GrdDet.Focus();
                            GrdDet.FocusedRowHandle = GrdDet.RowCount -1;
                            GrdDet.FocusedColumn = GrdDet.Columns["KAPANNAME"];
                        }
                        else
                        {
                            BtnSave.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtStoneNo_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtStoneNo.Focus())
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    PasteData = Convert.ToString(PasteclipData.GetData(System.Windows.Forms.DataFormats.Text));
                }
            }
        }

        private void txtStoneNo_TextChanged(object sender, EventArgs e)
        {
            if (txtStoneNo.Text.Length > 0 && Convert.ToString(PasteData) != "")
            {
                txtStoneNo.SelectAll();
                String str1 = PasteData.Replace("\r\n", ",");                   //data.Replace(\n, ",");
                str1 = str1.Trim();
                str1 = str1.TrimEnd();
                str1 = str1.TrimStart();
                str1 = str1.TrimEnd(',');
                str1 = str1.TrimStart(',');
                txtStoneNo.Text = str1;
                PasteData = "";
            }
        }

        private void txtStoneNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                IDataObject clipData = Clipboard.GetDataObject();
                String Data = Convert.ToString(clipData.GetData(System.Windows.Forms.DataFormats.Text));
                String str1 = Data.Replace("\r\n", ",");                   //data.Replace(\n, ",");
                str1 = str1.Trim();
                str1 = str1.TrimEnd();
                str1 = str1.TrimStart();
                str1 = str1.TrimEnd(',');
                str1 = str1.TrimStart(',');
                txtStoneNo.Text = str1;
            }
        }

        private void GrdDet_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }
            calculate();

            DataRow DRow = GrdDet.GetDataRow(e.RowHandle);

            switch (e.Column.FieldName)
            {

                case "PARTYSTOCKNO":
                    GrdDet.PostEditor();
                    string StockNo = Val.ToString(GrdDet.GetFocusedRowCellValue("PARTYSTOCKNO"));

                    if (CheckDuplicate("PARTYSTOCKNO", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle))
                    {
                        GrdDet.SetFocusedRowCellValue("PARTYSTOCKNO", "");
                        return;
                    }

                    DataTable DtabStkNo = ObjStock.CheckStockNo(StockNo);
                    if (DtabStkNo.Rows.Count >= 1)
                    {
                        Global.Message("[" + StockNo + "] : StockNo Is Already Exists..");
                        GrdDet.SetFocusedRowCellValue("PARTYSTOCKNO", "");
                        return;
                    }
                    break;

                case "KAPANNAME":
                case "PACKETNO":
                //case "TAG":
                    string KapanName = Val.ToString(DRow["KAPANNAME"]);
                    string PacketNo = Val.ToString(DRow["PACKETNO"]);
                    string Tag = Val.ToString(DRow["TAG"]);

                    if (CheckDuplicateKapan("KAPANNAME", Val.ToString(KapanName), GrdDet.FocusedRowHandle, "PACKETNO", PacketNo, "TAG", Tag))
                    {
                        GrdDet.SetFocusedRowCellValue(e.Column.FieldName, "");
                        return;
                    }

                    DataTable DtabKapan = ObjStock.CheckKapan(KapanName, PacketNo, Tag);
                    if (DtabKapan.Rows.Count >= 1)
                    {
                        Global.Message("This Type Of Entry Is Already Exists..");
                        GrdDet.SetFocusedRowCellValue(e.Column.FieldName, "");
                        return;
                    }

                    break;

                case "MFGCOLORNAME":
                case "MFGCLARITYNAME":
                case "SHAPENAME":
                case "CARAT":
                case "CUTNAME":
                case "POLNAME":
                case "SYMNAME":
                case "FLNAME":
                    Trn_RapSaveProperty Property = new Trn_RapSaveProperty();

                    Property.SHAPE_ID = Val.ToString(DRow["SHAPENAME"]) != "" ? Val.ToInt(DTabPara.Select("ParaType='SHAPE' And ShortName='" + Val.ToString(DRow["SHAPENAME"]) + "'")[0]["PARA_ID"]) : 0;
                    Property.COLOR_ID = Val.ToString(DRow["MFGCOLORNAME"]) != "" ? Val.ToInt(DTabPara.Select("ParaType='COLOR' And ShortName='" + Val.ToString(DRow["MFGCOLORNAME"]) + "'")[0]["PARA_ID"]) : 0;
                    Property.CLARITY_ID = Val.ToString(DRow["MFGCLARITYNAME"]) != "" ? Val.ToInt(DTabPara.Select("ParaType='CLARITY' And ShortName='" + Val.ToString(DRow["MFGCLARITYNAME"]) + "'")[0]["PARA_ID"]) : 0;

                    Property.CUT_ID = Val.ToString(DRow["CUTNAME"]) != "" ? Val.ToInt(DTabPara.Select("ParaType='CUT' And ShortName='" + Val.ToString(DRow["CUTNAME"]) + "'")[0]["PARA_ID"]) : 0;
                    Property.POL_ID = Val.ToString(DRow["POLNAME"]) != "" ? Val.ToInt(DTabPara.Select("ParaType='POLISH' And ShortName='" + Val.ToString(DRow["POLNAME"]) + "'")[0]["PARA_ID"]) : 0;
                    Property.SYM_ID = Val.ToString(DRow["SYMNAME"]) != "" ? Val.ToInt(DTabPara.Select("ParaType='SYMMETRY' And ShortName='" + Val.ToString(DRow["SYMNAME"]) + "'")[0]["PARA_ID"]) : 0;
                    Property.FL_ID = Val.ToString(DRow["FLNAME"]) != "" ? Val.ToInt(DTabPara.Select("ParaType='FLUORESCENCE' And ShortName='" + Val.ToString(DRow["FLNAME"]) + "'")[0]["PARA_ID"]) : 0;
                    Property.CARAT = Val.Val(DRow["CARAT"]);
                    // Property.DISCOUNT = Val.Val(DRow["MFGDISCOUNT"]);
                    Property.RAPDATE = Val.SqlDate(DTPGradingDate.Value.ToShortDateString());
                    Property = ObjRap.FindRap(Property);

                    DTabGrdData.Rows[e.RowHandle]["MFGRAPAPORT"] = Property.RAPAPORT;
                    DTabGrdData.Rows[e.RowHandle]["MFGPRICEPERCARAT"] = Math.Round(Property.RAPAPORT + ((Property.RAPAPORT * Val.Val(DRow["MFGDISCOUNT"])) / 100));
                    DTabGrdData.Rows[e.RowHandle]["MFGAMOUNT"] = Math.Round(Property.CARAT * Val.Val(DTabGrdData.Rows[e.RowHandle]["MFGPRICEPERCARAT"]), 2);

                    DTabGrdData.AcceptChanges();

                    break;

                case "MFGDISCOUNT":

                    double Rapaport = Val.Val(DRow["MFGRAPAPORT"]);
                    double Carat = Val.Val(DRow["CARAT"]);

                    DTabGrdData.Rows[e.RowHandle]["MFGRAPAPORT"] = Rapaport;
                    DTabGrdData.Rows[e.RowHandle]["MFGPRICEPERCARAT"] = Math.Round(Rapaport + ((Rapaport * Val.Val(DRow["MFGDISCOUNT"])) / 100));
                    DTabGrdData.Rows[e.RowHandle]["MFGAMOUNT"] = Math.Round(Carat * Val.Val(DTabGrdData.Rows[e.RowHandle]["MFGPRICEPERCARAT"]), 2);

                    break;

                case "LENGTH":
                case "WIDTH":
                    if (Val.ToString(DRow["SHAPENAME"]) != "RBC") //Round : Only Fancy Stone ma Ratio Display thase : 24-12-2021
                    {
                        double Length = Val.Val(DRow["LENGTH"]);
                        double Width = Val.Val(DRow["WIDTH"]);
                        double Ratio = Width == 0 ? 0 : Math.Round((Length / Width), 2);
                        DTabGrdData.Rows[e.RowHandle]["RATIO"] = Ratio;
                    }
                    else
                    {
                        DTabGrdData.Rows[e.RowHandle]["RATIO"] = 0.00;
                    }
                    break;

                case "DIAMIN": // K : 09/12/2022
                case "DIAMAX":
                    if (Val.ToString(DRow["SHAPENAME"]) != "RBC")
                    {
                        double Diamin = Val.Val(DRow["DIAMIN"]);
                        double DiaMax = Val.Val(DRow["DIAMAX"]);
                        DTabGrdData.Rows[e.RowHandle]["LENGTH"] = DiaMax;
                        DTabGrdData.Rows[e.RowHandle]["WIDTH"] = Diamin;
                        double Ratio = Diamin == 0 ? 0 : Math.Round((DiaMax / Diamin), 2);
                        DTabGrdData.Rows[e.RowHandle]["RATIO"] = Ratio;
                    }
                    break;

                default:
                    break;
            }
        }
        private bool CheckDuplicateKapan(string Kapanname, string kapanvalue, int IntRow, string PacketNo, string PacketValue, string Tag, string TagValue) // K : 09/12/2022
        {
            string Kapan = Val.ToString(GrdDet.GetFocusedRowCellValue("KAPANNAME"));
            string PktNo = Val.ToString(GrdDet.GetFocusedRowCellValue("PACKETNO"));
            //string TAG = Val.ToString(GrdDet.GetFocusedRowCellValue("TAG"));
            if (Val.ToString(kapanvalue).Trim().Equals(string.Empty)
                || Val.ToString(PacketValue).Trim().Equals(string.Empty)
                //|| Val.ToString(TagValue).Trim().Equals(string.Empty)
                )
                return false;

            var Result = from row in DTabGrdData.AsEnumerable()
                         where Val.ToString(row[Kapanname]).ToUpper() == Val.ToString(kapanvalue).ToUpper()
                         && Val.ToString(row[PacketNo]).ToUpper() == Val.ToString(PacketValue).ToUpper()
                         //&& Val.ToString(row[Tag]).ToUpper() == Val.ToString(TagValue).ToUpper()
                         & row.Table.Rows.IndexOf(row) != IntRow
                         select row;

            if (Result.Any())
            {
                Global.Message("This Selection Type Entry Is Already Exists.");
                //GrdDet.SetFocusedRowCellValue(GrdDet.FocusedColumn.FieldName, "");
                return true;
            }            
            return false;
        }

        private void BtnAddNewRow_Click(object sender, EventArgs e)
        {
            DTabGrdData.Rows.Add(DTabGrdData.NewRow());
            //calculate();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = "";
                //Validation pending

                if (ValSave() == false)
                {
                    return;
                }


                foreach (DataRow DRow in DTabGrdData.Rows)
                {
                    if (Val.ToString(DRow["KAPANNAME"]).Trim().Equals(string.Empty) && Val.ToString(DRow["PACKETNO"]).Trim().Equals(string.Empty))
                    {
                        continue;
                    }
                    string StrStoneNo = Val.ToString(DRow["PARTYSTOCKNO"]);
                    if (StrStoneNo != "" && Val.Val(DRow["CARAT"]) == 0)
                    {
                        Global.Message(StrStoneNo + " : Carat Is Missing");
                        return;
                    }
                    if (StrStoneNo != "" && Val.ToString(DRow["SHAPENAME"]) == "")
                    {
                        Global.Message(StrStoneNo + " : Shape Is Missing");
                        return;
                    }
                    if (StrStoneNo != "" && Val.ToString(DRow["KAPANNAME"]) == "")
                    {
                        Global.Message(StrStoneNo + " : Kapan Is Missing");
                        return;
                    }
                }


                this.Cursor = Cursors.WaitCursor;

                int IntSrNo = 0;

                foreach (DataRow DRow in DTabGrdData.Rows)
                {
                    if (Val.ToString(DRow["KAPANNAME"]).Trim().Equals(string.Empty) && Val.ToString(DRow["PACKETNO"]).Trim().Equals(string.Empty))
                    {
                        continue;
                    }
                    IntSrNo++;
                    DRow["MFGGRADINGENTRYSRNO"] = IntSrNo;

                    string StrMeasurement = Val.ToString(DRow["LENGTH"]);
                    if (Val.ToString(DRow["WIDTH"]).Length != 0)
                    {
                        StrMeasurement = StrMeasurement + "*" + Val.ToString(DRow["WIDTH"]);
                    }
                    if (Val.ToString(DRow["HEIGHT"]).Length != 0)
                    {
                        StrMeasurement = StrMeasurement + "*" + Val.ToString(DRow["HEIGHT"]);
                    }
                    DRow["MEASUREMENT"] = StrMeasurement;

                    if (Val.ToString(DRow["STOCK_ID"]) == "")
                    {
                        DRow["STOCK_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                    }

                    DRow["TABLEINCNAME"] = Val.Trim(DRow["TABLEINCNAME"]);
                    DRow["SIDETABLENAME"] = Val.Trim(DRow["SIDETABLENAME"]);

                    //string[] StrPacketNo = Val.ToString(DRow["PARTYSTOCKNO"]).Split('-');
                    //DRow["PACKETNO"] = Val.Trim(new string(StrPacketNo[1].Where(c => c - '0' < 10).ToArray()));
                    //DRow["TAG"] = Val.Trim(StrPacketNo[1].Replace(Val.ToString(DRow["PACKETNO"]), ""));
                    //msg = DRow.Field<string>(0);

                }
                DTabGrdData.AcceptChanges();
                DTabGrdData.TableName = "DETAIL";

                string ParameterUpdateXml;
                using (StringWriter sw = new StringWriter())
                {
                    DTabGrdData.WriteXml(sw);
                    ParameterUpdateXml = sw.ToString();
                }

                Trn_SinglePrdProperty Property = new Trn_SinglePrdProperty();
                Property.MFGGradingNo = Val.ToInt64(txtGradingNo.Text);
                Property.XMLDETSTR = ParameterUpdateXml;

                //Property = ObjStock.MFGGradingSave(Property);
                //this.Cursor = Cursors.Default;
                //txtGradingNo.Text = Property.ReturnValue;
                //Global.Message(Property.ReturnMessageDesc);

                //if (Property.ReturnMessageType == "SUCCESS")
                //{
                //    DTabGrdData.AcceptChanges();
                //    calculate();
                //}

                //K: 14 / 11 / 2022 Add
                DataTable DTabSave = ObjStock.MFGGradingSaveForStock(Property);
                this.Cursor = Cursors.Default;
                if (DTabSave.Rows.Count == 0)
                {
                    Global.MessageError("FAIL");
                    this.Cursor = Cursors.Default;
                    return;
                }

                DTabGrdData = DTabSave;

                Global.Message(DTabGrdData.Rows[0]["RETURNMESSAGEDESC"].ToString());
                if (DTabGrdData.Rows[0]["RETURNTYPE"].ToString() == "SUCCESS")
                {
                    txtGradingNo.Text = DTabGrdData.Rows[0]["MFGGRADINGNO"].ToString();
                    DTabGrdData.AcceptChanges();
                    calculate();
                    MainGrd.DataSource = DTabGrdData;
                    GrdDet.RefreshData();
                }
                //End
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }
        private bool ValSave()
        {

            for (int IntI = 0; IntI < DTabGrdData.Rows.Count; IntI++)
            {
                DataRow DRow = DTabGrdData.Rows[IntI];
                //if (Val.ToString(DRow["PARTYSTOCKNO"]).Trim() == "" && !Val.ToString(DRow["STOCK_ID"]).Trim().Equals(string.Empty))
                //{
                //    Global.MessageError("Row : " + (IntI + 1).ToString() + " Stock NO Is Required");
                //    GrdDet.FocusedRowHandle = IntI;
                //    GrdDet.FocusedColumn = GrdDet.Columns["PARTYSTOCKNO"];
                //    GrdDet.Focus();
                //    return false;
                //}
                //if (Val.ToString(DRow["PARTYSTOCKNO"]).Trim() == "")
                //{
                //    if (DTabGrdData.Rows.Count == 1)
                //    {
                //        Global.MessageError("Row : " + (IntI + 1).ToString() + " Stock NO Is Required");
                //        GrdDet.FocusedRowHandle = IntI;
                //        GrdDet.FocusedColumn = GrdDet.Columns["PARTYSTOCKNO"];
                //        GrdDet.Focus();
                //        //break;
                //        return false;
                //    }
                //    else
                //        continue;
                //}
                if (Val.ToString(DRow["KAPANNAME"]).Trim() == "")
                {
                    if (DTabGrdData.Rows.Count == 1)
                    {
                        Global.MessageError("Row : " + (IntI + 1).ToString() + " Kapan Name Is Required");
                        GrdDet.FocusedRowHandle = IntI;
                        GrdDet.FocusedColumn = GrdDet.Columns["KAPANNAME"];
                        GrdDet.Focus();
                        return false;
                    }
                    else
                        continue;
                }
                if (Val.ToInt32(DRow["PACKETNO"]) == 0)
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Packet NO Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["PACKETNO"];
                    GrdDet.Focus();
                    return false;
                }
                //if (Val.ToString(DRow["MFG_ID"]).Trim() == "")
                //{
                //    Global.MessageError("Row : " + (IntI + 1).ToString() + " Mfg_ID Is Required");
                //    GrdDet.FocusedRowHandle = IntI;
                //    GrdDet.FocusedColumn = GrdDet.Columns["MFG_ID"];
                //    GrdDet.Focus();
                //    return false;
                //}
                if (Val.ToString(DRow["SHAPENAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Shape Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["SHAPENAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["COLORNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Color Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["COLORNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["MFGCOLORNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Mfg Color Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["MFGCOLORNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["CLARITYNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Clarity Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["CLARITYNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["MFGCLARITYNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Mfg Clarity Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["MFGCLARITYNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.Val(DRow["CARAT"]) == 0)
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Carat Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["CARAT"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["CUTNAME"]).Trim() == "" && Val.ToString(DRow["SHAPENAME"]) == "R") //Round Hoy to j Cut Data Compulsory karva Dey..
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Cut Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["CUTNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["POLNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Pol Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["POLNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["SYMNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Sym Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["SYMNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["FLNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Fl Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["FLNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["LABNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Lab Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["LABNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["MILKYNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Milky Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["MILKYNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["EYECLEANNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " EyeClean Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["EYECLEANNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["LUSTERNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Luster Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["LUSTERNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["HANAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Ha Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["HANAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["TABLEINCNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Table Inc Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["TABLEINCNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["SIDETABLENAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Side Table Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["SIDETABLENAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["SIDEOPENNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Side Open Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["SIDEOPENNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["TABLEBLACKNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Table Black Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["TABLEBLACKNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["SIDEBLACKNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Side Black Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["SIDEBLACKNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["REDSPORTNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Red Sport Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["REDSPORTNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.ToString(DRow["GIRDLEINCNAME"]).Trim() == "")
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Girlde Inclusion Name Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["GIRDLEINCNAME"];
                    GrdDet.Focus();
                    return false;
                }
                if (Val.Val(DRow["DIAMIN"]) == 0)
                {
                    Global.MessageError("Row : " + (IntI + 1).ToString() + " Dia Min Is Required");
                    GrdDet.FocusedRowHandle = IntI;
                    GrdDet.FocusedColumn = GrdDet.Columns["DIAMINNAME"];
                    GrdDet.Focus();
                    return false;
                }
            }
            return true;

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            DTabGrdData.Rows.Clear();
            txtGradingNo.Text = "";
            DTPGradingDate.Value = DateTime.Now;
            DtabSum.Rows.Clear();
            MainGrdSum.DataSource = null; //hinal 02-01-2022
            MainGrdSum.Refresh(); //hinal 02-01-2022
        }

        private void GrdSummary_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                txtGradingNo.Text = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MFGGRADINGNO"));
                DTPGradingDate.Text = Val.ToDate(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "ENTRYDATE")), AxonDataLib.BOConversion.DateFormat.DDMMYYYY);
                BtnContinue_Click(null, null);
                calculate();
            }

        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataTable DTabSummary = ObjStock.MFGGradingGetSummary(Val.SqlDate(DTPFromDate.Value.ToShortDateString()), Val.SqlDate(DTPToDate.Value.ToShortDateString()), txtStoneNo.Text);
                MainGridSummary.DataSource = DTabSummary;
                MainGridSummary.Refresh();
                GrdSummary.BestFitMaxRowCount = 500;
                GrdSummary.BestFitColumns();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void retTextRemark_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (GrdDet.IsLastRow && Val.ToString(GrdDet.GetFocusedRowCellValue("LABNAME")) != "")
            //    {
            //        BtnAddNewRow_Click(null, null);
            //    }
            //    else
            //    {
            //        BtnSave.Focus();
            //    }
            //}
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            if (GrdDet.RowCount != 0)
            {
                Global.ExcelExport(txtGradingNo.Text + "_Grading.xlsx", GrdDet);
            }
        }

        private void GrdDet_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }

            int IntISBombayTransfer = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, "ISBOMBAYTRANSFER"));
            if (IntISBombayTransfer == 0)
            {
                e.Appearance.BackColor = lblPending.BackColor;
                e.Appearance.BackColor2 = lblPending.BackColor;
            }
            else if (IntISBombayTransfer == 1)
            {
                e.Appearance.BackColor = lblTransfered.BackColor;
                e.Appearance.BackColor2 = lblTransfered.BackColor;
            }
        }

        private void repTxtKapan_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "KAPANNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.TRN_KAPAN);
                    //FrmSearch.ColumnsToHide = "DEPARTMENT_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.SetFocusedRowCellValue("KAPANNAME", Val.ToString(FrmSearch.DRow["KAPANNAME"]));
                        GrdDet.SetFocusedRowCellValue("LOT_ID", Val.ToString(FrmSearch.DRow["KAPAN_ID"]));
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void btnExcelWithFormat_Click(object sender, EventArgs e)
        {
            string StrFileName = ExportExcelNew();
            if (StrFileName == "")
            {
                Global.Message("Please Select Atleast One Packet");
                return;
            }
            if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(StrFileName, "CMD");
            }
        }

        public string ExportExcelNew()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Trn_SinglePrdProperty Property = new Trn_SinglePrdProperty();
                Property.MFGGradingNo = Val.ToInt64(txtGradingNo.Text);
                DataSet DS = ObjStock.MFGGradingGetDetail_Export(Property);

                this.Cursor = Cursors.Default;


                DataTable DTabDetail = DS.Tables[0];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                DTabDetail.DefaultView.Sort = "LOTNAME";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);

                Color BackColor = Color.White;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet worksheetMemo = xlPackage.Workbook.Worksheets.Add("MEMO");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Stock Detail

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabDetail, true);
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = FontSize;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    worksheet.Cells[1, 1, 1, 33].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    //worksheet.Cells[1, 14, 1, 16].Style.Font.Color.SetColor(Color.Red);
                    //worksheet.Cells[1, 24, 1, 24].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    //worksheet.Cells[1, 25, 1, 25].Style.Fill.BackgroundColor.SetColor(Color.Red);
                    //worksheet.Cells[1, 28, 1, 28].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    //worksheet.Cells[1, 29, 1, 32].Style.Font.Color.SetColor(Color.Red);
                    //worksheet.Cells[1, 33, 1, 33].Style.Fill.BackgroundColor.SetColor(Color.Red);

                    // Header Set
                    for (int i = 1; i <= DTabDetail.Columns.Count; i++)
                    {
                        string StrHeader = Global.ExportExcelHeaderMfg(Val.ToString(worksheet.Cells[StartRow, i].Value), worksheet, i);
                        worksheet.Cells[StartRow, i].Value = StrHeader;
                    }

                    #endregion

                    xlPackage.Save();
                }

                this.Cursor = Cursors.Default;
                return StrFilePath;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
            return "";
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                Stream str = new System.IO.MemoryStream();
                GrdDet.SaveLayoutToStream(str);
                str.Seek(0, System.IO.SeekOrigin.Begin);
                StreamReader reader = new StreamReader(str);
                string text = reader.ReadToEnd();

                int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDet.Name, text);
                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Saved");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            try
            {
                int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDet.Name);
                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Deleted");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void fill()
        {
            //calculate();
            //DtabSum = MainGrd.DataSource;

        }
        private void GrdDet_Click(object sender, EventArgs e)
        {

            foreach (DataRow dr in DtabSum.Rows)
            {
                string kapannme = Val.ToString(dr["KAPANNAME"]);
                int pcs = Val.ToInt(dr["PCS"]);
                decimal carat = Val.ToDecimal(dr["CARAT"]);
            }
        }
        public void calculate() //hinal 30-12-21
        {
            try
            {
                DtabSum = new DataTable();
                DtabSum.Columns.Add("KAPANNAME", typeof(string));
                DtabSum.Columns.Add("PCS", typeof(Int32));
                DtabSum.Columns.Add("CARAT", typeof(double));

                string StrKapanname = "";
                int IntPcs = 0;
                double DcsCarat = 0;


                for (int i = 0; i < GrdDet.RowCount; i++)
                {

                    DataRow DR = GrdDet.GetDataRow(i);

                    StrKapanname = Val.ToString(DR["KAPANNAME"]);
                    DcsCarat = Val.Val(DR["CARAT"]);

                    if (StrKapanname == "")
                    {
                        continue;
                    }

                    DataRow DRS = DtabSum.NewRow();
                    DRS["KAPANNAME"] = StrKapanname;
                    DRS["PCS"] = IntPcs;
                    DRS["CARAT"] = DcsCarat;

                    DtabSum.Rows.Add(DRS);
                }
                if (DtabSum.Rows.Count <= 0)
                {
                    MainGrdSum.DataSource = DtabSum;
                    return;
                }
                var newDt = DtabSum.AsEnumerable()
                  .GroupBy(r => r.Field<string>("KAPANNAME"))
                  .Select(g =>
                  {
                      var row = DtabSum.NewRow();
                      row["KAPANNAME"] = g.Key;
                      row["PCS"] = g.Count();
                      row["CARAT"] = g.Sum(r => r.Field<double>("CARAT"));
                      return row;
                  }).CopyToDataTable();



                MainGrdSum.DataSource = newDt;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_CellValueChanged_1(object sender, CellValueChangedEventArgs e)
        {
            calculate();
        }

        public bool CheckDuplicate(string ColName, string ColValue, int IntRowIndex)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DTabGrdData.AsEnumerable()
                         where Val.ToString(row[ColName]).ToUpper() == Val.ToString(ColValue).ToUpper() && row.Table.Rows.IndexOf(row) != IntRowIndex
                         select row;

            if (Result.Any())
            {
                Global.Message("[" + ColValue + "] : StockNo Is Already Exists..");
                return true;
            }
            return false;
        }

        private void repTxtStockNo_Validating(object sender, CancelEventArgs e) //hinal : 13-01-2022
        {
            {
                if (GrdDet.FocusedRowHandle < 0)
                    return;

                if (CheckDuplicate("PARTYSTOCKNO", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle))
                    e.Cancel = true;
                return;

            }
        }

        private void GrdDet_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = (GridView)sender;
            int IntISBombayTransfer = Val.ToInt(view.GetFocusedRowCellValue("ISBOMBAYTRANSFER"));

            if (IntISBombayTransfer == 1)
                e.Cancel = true;
            else
                e.Cancel = false;
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

            //if (IsSelect)
            //{
            //    aryLst = ObjGridSelection.GetSelectedArrayList();
            //    resultTable = sourceTable.Clone();
            //    for (int i = 0; i < aryLst.Count; i++)
            //    {
            //        DataRowView oDataRowView = aryLst[i] as DataRowView;
            //        resultTable.Rows.Add(oDataRowView.Row.ItemArray);
            //    }
            //}

            return sourceTable;
        }

        private void BtnBarcodePrint_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = GetTableOfSelectedRows(GrdDet, true);

                if (DTab == null)
                {
                    Global.Message("Please Select at lease One Record For Barcode Print");
                    return;
                }

                if (DTab.Rows.Count == 0)
                {
                    Global.Message("Please Select at lease One Row For Barcode Print");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Print Barcode of All Selected Stones?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                foreach (DataRow DRow in DTab.Rows)
                {
                    if (Val.ToString(DRow["STOCKNO"]) == "")
                    {
                        continue;
                    }
                    Global.MFGGradingBarcodePrint(DRow);
                }
                Global.Message("Print Successfully");
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void MainGrd_Click(object sender, EventArgs e)
        {

        }

        private void deleteSelectedAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    int IntISBombayTransfer = Val.ToInt(GrdDet.GetRowCellValue(GrdDet.FocusedRowHandle, "ISBOMBAYTRANSFER"));
                    if (IntISBombayTransfer == 1)
                    {
                        Global.Message("This Entry Not Deleted Beacuse Entry Transfer.");
                    }
                    else
                    {
                        if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY") == System.Windows.Forms.DialogResult.Yes)
                        {
                            FrmPassword FrmPassword = new FrmPassword();
                            if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                            {
                                Trn_SinglePrdProperty Property = new Trn_SinglePrdProperty();
                                DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                                Property.STOCK_ID = Val.ToString(Drow["STOCK_ID"]);
                                if (Property.STOCK_ID == "")
                                    return;
                                Property = ObjStock.Delete(Property);
                                if (Property.ReturnMessageType == "SUCCESS")
                                {
                                    Global.Message("ENTRY DELETED SUCCESSFULLY");
                                    DTabGrdData.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                                    DTabGrdData.AcceptChanges();
                                    MainGrd.DataSource = DTabGrdData;
                                    GrdDet.RefreshData();
                                    GrdDet.BestFitMaxRowCount = 500;
                                    GrdDet.BestFitColumns();
                                }
                                else
                                {
                                    Global.Message("ERROR IN DELETE ENTRY");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void repTextPopup_KeyDown(object sender, KeyEventArgs e)
        {
            //if (GrdDet.FocusedColumn.FieldName.ToUpper() == "LABNAME")
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        if (GrdDet.IsLastRow && Val.ToString(GrdDet.GetFocusedRowCellValue("LABNAME")) != "")
            //        {
            //            BtnAddNewRow_Click(null, null);
            //        }
            //        else
            //        {
            //            BtnSave.Focus();
            //        }
            //    }
            //}
        }
    }
}
