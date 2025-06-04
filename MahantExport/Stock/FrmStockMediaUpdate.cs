using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MahantExport.Stock
{
    public partial class FrmStockMediaUpdate : DevControlLib.cDevXtraForm
    {
        public delegate void SetControlValueCallback(Control oControl, string propName, object propValue);

        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMast = new BOTRN_MemoEntry();
        DataTable DTag = new DataTable();

        #region Property Settings

        public FrmStockMediaUpdate()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;
            
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }
            this.Show();
            SetControlPropertyValue(lblMessage, "Text", "");
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        #endregion

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int IntI = 0;

                this.Cursor = Cursors.WaitCursor;

                foreach (DataRow Dr in DTag.GetChanges().Rows)
                {

                    IntI += ObjMast.StoneMediaSave(
                        Val.ToString(Dr["STOCK_ID"]),
                        Val.ToBoolean(Dr["ISIMAGE"]),
                        Val.ToString(Dr["IMAGEURL"]),
                        Val.ToBoolean(Dr["ISCERTI"]),
                        Val.ToString(Dr["CERTURL"]),
                        Val.ToBoolean(Dr["ISVIDEO"]),
                        Val.ToString(Dr["VIDEOURL"])
                        );
                }
                DTag.AcceptChanges();

                this.Cursor = Cursors.Default;

                if (IntI != 0)
                {
                    Global.Message( IntI +" Stone Media Updated Successfully");
                    BtnShow_Click(null, null);
                }
                else
                {
                    Global.Message("No Record Found For Update");
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("StoneMediaList", GrdDet);
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            GrdDet.BeginUpdate();
            DTag = ObjMast.StoneMediaGetData(txtStockNo.Text);
            MainGrid.DataSource = DTag;
            MainGrid.Refresh();
            GrdDet.BestFitColumns();
            GrdDet.EndUpdate();

            this.Cursor = Cursors.Default;
        }

        private void txtStockNo_TextChanged(object sender, EventArgs e)
        {
            if (txtStockNo.Text.Length > 0 && Convert.ToString(PasteData) != "")
            {
                txtStockNo.SelectAll();
                String str1 = PasteData.Replace("\r\n", ",");                   //data.Replace(\n, ",");
                str1 = str1.Trim();
                str1 = str1.TrimEnd();
                str1 = str1.TrimStart();
                str1 = str1.TrimEnd(',');
                str1 = str1.TrimStart(',');
                txtStockNo.Text = str1;
                PasteData = "";
            }

        }

        private void txtStockNo_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtStockNo.Focus())
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    PasteData = Convert.ToString(PasteclipData.GetData(System.Windows.Forms.DataFormats.Text));
                }
            }
        }

        private void txtStockNo_KeyUp(object sender, KeyEventArgs e)
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
                txtStockNo.Text = str1;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            txtStockNo.Text = string.Empty;
            DTag.Rows.Clear();
        }

        private void BtnCheckImageCerti_Click(object sender, EventArgs e)
        {
            progressPanel1.Visible = true;
            SetControlPropertyValue(lblMessage, "Text", "");
            backgroundWorker1.RunWorkerAsync();
        }

        public bool DoesImageExistRemotely(string uriToImage)
        {
            if (uriToImage == "")
            {
                return false;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriToImage);

            request.Method = "HEAD";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (WebException) { return false; }
            catch
            {
                return false;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            DataTable DTab = ObjMast.MissingMediaGetData();
            int IntTotal = DTab.Rows.Count;
            int IntI = 0;
            foreach (DataRow DRow in DTab.Rows)
            {
                IntI++;

                SetControlPropertyValue(lblMessage, "Text", "("+IntI+"/"+IntTotal+") ");
        
                string StrStockID = Val.ToString(DRow["STOCK_ID"]);
                string StrStockNO = Val.ToString(DRow["STOCKNO"]);

                string StrImageURL = Val.ToString(DRow["IMAGEURL"]);
                string StrCertiURL = Val.ToString(DRow["CERTURL"]);
                string StrVideoURL = Val.ToString(DRow["VIDEOURL"]);

                bool ISImage = Val.ToBoolean(DRow["ISIMAGE"]);
                bool ISCerti = Val.ToBoolean(DRow["ISCERTI"]);
                bool ISVideo = Val.ToBoolean(DRow["ISVIDEO"]);

                if (ISImage == false)
                {
                    SetControlPropertyValue(lblMessage, "Text", "(" + IntI + "/" + IntTotal + ") " + StrStockNO + " : Image");
                    ISImage = DoesImageExistRemotely(StrImageURL);
                }

                if (ISCerti == false)
                {
                    SetControlPropertyValue(lblMessage, "Text", "(" + IntI + "/" + IntTotal + ") " + StrStockNO + " : Certi");
                    ISCerti = DoesImageExistRemotely(StrCertiURL);
                }

                if (ISVideo == false)
                {
                    SetControlPropertyValue(lblMessage, "Text", "(" + IntI + "/" + IntTotal + ") " + StrStockNO + " : Video");
                    ISVideo = DoesImageExistRemotely(StrVideoURL);
                }

                DRow["ISIMAGE"] = ISImage;
                DRow["ISCERTI"] = ISCerti;
                DRow["ISVIDEO"] = ISVideo;
            }
            
            DTab.TableName = "DETAIL";
            DTab.AcceptChanges();

            string StockUploadXML;
            using (StringWriter sw = new StringWriter())
            {
                DTab.WriteXml(sw);
                StockUploadXML = sw.ToString();
            }
            ObjMast.MissingMediaSave(StockUploadXML);

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetControlPropertyValue(lblMessage, "Text", "");
            progressPanel1.Visible = false;
            Global.Message("All Media Flags Are Updated Successfully");
        }

        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] 
                        {
                            oControl,
                            propName,
                            propValue
                        });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if ((p.Name.ToUpper() == propName.ToUpper()))
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }


    }
}
