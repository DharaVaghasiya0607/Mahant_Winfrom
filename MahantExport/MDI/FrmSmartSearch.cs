using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MahantExport.Stock;
using BusLib.TableName;
using System.Collections;

namespace MahantExport.MDI
{
    public partial class FrmSmartSearch : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public FrmSmartSearch()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();


            DataTable DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SMARTSEARCH);

            txtTokenSearch.Properties.DataSource = DTab;
            txtTokenSearch.Properties.DisplayMember = "PARANAME";
            txtTokenSearch.Properties.ValueMember = "PARA_ID";

            this.Show();
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
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            //Global.Message(txtTokenSearch.EditValue.ToString());
            //Global.Message(txtTokenSearch.EditText);

            LiveStockProperty mLStockProperty = new LiveStockProperty();
            mLStockProperty.MULTYSHAPE_ID = string.Empty;
            mLStockProperty.MULTYCOLOR_ID = string.Empty;
            mLStockProperty.MULTYCLARITY_ID = string.Empty;
            mLStockProperty.MULTYCUT_ID = string.Empty;
            mLStockProperty.MULTYPOL_ID = string.Empty;
            mLStockProperty.MULTYSYM_ID = string.Empty;
            mLStockProperty.MULTYFL_ID = string.Empty;
            mLStockProperty.FROMCARAT1 = 0;
            mLStockProperty.TOCARAT1 = 0;
            mLStockProperty.FROMCARAT2 = 0;
            mLStockProperty.TOCARAT2 = 0;
            mLStockProperty.FROMCARAT3 = 0;
            mLStockProperty.TOCARAT3 = 0;
            mLStockProperty.FROMCARAT4 = 0;
            mLStockProperty.TOCARAT4 = 0;
            mLStockProperty.FROMCARAT5 = 0;
            mLStockProperty.TOCARAT5 = 0;
            mLStockProperty.WEBSTATUS = "ASSISS,ASSRET,AVAILABLE,CONSIGNMENT,FACTGRD,HOLD,LAB,LAB-RESULT,LAB-RETURN,MEMO,PURCHASE,NONE,RPURCHASE,SOLD,SURATREP";


            string[] Str = txtTokenSearch.EditValue.ToString().Split(',');

            ArrayList AL = new ArrayList();

            for (int i = 0; i < Str.Length; i++)
            {
                string[] StrContent = Str[i].Split('/');
                if (StrContent.Length == 0)
                {
                    continue;
                }

                StrContent[0] = Val.Trim(StrContent[0]);
                StrContent[1] = Val.Trim(StrContent[1]);

                if (StrContent[0] == "SHAPE")
                {
                    mLStockProperty.MULTYSHAPE_ID = mLStockProperty.MULTYSHAPE_ID + StrContent[1] + ",";
                }
                else if (StrContent[0] == "COLOR")
                {
                    mLStockProperty.MULTYCOLOR_ID = mLStockProperty.MULTYCOLOR_ID + StrContent[1] + ",";
                }
                else if (StrContent[0] == "CLARITY")
                {
                    mLStockProperty.MULTYCLARITY_ID = mLStockProperty.MULTYCLARITY_ID + StrContent[1] + ",";
                }
                else if (StrContent[0] == "CUT")
                {
                    mLStockProperty.MULTYCUT_ID = mLStockProperty.MULTYCUT_ID + StrContent[1] + ",";
                }
                else if (StrContent[0] == "POLISH")
                {
                    mLStockProperty.MULTYPOL_ID = mLStockProperty.MULTYPOL_ID + StrContent[1] + ",";
                }
                else if (StrContent[0] == "SYMMETRY")
                {
                    mLStockProperty.MULTYSYM_ID = mLStockProperty.MULTYSYM_ID + StrContent[1] + ",";
                }
                else if (StrContent[0] == "FLUORESCENCE")
                {
                    mLStockProperty.MULTYFL_ID = mLStockProperty.MULTYFL_ID + StrContent[1] + ",";
                }
                else if (StrContent[0] == "MILKY")
                {
                    mLStockProperty.MULTYMILKY_ID = mLStockProperty.MULTYMILKY_ID + StrContent[1] + ",";
                }
                else if (StrContent[0] == "FANCYCOLOR")
                {
                    mLStockProperty.MULTYFANCYCOLOR_ID = mLStockProperty.MULTYFANCYCOLOR_ID + StrContent[1] + ",";
                }
                else if (StrContent[0] == "LOCATION")
                {
                    mLStockProperty.MULTYLOCATION_ID = mLStockProperty.MULTYLOCATION_ID + StrContent[1] + ",";
                }
                else if (StrContent[0] == "LAB")
                {
                    mLStockProperty.MULTYLAB_ID = mLStockProperty.MULTYLAB_ID + StrContent[1] + ",";
                }
                
                else if (StrContent[0] == "STONENO")
                {
                    mLStockProperty.STOCKNO = mLStockProperty.STOCKNO + StrContent[1] + ",";
                }
                else if (StrContent[0] == "CERT")
                {
                    mLStockProperty.LABREPORTNO = mLStockProperty.LABREPORTNO + StrContent[1] + ",";
                }
                else if (StrContent[0] == "SIZE")
                {
                    AL.Add(StrContent[1]);
                }
                
            }
            int IntSize = 1;
            for (int IntI = 0; IntI < AL.Count; IntI++)
            {
                string[] StrSize = Val.Trim(AL[IntI]).ToString().Split('-');

                if (IntSize == 1)
                {
                    mLStockProperty.FROMCARAT1 = Val.Val(StrSize[0]);
                    mLStockProperty.TOCARAT1 = Val.Val(StrSize[1]);
                }
                else if (IntSize == 2)
                {
                    mLStockProperty.FROMCARAT2 = Val.Val(StrSize[0]);
                    mLStockProperty.TOCARAT2 = Val.Val(StrSize[1]);
                }
                else if (IntSize == 3)
                {
                    mLStockProperty.FROMCARAT3 = Val.Val(StrSize[0]);
                    mLStockProperty.TOCARAT3 = Val.Val(StrSize[1]);
                }
                else if (IntSize == 4)
                {
                    mLStockProperty.FROMCARAT4 = Val.Val(StrSize[0]);
                    mLStockProperty.TOCARAT4 = Val.Val(StrSize[1]);
                }
                else if (IntSize == 5)
                {
                    mLStockProperty.FROMCARAT5 = Val.Val(StrSize[0]);
                    mLStockProperty.TOCARAT5 = Val.Val(StrSize[1]);
                }
                IntSize++;
            }
           
            mLStockProperty.MULTYBOX_ID ="";            
            mLStockProperty.MULTYKAPAN = "";

            mLStockProperty.FROMLENGTH = 0;
            mLStockProperty.TOLENGTH = 0;

            mLStockProperty.FROMWIDTH = 0;
            mLStockProperty.TOWIDTH = 0;

            mLStockProperty.FROMHEIGHT = 0;
            mLStockProperty.TOHEIGHT = 0;

            mLStockProperty.FROMTABLEPER = 0;
            mLStockProperty.TOTABLEPER = 0;

            mLStockProperty.FROMDEPTHPER = 0;
            mLStockProperty.TODEPTHPER = 0;

            mLStockProperty.SERIALNO = string.Empty;
            mLStockProperty.MEMONO = string.Empty;
        }

        private void BtnNewArrivals_Click(object sender, EventArgs e)
        {

        }

        private void BtnExclusiveStone_Click(object sender, EventArgs e)
        {

        }

    }
}