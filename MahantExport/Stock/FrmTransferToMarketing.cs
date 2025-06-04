using BusLib.Master;
using BusLib.Rapaport;
using BusLib.TableName;
using BusLib.Transaction;
using MahantExport.Utility;
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

namespace MahantExport.Stock
{
    public partial class FrmTransferToMarketing : Form
    {
        BODevGridSelection ObjGridSelection;
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();

        BOFindRap ObjRap = new BOFindRap();
        DataTable DtabPara = new DataTable();
        DataTable  DTabSearch = new DataTable();

        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();

        public FrmTransferToMarketing()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();
            if (MainGridParameter.RepositoryItems.Count == 10)
            {
                ObjGridSelection = new BODevGridSelection();
                ObjGridSelection.View = GrdDetParameter;
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }
            else
            {
                ObjGridSelection.ClearSelection();
            }
            GrdDetParameter.Columns["COLSELECTCHECKBOX"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }
            DtabPara = new BOMST_Parameter().GetParameterData();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string StrStatus = "";
                if(RbtAll.Checked == true)
                {
                    StrStatus = "ALL";
                }
                else if (RbtPending.Checked == true)
                {
                    StrStatus = "PENDING";
                }
                else if (RbtTransferToMarketing.Checked == true)
                {
                    StrStatus = "TRANSFER TO MARKETING";
                }
                DTabSearch = ObjStock.GetTranferToMarketingData(StrStatus,Val.ToString(txtStoneNo.Text));
                MainGridParameter.DataSource = DTabSearch;
                MainGridParameter.Refresh();
                GrdDetParameter.BestFitColumns();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        private void txtStoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            
            
            try
            {
                string StrStock_ID = "";
                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDetParameter, true, ObjGridSelection);

