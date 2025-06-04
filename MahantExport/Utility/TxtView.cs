using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BusLib.Configuration;
using AxonDataLib;
//using AxonDataLib;
namespace MahantExport.Utility
{
    public partial class frmTxtView  : DevControlLib.cDevXtraForm
    {

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        AxonDataLib.BOPrinting Prn = new AxonDataLib.BOPrinting();

        private string mStrDestFileName;
        private string mStrSourFileName;

        private string mStrPrintDosWin;
        private System.Drawing.Printing.PrintDocument mPrnDoc;
        private System.IO.FileStream FSReadFile;
        private System.IO.StreamReader Reader;
        public frmTxtView()
        {
            InitializeComponent();
        }

        public void ShowForm(string pStrFileName)
        {
            Val.FormGeneralSetting(this);
            radWin.Checked = true; 
            mStrSourFileName = pStrFileName;
            mStrDestFileName = Prn.SetFileForView(mStrSourFileName);
            Rtb.LoadFile(mStrDestFileName, RichTextBoxStreamType.PlainText);
            AttachFormDefaultEvent();
            this.ShowDialog();
            btnClose.DialogResult = DialogResult.Cancel;
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(Prn);
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
        }
        private void mPrnDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int IntRow = 1; int IntLinesPerPage = 0; int IntCurRow; int IntCurCol; int IntFSize = 8; 
            bool BlnFontBold = false; bool BlnFontUnderline = false;
            char ChrSingle = ' ';
            float FHeight;
            System.Drawing.Font FontCur = new System.Drawing.Font("Courier New", 8);

            IntCurRow = 1;
            IntCurCol = 1;
            IntRow = 1;
            FHeight = FontCur.GetHeight(e.Graphics);
            IntLinesPerPage = Convert.ToInt32(e.MarginBounds.Height / FontCur.GetHeight(e.Graphics));
            ChrSingle = Convert.ToChar(Reader.Read());
            try
            {
                while (IntRow < IntLinesPerPage)
                {
                    if (ChrSingle == Convert.ToChar(27))
                    {
                        ChrSingle = Convert.ToChar(Reader.Read());
                        switch (ChrSingle)
                        {
                            case 'G':
                            case 'E':
                                BlnFontBold = true;
                                break;
                            case 'H':
                            case 'F':
                                BlnFontBold = false;
                                break;
                            case 'C':
                                IntFSize = 10;
                                break;
                            case 'W':
                                ChrSingle = Convert.ToChar(Reader.Read());
                                if (ChrSingle == '1')
                                {
                                    IntFSize = 16;
                                }
                                else if (ChrSingle == '2')
                                {
                                    IntFSize = 12;
                                }
                                break;
                            case 'U':
                                ChrSingle = Convert.ToChar(Reader.Read());
                                if (ChrSingle == '1')
                                {
                                    BlnFontUnderline = true;
                                }
                                else if (ChrSingle == '0')
                                {
                                    BlnFontUnderline = false;
                                }
                                break;
                            case 'M':
                                IntFSize = 9;
                                break;
                            case 'N':
                            case 'O':
                                ChrSingle = Convert.ToChar(Reader.Read());                                 
                                break;
                        }
                    }
                    else if (ChrSingle == Convert.ToChar(15))
                    {
                        IntFSize = 8;
                    }
                    else if (ChrSingle == Convert.ToChar(18))
                    {
                    }
                    else if (ChrSingle == Convert.ToChar(12))
                    {
                    }
                    else if ((Microsoft.VisualBasic.Strings.Asc(ChrSingle) >= 32) || (Microsoft.VisualBasic.Strings.Asc(ChrSingle) == 13))
                    {
                        if (BlnFontBold == true)
                        {
                            FontCur = new System.Drawing.Font("Courier New", IntFSize, FontStyle.Bold);
                        }
                        else if (BlnFontUnderline == true)
                        {
                            FontCur = new System.Drawing.Font("Courier New", IntFSize, FontStyle.Underline);
                        }
                        else
                        {
                            FontCur = new System.Drawing.Font("Courier New", IntFSize);
                        }
                        e.Graphics.DrawString(Convert.ToString(ChrSingle), FontCur, Brushes.Black, (IntCurCol * 6), (IntRow * FHeight), new StringFormat());
                        IntCurCol = (IntCurCol + 1);
                        if (Microsoft.VisualBasic.Strings.Asc(ChrSingle) == 13)
                        {
                            IntCurRow = IntCurRow + 1;
                            IntRow = IntRow + 1;
                            IntCurCol = 1;
                        }
                    }
                    ChrSingle = Convert.ToChar(Reader.Read());
                }
                if (IntRow >= IntLinesPerPage)
                {
                    e.HasMorePages = true;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }
            catch  
            {
                e.HasMorePages = false;
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            mPrnDoc = null;
            this.Close();
        }

        private void frmTxtView_Resize(object sender, System.EventArgs e)
        {
            Val.FormResize(this);
        }

        private void frmTxtView_Disposed(object sender, System.EventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(mStrDestFileName) == true)
                {
                    System.IO.File.Delete(mStrDestFileName);
                }
            }
            catch  
            {
                Application.DoEvents();
            }
            mPrnDoc = null;
        }

        private void btnPrint_Click(object sender, System.EventArgs e)
        {          
            if (mStrPrintDosWin == "D")
            {
                Prn.PrintTextFileDos(mStrSourFileName);              
            }
            else
            {
                Prn.PrintTextFileWin(mStrSourFileName);                 
            }           
        }

        private void radDosWin_CheckedChanged(object sender, System.EventArgs e)
        {
            for (int IntI = 0; IntI < grpDosWin.Controls.Count; IntI++)
            {
                grpDosWin.Controls[IntI].ForeColor = System.Drawing.Color.DarkBlue;
            }
            ((AxonContLib.cRadioButton)sender).ForeColor = System.Drawing.Color.DeepPink;
            mStrPrintDosWin = Convert.ToString(((AxonContLib.cRadioButton)sender).Tag);
        }
    }
}