                if (DTab.Rows.Count == 0 || DTab == null)
                {
                    Global.Message("Please Select At Least One Record For Transfer To Marketing.. ");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Transfer To Marketing of All Selected Packets?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                if (DTab.Rows.Count > 0)
                {
                    var list = DTab.AsEnumerable().Select(r => r["STOCK_ID"].ToString());
                    StrStock_ID = string.Join(",", list);
                }
                else
                {
                    StrStock_ID = Val.ToString(GrdDetParameter.GetFocusedRowCellValue("STOCK_ID"));
                }
                SingleStockUpdateProperty Property = new SingleStockUpdateProperty();

                Property = ObjStock.UpdateSingleStockStatus(Property, StrStock_ID);

                Global.Message(Property.ReturnMessageDesc);
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    BtnSearch_Click(null, null);
                }
                DTab.Rows.Clear();
                DTab = null;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void GrdDetParameter_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "SALEDISCOUNT")
            {
                //DataRow DRow = GrdDetParameter.GetDataRow(e.RowHandle);

                //Trn_RapSaveProperty Property = new Trn_RapSaveProperty();

                //Property.SHAPE_ID = Val.ToString(DRow["SHAPE"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='SHAPE' And ShortName='" + Val.ToString(DRow["SHAPE"]) + "'")[0]["PARA_ID"]) : 0;
                //Property.COLOR_ID = Val.ToString(DRow["COLOR"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='COLOR' And ShortName='" + Val.ToString(DRow["COLOR"]) + "'")[0]["PARA_ID"]) : 0;
                //Property.CLARITY_ID = Val.ToString(DRow["CLARITY"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='CLARITY' And ShortName='" + Val.ToString(DRow["CLARITY"]) + "'")[0]["PARA_ID"]) : 0;

                //Property.CUT_ID = Val.ToString(DRow["CUT"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='CUT' And ShortName='" + Val.ToString(DRow["CUT"]) + "'")[0]["PARA_ID"]) : 0;
                //Property.POL_ID = Val.ToString(DRow["POLISH"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='POLISH' And ShortName='" + Val.ToString(DRow["POLISH"]) + "'")[0]["PARA_ID"]) : 0;
                //Property.SYM_ID = Val.ToString(DRow["SYMM"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='SYMMETRY' And ShortName='" + Val.ToString(DRow["SYMM"]) + "'")[0]["PARA_ID"]) : 0;
                //Property.FL_ID = Val.ToString(DRow["FLOUR"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='FLUORESCENCE' And ShortName='" + Val.ToString(DRow["FLOUR"]) + "'")[0]["PARA_ID"]) : 0;
                //Property.CARAT = Val.Val(DRow["CARAT"]);

                //Property = ObjRap.FindRap(Property);

                //DTabSearch.Rows[e.RowHandle]["SALERAPAPORT"] = Property.RAPAPORT;
                //DTabSearch.Rows[e.RowHandle]["SALEPRICEPERCTS"] = Math.Round(Property.RAPAPORT - ((Property.RAPAPORT * Val.Val(DRow["SALEDISCOUNT"])) / 100)); //#P:23-04-2021
                //DTabSearch.Rows[e.RowHandle]["SALEAMOUNT"] = Math.Round(Property.CARAT * Val.Val(DTabSearch.Rows[e.RowHandle]["SALEPRICEPERCTS"]), 2);

                //DTabSearch.AcceptChanges();
            }
        }

        private void BtnUpdateSaleDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = DTabSearch.GetChanges();
                if (DTab == null || DTab.Rows.Count <= 0)
                {
                    if (Global.Confirm("No Any Changes , Still You Want Save This All Record ?") == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    DTab = DTabSearch.Copy();
                }

                this.Cursor = Cursors.WaitCursor;

                DTab.TableName = "DETAIL";

                string ParameterUpdateXml;
                using (StringWriter sw = new StringWriter())
                {
                    DTab.WriteXml(sw);
                    ParameterUpdateXml = sw.ToString();
                }

                MemoEntryProperty Property = new MemoEntryProperty();
                Property = new BOTRN_StockUpload().SaveParameterOrPriceUpdateDetail(ParameterUpdateXml, false);
                this.Cursor = Cursors.Default;
                Global.Message(Property.ReturnMessageDesc);
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DTabSearch.AcceptChanges();
                }
                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void RbtAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                BtnSearch_Click(null, null);
            }
            catch (Exception Ex)
            {

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
                //rTxtStoneCertiMfgMemo.Text = str1.Trim().TrimStart().TrimEnd();
                txtStoneNo.Select(txtStoneNo.Text.Length, 0);
                //rTxtStoneCertiMfgMemo.Text = rTxtStoneCertiMfgMemo.Text.Trim().TrimStart().TrimEnd();

                //lblTotalCount.Text = "(" + rTxtStoneCertiMfgMemo.Text.Split(',').Length + ")";


            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtStoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearch_Click(null, null);
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

        private void BtnOffline_Click(object sender, EventArgs e)
        {
            try
            {
                string StrStock_ID = "";
                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDetParameter, true, ObjGridSelection);

                if (DTab.Rows.Count == 0 || DTab == null)
                {
                    Global.Message("Please Select At Least One Record For Transfer To Marketing.. ");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Transfer To Marketing of All Selected Packets?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                if (DTab.Rows.Count > 0)
                {
                    var list = DTab.AsEnumerable().Select(r => r["STOCK_ID"].ToString());
                    StrStock_ID = string.Join(",", list);
                }
                else
                {
                    StrStock_ID = Val.ToString(GrdDetParameter.GetFocusedRowCellValue("STOCK_ID"));
                }
                SingleStockUpdateProperty Property = new SingleStockUpdateProperty();

                Property = ObjStock.UpdateSingleStockStatusOffline(Property, StrStock_ID);

                Global.Message(Property.ReturnMessageDesc);
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    BtnSearch_Click(null, null);
                }
                DTab.Rows.Clear();
                DTab = null;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
    }
}
